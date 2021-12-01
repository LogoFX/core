namespace LogoFX.Core.Specs;

public class TestAfterValueUpdateClass : TestClassBase
{
    public TestAfterValueUpdateClass()
    {
        NotifyPropertyChanged = new NotifyPropertyChangedCore<TestAfterValueUpdateClass>(this);
    }

    protected override NotifyPropertyChangedCore<TestAfterValueUpdateClass> NotifyPropertyChanged { get; }

    private int _number = 4;
    public override int Number
    {
        get => _number;
        set => NotifyPropertyChanged.SetProperty(ref _number, value, new SetPropertyOptions
        {
            AfterValueUpdate = () => _number = 6
        });
    }
}