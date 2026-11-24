using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace ABUF.BetterStaminaRegen
{
    [BepInPlugin(Guid, PluginName, PluginVersion)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string Guid = "abuf.betterstaminaregen";
        public const string PluginName = "Better Stamina Regeneration";
        public const string PluginVersion = "1.0.1";

        public static Plugin Instance;
        public static ManualLogSource Log;

        /// <summary>自然回体速度加成（0 ~ 0.5，即最多快 50%）。</summary>
        public static ConfigEntry<float> SpeedBonus;

        /// <summary>停止消耗后等待时间的缩短比例（0 ~ 0.5，即最多快 50% 开始回体）。</summary>
        public static ConfigEntry<float> DelayReduction;

        private static readonly Harmony Harmony = new Harmony(Guid);

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            SpeedBonus = Config.Bind(
                "Stamina",
                "SpeedBonus",
                0.5f,
                new ConfigDescription(
                    "自然回体速度加成。0 = 原版，0.5 = 快 50%（默认，也是上限）。",
                    new AcceptableValueRange<float>(0f, 0.5f)));

            DelayReduction = Config.Bind(
                "Stamina",
                "DelayReduction",
                0.5f,
                new ConfigDescription(
                    "停止消耗体力后等待时间的缩短比例。0 = 原版，0.5 = 等待时间减半（默认，也是上限）。",
                    new AcceptableValueRange<float>(0f, 0.5f)));

            SpeedBonus.Value = Clamp01(SpeedBonus.Value, 0.5f);
            DelayReduction.Value = Clamp01(DelayReduction.Value, 0.5f);

            Harmony.PatchAll();

            Log.LogInfo(PluginName + " " + PluginVersion + " loaded. speed +"
                + SpeedBonus.Value * 100f + "%, delay -" + DelayReduction.Value * 100f + "%");
        }

        private void OnDestroy()
        {
            Harmony.UnpatchSelf();
        }

        private static float Clamp01(float value, float max)
        {
            if (value < 0f)
            {
                return 0f;
            }
            return value > max ? max : value;
        }
    }
}
