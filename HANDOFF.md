# HANDOFF — Better Stamina Regeneration

> 这份文件是给「新工作区 / 新会话」看的上下文摘要。项目从 PEAK 主模组
> 「巅峰升华 / Peak Ascension」里拆出来，只保留体力恢复相关功能。
> 当前工作区：`DSH_PEAK_mod`

## 工作区说明（重要）

本项目是**独立工作区**（本机路径不入库，克隆后即为工作区根目录）。

- 游戏反编译源码：`refs/game-decomp`（本地参考用，**已在 .gitignore 排除，绝不能提交**）
- 主模组「巅峰升华」：位于本工作区之外（只读参考，不要改）
- 构建产物、`dist/`、`refs/` 都不会进 git
## 项目信息

| 项 | 值 |
|---|---|
| 显示名 | Better Stamina Regeneration（更好的耐力恢复） |
| 包名 / 程序集 | `BetterStaminaRegen` |
| BepInEx GUID | `abuf.betterstaminaregen` |
| 作者 | ABUF |
| 目标框架 | `net472` |
| 当前版本 | 1.0.1 |
| 依赖 | BepInEx 5（插件本身无其它模组依赖） |
| GitHub 仓库 | <https://github.com/ssc123321/BetterStaminaRegen>（`main` 分支） |
| 雷霆商店 | <https://thunderstore.io/c/peak/p/ABUF/BetterStaminaRegen/> |

## 功能

1. **自然回体速度**：`SpeedBonus`，0 ~ 0.5，默认 0.5（+50%，上限 50%）
2. **停止消耗后等待时间**：`DelayReduction`，0 ~ 0.5，默认 0.5（等待时间减半，上限 50%）

配置节：`BepInEx/config/abuf.betterstaminaregen.cfg` → `[Stamina]`

## 钩子原理（重要）

游戏 `Character.UpdateVariablesFixed()` 里：

```csharp
data.sinceUseStamina += Time.fixedDeltaTime;
...
if (CanRegenStamina())
{
    AddStamina(Time.fixedDeltaTime * 0.2f);
}
```

本模组两个补丁：

- `RegenContextPatch`（`Character.UpdateVariablesFixed` 前后缀）
  - 进入方法时把静态标记 `InNaturalRegen = true`
  - 同时按 `DelayReduction` 给 `sinceUseStamina` 额外累加"虚拟时间"：
    `extra = dt * r / (1 - r)`（r = 0.5 时正好翻倍，等待时间减半）
  - 退出时复位标记
- `RegenSpeedPatch`（`Character.AddStamina` 前缀）
  - 只在 `InNaturalRegen == true` 且是本地角色时放大 `add`
  - 食物、篝火、士气等其它回体来源不受影响

`CanRegenStamina()` 原版逻辑：`sinceUseStamina` 要超过 `1 秒`（体力 > 0）或 `2 秒`（体力为 0），
并且要处于地面/未被蛛网缠住。

## 构建

仓库不含游戏二进制，构建前设置环境变量：

```powershell
$env:PEAK_GAME_DIR     = "…\steamapps\common\PEAK"
$env:PEAK_BEPINEX_CORE = "…\BepInEx\core"     # 含 BepInEx.dll / 0Harmony.dll
./build.ps1
```

- `dotnet build -c Release` 即可编译（`net472` 引用程序集来自 NuGet 包
  `Microsoft.NETFramework.ReferenceAssemblies 1.0.3`，已在本机缓存，**不需要**装
  .NET Framework 4.7.2 Developer Pack）
- `build.ps1` 会产出 `dist/BetterStaminaRegen-<版本>.zip`，结构为
  `BepInEx/plugins/BetterStaminaRegen/BetterStaminaRegen.dll`，可直接上传雷霆商店
- 雷霆商店的介绍页正文 = **包内 `README.md`**；`build.ps1` 有 `DESCRIPTION.md` 时
  就把 `DESCRIPTION.md` 打包成 `README.md`（中文在前、英文在后），仓库根目录的
  `README.md` 只给 GitHub 看
- 打包时**不包含** pdb（pdb 里带本机源码路径）、bin、obj、dist

## 发布前检查

- `manifest.json` 的 `website_url` 已是真实 GitHub 仓库地址
- `icon.png` 必须是 256×256
- 版本号同时改 `manifest.json`、`csproj`、`Plugin.PluginVersion`、`CHANGELOG.md`
- 确认 zip 里没有任何本机路径 / 用户名 / 邮箱

## 本机开发环境（仅本地参考，不要写进仓库）

- 游戏：`<Steam 库>\steamapps\common\PEAK`（含 `PEAK.exe`）
- BepInEx core（Gale profile）：`<Gale 数据目录>\peak\profiles\<profile>\BepInEx\core`
- 反编译的游戏源码：`refs/game-decomp/`
- 原来的主模组（只读参考，不要改）：与本工作区并列的 `PEAK-PeakAscension`
- 以上路径一律通过环境变量 `PEAK_GAME_DIR` / `PEAK_BEPINEX_CORE` 传入，仓库里不写死

## 已知坑

- `Character` 继承自 Photon 的 `MonoBehaviourPun`，所以 csproj 必须引用
  `PhotonUnityNetworking / PhotonRealtime / Photon3Unity3D`，否则 `CS0012`。
- `net472` 编译需要 `Microsoft.NETFramework.ReferenceAssemblies`；如果换机器且没网，
  才需要装 .NET Framework 4.7.2 Developer Pack。
- `Character.AddStamina` 会被多处调用（食物/士气/被动），务必保留"仅自然回体"的上下文判断，
  否则会把所有回体一起放大。
