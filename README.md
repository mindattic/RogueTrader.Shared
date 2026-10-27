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
| `RogueTrader.OwlcatMod.targets` | (via Mod.props) | with `OwlcatModification=true`, fails the build if the installed mod lacks `Bundles` or the settings bundle sections (see below) |
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

## Owlcat-format mods: three mistakes that stop the game

Rogue Trader loads mods two ways. Unity Mod Manager mods (like RogueTrader.Brevity) live in
`UnityModManager\<Id>` with an `Info.json`. Owlcat's own format (like RogueTrader.Overwatch) lives
in `Modifications\<Name>` with `OwlcatModificationManifest.json` and `OwlcatModificationSettings.json`.
An Owlcat-format mod must get all three right, and none of them fails until the game runs:

1. **A `Bundles` folder, even when empty.** The loader lists it without checking that it exists. The
   exception skips loading every blueprint in the mod, so no ability or item from it appears.
   Create it in the install target, and ship it with `<PackageDirectory Include="Bundles" />`.
2. **The bundle sections in `OwlcatModificationSettings.json`, even when empty:**
   ```json
   "BundlesLayout": { "m_Guids": [], "m_Bundles": [] },
   "BundleDependencies": { "m_List": [] },
   ```
   The loader reads the file with Unity's `JsonUtility`, which leaves missing sections null. Every
   scene load then asks each mod for bundles, and the game hangs on its first loading screen
   (`NullReferenceException` in `OwlcatModification.TryLoadBundleAsync` in `Player.log`).

3. **Enum values as strings in component patches.** A `*.jbp_patch` component (`ComponentsPatches`'
   `FieldValue`) is read with a strict converter that only accepts enum values as text, unlike `.jbp`
   files, which also accept numbers. A number such as `"m_Flags": 0` stops the game with "Cannot read
   Integer as enum value" when the patched blueprint first loads, usually while loading a save. Write
   `"m_Flags": "0"`, as Owlcat's own example patches do. Also use a real `Author` in `.jbp` files
   (`"Unknown"`): it's an enum of Owlcat staff names, and anything else logs an exception per blueprint.

Set `<OwlcatModification>true</OwlcatModification>` in the mod's csproj and
`RogueTrader.OwlcatMod.targets` checks all three after every install, failing the build with the fix
spelled out. The template's own build (Unity) produces these; a hand-built mod has to add them.
