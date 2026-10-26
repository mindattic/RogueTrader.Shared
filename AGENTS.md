# MindAttic project agent entrypoint

Read the shared protocol at ..\mindattic-agent-standard\AGENTS.md and this project's own
documentation. Common prompt commands are implemented by the shared runner; do not add a second
copy under a provider-specific command folder.

## Project context

RogueTrader.Shared is shared build setup and source for MindAttic's Warhammer 40,000: Rogue Trader
mods (RogueTrader.Brevity, RogueTrader.Overwatch). See README.md.

- It's compiled into each mod as source (internal types), never shipped as its own DLL: mods share
  one game process, so a shared DLL would be loaded once for all of them.
- A change here affects every mod: build each consumer (`dotnet build` in RogueTrader.Brevity's
  `src/BrevityMod` and `tools/DialogueExport`, and RogueTrader.Overwatch's `src/RogueTrader.Overwatch`)
  before committing.
- Game install: `D:\Games\Steam\steamapps\common\Warhammer 40,000 Rogue Trader`.
