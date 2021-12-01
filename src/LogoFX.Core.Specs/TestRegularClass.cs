namespace LogoFX.Core.Specs;
public class TestRegularClass : TestClassBase
{
    public TestRegularClass()
    {
        NotifyPropertyChanged = new NotifyPropertyChangedCore<TestRegularClass>(this);
    }

    protected override NotifyPropertyChangedCore<TestRegularClass> NotifyPropertyChanged { get; }

    private int _number;

    public override int Number
    {
        get => _number;
        set => NotifyPropertyChanged.SetProperty(ref _number, value);
    }
}