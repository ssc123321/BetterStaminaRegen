# Better Stamina Regeneration

A small PEAK mod that makes stamina come back faster.

Created by **ABUF**.

## Features

- **Faster regeneration** — natural stamina regeneration is up to **50% faster**.
- **Shorter wait** — the delay before regeneration starts after using stamina is up to **50% shorter**.

Both values are configurable in the BepInEx config file, and both are capped at 50%.
By default the mod is already at the maximum (50% faster / 50% shorter wait).

## Config

After the first launch, edit:

```
BepInEx/config/abuf.betterstaminaregen.cfg
```

| Key | Default | Range | Meaning |
|---|---|---|---|
| `Stamina.SpeedBonus` | `0.5` | `0` – `0.5` | Extra natural regeneration speed (0.5 = +50%) |
| `Stamina.DelayReduction` | `0.5` | `0` – `0.5` | Shorter wait before regeneration (0.5 = -50%) |

Set both to `0` to get vanilla behaviour without uninstalling the mod.

## How it works

PEAK regenerates stamina in `Character.UpdateVariablesFixed`:

```csharp
if (CanRegenStamina())
{
    AddStamina(Time.fixedDeltaTime * 0.2f);
}
```

The mod only scales that specific `AddStamina` call, so food, campfire and morale
stamina bonuses are **not** affected. The wait is shortened by adding virtual time to
`data.sinceUseStamina`, which the vanilla check compares against 1 s / 2 s.

## Installation

1. Install **BepInEx 5** for PEAK.
2. Drop the `BepInEx` folder from the release zip into the game folder
   (merge with the existing one).
3. Launch the game once to generate the config file.

## Building from source

The repo does not contain game binaries. Set two environment variables first:

```powershell
$env:PEAK_GAME_DIR     = "C:\path\to\PEAK"                      # contains PEAK.exe
$env:PEAK_BEPINEX_CORE = "C:\path\to\BepInEx\core"              # contains BepInEx.dll, 0Harmony.dll
dotnet build -c Release
```

Then run `./build.ps1` to produce `dist/BetterStaminaRegen-1.0.0.zip`, which is the
Thunderstore-ready package.

## License

MIT — see `LICENSE`.
