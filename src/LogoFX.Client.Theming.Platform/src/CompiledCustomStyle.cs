using System;
using System.Diagnostics;
using System.Windows;

namespace LogoFX.Client.Theming
{
    internal sealed class CompiledCustomStyle : CustomStyleBase
    {
        #region Fields

        private readonly Type _resourceDictionaryType;

        #endregion

        #region Constructors

        public CompiledCustomStyle(string name, Type resourceDictionaryType)
            : base(name)
        {
            _resourceDictionaryType = resourceDictionaryType;
        }

        #endregion

        #region Overrides

        protected override ResourceDictionary CreateResourceDictionary()
        {
            var ctor = _resourceDictionaryType.GetConstructor(Type.EmptyTypes);
            Debug.Assert(ctor != null, nameof(ctor) + " != null");
            return (ResourceDictionary) ctor.Invoke(new object[] { });
        }

        #endregion
    }
}