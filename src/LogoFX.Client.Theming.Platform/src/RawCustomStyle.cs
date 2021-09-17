using System.Windows;
using System.Windows.Markup;

namespace LogoFX.Client.Theming
{
    internal class RawCustomStyle : CustomStyleBase
    {
        private readonly string _xamlText;

        public RawCustomStyle(string name, string xamlText)
            : base(name)
        {
            _xamlText = xamlText;
        }

        protected override ResourceDictionary CreateResourceDictionary()
        {
            var result = XamlReader.Parse(_xamlText);
            return (ResourceDictionary) result;
        }
    }
}