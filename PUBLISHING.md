# 发布清单（GitHub + Thunderstore）

> 本文件给「新工作区 / 新会话」用。代码与打包已就绪，下面只剩账号与发布动作。

## 0. 需要替换的占位符

| 位置 | 当前内容 | 要改成 |
|---|---|---|
| `manifest.json` → `website_url` | `https://github.com/ABUF/BetterStaminaRegen` | 你的最终 GitHub 仓库地址 |
| `README.md` → 构建说明 | 无个人信息 | 检查一遍，不要写本机路径 |

---

## 1. GitHub 用户名与隐私

1. GitHub → **Settings → Account → Change username**
   - 旧用户名会释放，GitHub 对旧用户名/旧仓库链接保留一段时间重定向
   - 改完后本地仓库的 remote 需要更新：`git remote set-url origin <新地址>`
2. **隐私加固**
   - Settings → Emails → 勾选 **Keep my email addresses private**
   - 使用 GitHub 提供的 noreply 邮箱：`<ID>+<username>@users.noreply.github.com`
   - 本地配置：
     ```
     git config user.name "ABUF"
     git config user.email "<ID>+<username>@users.noreply.github.com"
     ```
3. 如果旧提交已经推过且含真实邮箱，用 `git filter-repo --mail-map` 重写历史后再强推；
   本项目是全新仓库，直接按上面配置即可，无需重写。

---

## 2. 创建仓库并首次推送

```powershell
cd DSH_PEAK_mod
git init
git config user.name "ABUF"
git config user.email "<ID>+<username>@users.noreply.github.com"

git add .
git commit -m "Initial release: Better Stamina Regeneration 1.0.0"

git branch -M main
git remote add origin https://github.com/<username>/BetterStaminaRegen.git
git push -u origin main
```

> 仓库名建议 `BetterStaminaRegen`，与 `manifest.json` 的 `name` 保持一致。

---

## 3. 本地构建与打包

```powershell
$env:PEAK_GAME_DIR     = "…\steamapps\common\PEAK"
$env:PEAK_BEPINEX_CORE = "…\BepInEx\core"
./build.ps1
```

产物：`dist/BetterStaminaRegen-1.0.0.zip`

打包内容（已确认无 pdb、无本机路径）：

```
BepInEx/plugins/BetterStaminaRegen/BetterStaminaRegen.dll
manifest.json
icon.png
README.md
CHANGELOG.md
LICENSE
```

---

## 4. Thunderstore 上传

1. 打开 <https://thunderstore.io/c/peak/>，登录并选择团队（Team）
2. **Upload** → 选择 `dist/BetterStaminaRegen-1.0.0.zip`
3. 填写：
   - **Name**：`BetterStaminaRegen`
   - **Version**：`1.0.0`（必须与 manifest 一致）
   - **Description**：粘贴 `DESCRIPTION.md` 里的钩子三行 + 中英文正文
   - **Categories**：`AI Generated`、`Client-side`、`Quality of Life`
4. 提交后 PEAK 社区**通常需要管理员审核**才公开，审核期间可在你的 profile 看到

## 5. 每次发新版本

按顺序改 **4 处**版本号，缺一处会出问题：

1. `manifest.json` → `version_number`
2. `BetterStaminaRegen.csproj` → `<Version>`
3. `src/Plugin.cs` → `PluginVersion`
4. `CHANGELOG.md` 增加一节

然后重新 `./build.ps1`，上传新的 zip。Thunderstore **不接受同版本号覆盖**。

---

## 6. 发布前自检

- [ ] `manifest.json` 的 `website_url` 已更新
- [ ] `icon.png` 是 256×256
- [ ] zip 内无 `bin/ / obj/ / *.pdb`
- [ ] DLL 与文档里搜不到 `C:\Users`、`AppData`、用户名、邮箱
- [ ] 游戏里实测过：体力回体变快、等待变短
- [ ] README 的构建说明可在别人的机器上照做（环境变量，不是本机路径）
