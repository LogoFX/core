using System;
using System.Windows.Media;

namespace LogoFX.Client.Theming
{
    public static class ColorEntryUtils
    {
        public static Color ToColor(this ColorEntry colorEntry)
        {
            return ColorFromBytes(colorEntry.Color);
        }

        public static void FromColor(this ColorEntry colorEntry, Color color)
        {
            colorEntry.Color = ColorToBytes(color);
        }

        public static Color ColorFromBytes(uint n)
        {
            var bytes = BitConverter.GetBytes(n);
            var color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            return color;
        }

        public static uint ColorToBytes(Color color)
        {
            var bytes = new[] { color.B, color.G, color.R, color.A };
            var n = BitConverter.ToUInt32(bytes, 0);
            return n;
        }
    }
}