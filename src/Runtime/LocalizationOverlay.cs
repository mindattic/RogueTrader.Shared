using System.Collections.Generic;
using Kingmaker.Localization.Enums;
using Kingmaker.Localization.Shared;

namespace RogueTrader.Shared
{
    // Replaces strings in the loaded localization pack in memory, and puts the originals back when switched off.
    // The game reads every LocalizedString from the pack on each access, so a change shows immediately.
    internal sealed class LocalizationOverlay
    {
        private readonly Dictionary<string, string> m_Replacements;
        private readonly Dictionary<string, string> m_Originals = new Dictionary<string, string>();
        private readonly Locale m_Locale;

        public LocalizationOverlay(Dictionary<string, string> replacements, Locale locale = Locale.enGB)
        {
            m_Replacements = replacements;
            m_Locale = locale;
        }

        public int Count => m_Replacements.Count;

        // Returns how many strings were written, or -1 if the pack isn't this overlay's locale.
        public int Apply(LocalizationPack pack, bool enabled)
        {
            if (pack == null || pack.Locale != m_Locale)
                return -1;
            var applied = 0;
            foreach (var entry in m_Replacements)
            {
                if (!m_Originals.TryGetValue(entry.Key, out var original))
                {
                    if (!pack.TryGetText(entry.Key, out original, false))
                        continue;
                    m_Originals[entry.Key] = original;
                }
                pack.PutString(entry.Key, enabled ? entry.Value : original);
                applied++;
            }
            return applied;
        }
    }
}
