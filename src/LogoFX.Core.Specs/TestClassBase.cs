using System.ComponentModel;
namespace LogoFX.Core.Specs;

public abstract class TestClassBase : INotifyPropertyChanged
{
    protected abstract INotifyPropertyChanged NotifyPropertyChanged { get; }

    public abstract int Number
    {
        get;
        set;
    }

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => NotifyPropertyChanged.PropertyChanged += value;
        remove => NotifyPropertyChanged.PropertyChanged -= value;
    }

    public void UpdateSilent(Action action)
    {
        using (((ISuppressNotify)NotifyPropertyChanged).SuppressNotify)
        {
            action();
        }
    }
}

