using System;

namespace LogoFX.Client.Core.Specs.Common
{
    public abstract class TestClassBase : NotifyPropertyChangedBase<TestClassBase>
    {        
        public abstract int Number { get;
            set;
        }

        public void Refresh()
        {
            NotifyOfPropertiesChange();
        }

        public void UpdateSilent(Action action)
        {
            using (SuppressNotify)
            {
                action();
            }
        }
    }       
}