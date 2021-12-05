namespace LogoFX.Core.Specs;

public class TestNameClass : TestClassBase
{
    public TestNameClass()
    {
        NotifyPropertyChanged = new NotifyPropertyChangedCore<TestNameClass>(this);
    }

    protected override NotifyPropertyChangedCore<TestNameClass> NotifyPropertyChanged { get; }

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
}