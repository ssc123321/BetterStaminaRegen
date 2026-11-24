# 雷霆商店介绍页文案

> 复制到 Thunderstore 的 **Description**（Markdown 支持）。
> 开头的三行是钩子文案，下面的中英文介绍按顺序粘贴。

---

## 中文

队友总是跑得太快，一不留神就消失不见。
你跟不上他们的脚步，只能留在原地苦等体力恢复。
**——消除无意义的等待时间。**

### 原版是怎么回体的？

PEAK 的体力恢复有两个阶段，都由游戏写死在代码里：

| 情况 | 原版等待时间 | 原版回体速度 |
|---|---|---|
| 体力条还没空 | 停止消耗后 **1 秒** 开始回体 | 0.2 / 秒 |
| 体力完全耗尽 | 停止消耗后 **2 秒** 才开始回体 | 0.2 / 秒 |

所以在空体状态下失误一次，你至少要干站 2 秒。

### 这个模组做了什么？

| 项目 | 原版 | 满级（默认） |
|---|---|---|
| 等待时间（体力未空） | 1 秒 | **0.5 秒** |
| 等待时间（空体） | 2 秒 | **1 秒** |
| 自然回体速度 | 0.2 / 秒 | **0.3 / 秒（+50%）** |

两项都**最多只能调到 50%**，装好即默认满级，不需要额外设置。

### 配置

`BepInEx/config/abuf.betterstaminaregen.cfg`

| 配置项 | 默认 | 范围 | 说明 |
|---|---|---|---|
| `Stamina.SpeedBonus` | 0.5 | 0 – 0.5 | 自然回体速度加成，0.5 = 快 50% |
| `Stamina.DelayReduction` | 0.5 | 0 – 0.5 | 等待时间缩短比例，0.5 = 等待减半 |

把两项都改成 `0` 就是原版手感，不用卸载模组。

### 说明

- 只影响**自然回体**；食物、篝火、士气提供的额外体力不受影响
- 纯客户端模组，自己装就生效，不需要队友也装
- 支持多人游戏，不修改任何网络同步数据

---

## English

Your teammates are always faster than you. One moment they're ahead, the next
they're gone — and you're standing still, waiting for stamina to come back.
**No more pointless waiting.**

### How vanilla regeneration works

PEAK handles stamina recovery in two stages, both hard-coded:

| Situation | Vanilla delay | Vanilla regen |
|---|---|---|
| Stamina bar not empty | Regeneration starts **1 s** after you stop using stamina | 0.2 / s |
| Stamina fully drained | Regeneration starts **2 s** after you stop | 0.2 / s |

One bad move while exhausted and you stand there for two full seconds.

### What this mod does

| Value | Vanilla | Max (default) |
|---|---|---|
| Delay (bar not empty) | 1 s | **0.5 s** |
| Delay (fully drained) | 2 s | **1 s** |
| Natural regen speed | 0.2 / s | **0.3 / s (+50%)** |

Both values are capped at **50%**. The mod ships at the maximum; no setup needed.

### Config

`BepInEx/config/abuf.betterstaminaregen.cfg`

| Key | Default | Range | Meaning |
|---|---|---|---|
| `Stamina.SpeedBonus` | 0.5 | 0 – 0.5 | Extra natural regen speed (0.5 = +50%) |
| `Stamina.DelayReduction` | 0.5 | 0 – 0.5 | Shorter wait before regen (0.5 = -50%) |

Set both to `0` for vanilla behaviour without uninstalling the mod.

### Notes

- Only **natural regeneration** is affected; food, campfire and morale stamina bonuses are untouched
- Client-side mod: only you need to install it
- Multiplayer-safe: no network data is modified

---

## 分类选择（上传时勾选）

- **AI生成**（AI Generated）
- **客户端模组**（Client-side）
- **生活质量调整**（Quality of Life）
