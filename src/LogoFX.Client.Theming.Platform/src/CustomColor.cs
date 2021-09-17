using System;
using System.Windows.Media;

namespace LogoFX.Client.Theming
{
    [Serializable]
    public sealed class CustomColor
    {
        public object Key { get; set; }

        public Color Color { get; set; }
    }
}