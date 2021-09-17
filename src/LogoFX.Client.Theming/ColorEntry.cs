using System;

namespace LogoFX.Client.Theming
{
    [Serializable]
    public sealed class ColorEntry : ResourceEntry
    {
        public ColorEntry()
            : base(null, null)
        {}
        
        public ColorEntry(string caption, object resourceKey, uint color)
            : base(resourceKey, color)
        {
            Caption = caption;
        }

        public uint Color
        {
            get { return (uint) Value; }
            set { Value = value; }
        }
    }
}