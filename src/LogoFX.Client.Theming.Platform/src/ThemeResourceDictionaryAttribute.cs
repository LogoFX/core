using System;

namespace LogoFX.Client.Theming
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ThemeResourceDictionaryAttribute : Attribute
    {
        public string ParentThemeName { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}