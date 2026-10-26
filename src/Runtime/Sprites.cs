using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace RogueTrader.Shared
{
    internal static class Sprites
    {
        // ImageConversion's assembly targets netstandard 2.1, which net48 can't compile against, so it's called by reflection.
        private static readonly MethodInfo LoadImage =
            Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule")
                ?.GetMethod("LoadImage", new[] { typeof(Texture2D), typeof(byte[]) });

        // A sprite from a PNG on disk, or null if the file is missing or isn't an image.
        public static Sprite FromPng(string path)
        {
            if (LoadImage == null || !File.Exists(path))
                return null;
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false) { name = Path.GetFileNameWithoutExtension(path) };
            if (!(bool)LoadImage.Invoke(null, new object[] { texture, File.ReadAllBytes(path) }))
                return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
