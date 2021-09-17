using System;

namespace LogoFX.Client.Theming
{
    [Serializable]
    public abstract class ResourceEntry
    {
        protected ResourceEntry(object resourceKey, object value)
        {
            ResourceKey = resourceKey;
            Value = value;
        }

        public object ResourceKey { get; set; }

        public object Value { get; protected set; }

        private string _caption;
        public string Caption
        {
            get { return _caption ?? ResourceKey.ToString(); }
            set { _caption = value; }
        }
    }
}