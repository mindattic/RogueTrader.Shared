using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.Text;

namespace RogueTrader.Shared
{
    // Reads the game's blueprints (*.jbp, JSON) straight from Owlcat's modding template archive,
    // <game>\Modding\WhRtModificationTemplate.tar, or from an already extracted Blueprints folder.
    internal static class BlueprintArchive
    {
        // Yields each blueprint's path relative to the Blueprints folder (forward slashes) and its JSON.
        public static IEnumerable<(string Path, string Json)> Read(string source)
        {
            if (Directory.Exists(source))
            {
                foreach (var f in Directory.EnumerateFiles(source, "*.jbp", SearchOption.AllDirectories))
                    yield return (System.IO.Path.GetRelativePath(source, f).Replace('\\', '/'), File.ReadAllText(f));
                yield break;
            }

            using var fs = File.OpenRead(source);
            using var tar = new TarReader(fs);
            const string marker = "/Blueprints/";
            while (tar.GetNextEntry() is { } e)
            {
                var at = e.Name.IndexOf(marker, StringComparison.Ordinal);
                if (e.EntryType is not (TarEntryType.RegularFile or TarEntryType.V7RegularFile) || at < 0 || !e.Name.EndsWith(".jbp") || e.DataStream is null)
                    continue;
                using var r = new StreamReader(e.DataStream, Encoding.UTF8);
                yield return (e.Name[(at + marker.Length)..], r.ReadToEnd());
            }
        }
    }
}
