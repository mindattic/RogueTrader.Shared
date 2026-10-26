using System;
using Kingmaker;

namespace RogueTrader.Shared
{
    // The game's mod log (Player.log, PFLog.Mods), with every line tagged by the mod's name.
    internal static class ModLog
    {
        public static string Name = "Mod";

        public static void Log(string message) => PFLog.Mods.Log(Name + ": " + message);

        public static void Error(string message) => PFLog.Mods.Error(Name + ": " + message);

        public static void Exception(Exception ex, string doing) => PFLog.Mods.Exception(ex, Name + ": " + doing);
    }
}
