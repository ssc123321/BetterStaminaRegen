using HarmonyLib;
using UnityEngine;

namespace ABUF.BetterStaminaRegen
{
    /// <summary>
    /// 原版自然回体发生在 Character.UpdateVariablesFixed 里：
    ///   if (CanRegenStamina()) AddStamina(Time.fixedDeltaTime * 0.2f);
    /// 这里用"是否处于该方法调用中"作为上下文，避免把食物/士气等额外回体也放大。
    /// </summary>
    [HarmonyPatch(typeof(Character), "UpdateVariablesFixed")]
    internal static class RegenContextPatch
    {
        public static bool InNaturalRegen;

        [HarmonyPrefix]
        private static void Prefix(Character __instance)
        {
            InNaturalRegen = false;

            if (__instance == null || __instance.data == null || !__instance.IsLocal)
            {
                return;
            }

            // 等待时间缩短：原版用 sinceUseStamina 跟 1 秒 / 2 秒比较。
            // 想让它更快到阈值，就额外累加一段"虚拟时间"。
            float r = Plugin.DelayReduction.Value;
            if (r > 0f)
            {
                float clamped = Mathf.Clamp(r, 0f, 0.5f);
                float factor = clamped / Mathf.Max(0.01f, 1f - clamped);
                __instance.data.sinceUseStamina += Time.fixedDeltaTime * factor;
            }

            InNaturalRegen = true;
        }

        [HarmonyPostfix]
        private static void Postfix()
        {
            InNaturalRegen = false;
        }
    }

    /// <summary>只放大"自然回体"那一次 AddStamina，其它来源原样通过。</summary>
    [HarmonyPatch(typeof(Character), "AddStamina")]
    internal static class RegenSpeedPatch
    {
        [HarmonyPrefix]
        private static void Prefix(Character __instance, ref float add)
        {
            if (!RegenContextPatch.InNaturalRegen || add <= 0f)
            {
                return;
            }

            if (__instance == null || !__instance.IsLocal)
            {
                return;
            }

            float bonus = Mathf.Clamp(Plugin.SpeedBonus.Value, 0f, 0.5f);
            if (bonus > 0f)
            {
                add *= 1f + bonus;
            }
        }
    }
}
