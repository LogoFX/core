using System;

namespace LogoFX.Client.Theming
{
    [Serializable]
    public sealed class ColorTheme
    {
        public string Name { get; set; }

        public ResourceEntry[] Entries { get; set; }
    }
}