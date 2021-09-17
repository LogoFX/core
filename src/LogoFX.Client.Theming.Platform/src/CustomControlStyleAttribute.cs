using System;

namespace LogoFX.Client.Theming
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomControlStyleAttribute : Attribute
    {
        public CustomControlStyleAttribute(string name, string id)
        {
            Name = name;
            Id = new Guid(id);
        }

        public Guid Id { get; set; }

        public string Name { get; private set; }
    }
}