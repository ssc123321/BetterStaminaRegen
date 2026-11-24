# 发布清单（GitHub + Thunderstore）

> 本文件给「新工作区 / 新会话」用。代码与打包已就绪，下面只剩账号与发布动作。

## 0. 仓库信息（已确定）

| 项 | 值 |
|---|---|
| GitHub 账号 | `ssc123321`（改名方案已放弃：`ABUF` 已被他人占用） |
| 仓库 | <https://github.com/ssc123321/BetterStaminaRegen> |
| `manifest.json` → `website_url` | 同上，已写入 |
| 提交身份 | `ABUF <148782431+ssc123321@users.noreply.github.com>` |
| 默认分支 | `main` |

---

## 1. GitHub 用户名与隐私

1. 用户名保持 `ssc123321`（`ABUF` 已被 2017 年注册的另一个账号占用，不可用）。
   - 如果将来仍想改名，注意：旧用户名会被释放，别人可能抢注，所有仓库链接都会变，
     noreply 邮箱的 `+<username>` 部分也会跟着变。
2. **隐私加固**
   - Settings → Emails → 勾选 **Keep my email addresses private**（本账号已勾选）
   - 提交只用 GitHub 提供的 noreply 邮箱：`<ID>+<username>@users.noreply.github.com`
   - 本地配置（仓库级，不写全局）：
     ```
     git config user.name "ABUF"
     git config user.email "148782431+ssc123321@users.noreply.github.com"
     ```
3. 本项目是全新仓库，首个提交从零开始，不含任何真实邮箱，无需重写历史。

---

## 2. 创建仓库并首次推送

仓库已创建并完成首次推送（提交 `Initial release: Better Stamina Regeneration 1.0.0`）。

以后新机器上的完整流程：

```powershell
git init -b main
git config user.name "ABUF"
git config user.email "148782431+ssc123321@users.noreply.github.com"

git add -A
git commit -m "Initial release: Better Stamina Regeneration 1.0.0"
git remote add origin https://github.com/ssc123321/BetterStaminaRegen.git
git push -u origin main
```

> 仓库名 `BetterStaminaRegen`，与 `manifest.json` 的 `name` 保持一致。

**推送认证**（这台机器没有存任何 GitHub 凭据，也没有 SSH key）：

- 临时方案：在 GitHub → Settings → Developer settings → Personal access tokens (classic)
  建一个 `public_repo` 权限、短期有效的 token，推送时用
  `git push https://x-access-token:<TOKEN>@github.com/ssc123321/BetterStaminaRegen.git main:main`，
  推完立刻删除 token（不要写进 `.git/config`）。
- 长期方案：配一次 Git Credential Manager 或 SSH key，之后 `git push` 免密。

**网络**：直连 `github.com:443` 会被重置，需要走本机代理（系统代理），
并让 git 用 OpenSSL 后端，例如：
`git -c http.proxy=http://127.0.0.1:<port> -c http.sslBackend=openssl push <url> main:main`

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

1. 打开 <https://thunderstore.io/package/create/>，登录
2. **Upload file** → 选择 `dist/BetterStaminaRegen-<版本>.zip`
3. 填写：
   - **Team**：`ABUF`（首次上传前要先在 <https://thunderstore.io/teams/> 建团队；
     页面上 “No teams available?” 只是常驻提示，下拉里其实有 ABUF）
   - **Communities**：`PEAK`
   - **Categories**：`AI Generated`、`Client Side`、`Quality Of Life`
   - **NSFW**：`No`
4. 提交后如果版本号已存在会被拒绝；PEAK 社区可能需要审核后才公开

> 新界面**没有** Description 输入框：商店页正文取的是包内 `README.md`。
> `build.ps1` 已经把 `DESCRIPTION.md` 打包成 `README.md`，所以只要文案写进
> `DESCRIPTION.md` 即可，中英文都在里面，中文在前。

## 5. 每次发新版本

按顺序改 **4 处**版本号，缺一处会出问题：

1. `manifest.json` → `version_number`
2. `BetterStaminaRegen.csproj` → `<Version>`
3. `src/Plugin.cs` → `PluginVersion`
4. `CHANGELOG.md` 增加一节

然后重新 `./build.ps1`，上传新的 zip。Thunderstore **不接受同版本号覆盖**。

---

## 6. 发布前自检

- [x] `manifest.json` 的 `website_url` 已指向真实仓库
- [x] `icon.png` 是 256×256
- [ ] zip 内无 `bin/ / obj/ / *.pdb`
- [ ] DLL 与文档里搜不到 `C:\Users`、`AppData`、用户名、邮箱
- [ ] 游戏里实测过：体力回体变快、等待变短
- [ ] README 的构建说明可在别人的机器上照做（环境变量，不是本机路径）
