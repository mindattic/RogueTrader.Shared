# RogueTrader.Shared

Build setup and helpers shared by MindAttic's Warhammer 40,000: Rogue Trader mods
([RogueTrader.Brevity](https://github.com/mindattic/RogueTrader.Brevity),
[RogueTrader.Overwatch](https://github.com/mindattic/RogueTrader.Overwatch) and any later ones).

It's shared **source**, not a DLL: each mod compiles these files into its own assembly, as internal
types in the `RogueTrader.Shared` namespace. Several mods run in one game process, and a shared DLL
would be loaded once, so mods built against different versions of it would break each other.

Check it out next to the mods (`D:\Projects\MindAttic\RogueTrader.Shared`); they import it by
relative path, or set `-p:RogueTraderShared=<path>`.

## What's in it

| File | Import from | Gives you |
|---|---|---|
| `RogueTrader.Game.props` | anything | `GameDir`, `Managed`, `GameDataDir`, `ModTemplateTar` (override with `-p:Name=value`) |
| `RogueTrader.Mod.props` | a mod's csproj (net48) | the above, a reference to every game assembly except the framework ones, `src/Runtime/*.cs`, and release packaging |
| `RogueTrader.Package.targets` | (via Mod.props) | `dotnet build -c Release` zips `$(ModInstallDir)` to `dist/$(ModFolderName)-$(ModVersion).zip`, after the mod's `InstallMod` target |
| `RogueTrader.Tools.props` | a command-line tool (net8+) | game paths and `src/Tools/*.cs` |

Runtime helpers (`src/Runtime`):
- `ModLog`: the game's mod log (`Player.log`, `PFLog.Mods`) with lines tagged by `ModLog.Name`.
- `Sprites.FromPng(path)`: a `Sprite` from a PNG on disk, for icons without a Unity asset bundle.
- `LocalizationOverlay`: swaps strings in the loaded localization pack in memory and restores the
  originals when switched off.

Tool helpers (`src/Tools`):
- `BlueprintArchive.Read(source)`: every blueprint's path and JSON, straight from Owlcat's modding
  template (`<game>\Modding\WhRtModificationTemplate.tar`) or an extracted Blueprints folder.

## Using it in a mod

```xml
<PropertyGroup>
  <RogueTraderShared Condition="'$(RogueTraderShared)' == ''">$(MSBuildThisFileDirectory)..\..\..\RogueTrader.Shared</RogueTraderShared>
</PropertyGroup>
<Import Project="$(RogueTraderShared)\RogueTrader.Mod.props" />
<PropertyGroup>
  <ModFolderName>MyMod</ModFolderName>
  <ModInstallDir>$(GameDataDir)\Modifications\$(ModFolderName)</ModInstallDir>
  <ModVersion>1.0.0</ModVersion>
</PropertyGroup>
<Target Name="InstallMod" AfterTargets="Build"> ... copy the mod's files to $(ModInstallDir) ... </Target>
```

Keep a helper here only once a second mod needs it, or it's clearly game plumbing every mod will need.
