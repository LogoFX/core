using System.ComponentModel;

namespace LogoFX.Core.Specs;

public class TestNameClass : TestClassBase
{
    protected readonly NotifyPropertyChangedCore<TestNameClass> NotifyPropertyChanged;

    public TestNameClass()
    {
        NotifyPropertyChanged = new NotifyPropertyChangedCore<TestNameClass>(this);
    }

    private int _number;
    public override int Number
    {
        get => _number;
        set
        {
            _number = value;
            NotifyPropertyChanged.OnPropertyChanged();
        }
    }

    public override event PropertyChangedEventHandler? PropertyChanged
    {
        add => NotifyPropertyChanged.PropertyChanged += value;
        remove => NotifyPropertyChanged.PropertyChanged -= value;
    }

    public override void UpdateSilent(Action action)
    {
        using (NotifyPropertyChanged.SuppressNotify)
        {
            action();
        }
    }
}