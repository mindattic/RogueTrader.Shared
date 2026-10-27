---
codex: 1
project: RogueTrader.Shared
code: RTS
layer: stories
status: living
updated: 2026-10-26
---
<!-- GENERATED from MindAttic.Tickets/data/Tickets.db by `tickets db render`. Do not edit: this file is rewritten from the database; change stories and tickets with the tickets CLI. -->

# RogueTrader.Shared — User Stories
> ✅ done (shipped & tested) · 🟡 partial · ⬜ planned. Every ✅ cites the test.

## Priority backlog
_Nothing recorded yet._

## Tickets
> Permanent log of every change and fix — this repo's ticket system. Rows are append-only: never
> delete or reword one; only move its Status (open → closed / wont-do) and fill in Ref. Status is
> a word, not a glyph, so tickets don't count toward the story index. Keep this section last.

| ID | Date | Type | Status | Summary | Ref |
|---|---|---|---|---|---|
| RTS-T1 | 2026-10-26 | docs | closed | Start the permanent ticket log in the ticket database | — |
| RTS-T2 | 2026-10-26 | feature | closed | Create RogueTrader.Shared: source-shared build setup and helpers for Rogue Trader mods (Brevity, Overwatch, future ones) | 311f523 |
| RTS-T3 | 2026-10-26 | fix | closed | Release zips drop empty folders an Owlcat mod needs (Bundles), and mods built from a worktree can't find RogueTrader.Shared | bf259fb |
| RTS-T4 | 2026-10-26 | fix | closed | Guard Owlcat-format mods against the two startup traps (missing Bundles folder, missing bundle sections in OwlcatModificationSettings.json): README note plus a build check | 0bdd788 |
| RTS-T5 | 2026-10-26 | fix | closed | Guard against a third Owlcat-mod trap: component patches are read strictly, so a numeric enum like "m_Flags": 0 stops the game on save load | a2be2d7 |
