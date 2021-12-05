namespace LogoFX.Core.Specs;

public class TestExpressionClass : TestClassBase
{
    public TestExpressionClass()
    {
        NotifyPropertyChanged = new NotifyPropertyChangedCore<TestExpressionClass>(this);
    }

    protected override NotifyPropertyChangedCore<TestExpressionClass> NotifyPropertyChanged { get; }

    private int _number;
    public override int Number
    {
        get => _number;
        set
        {
            _number = value;
            NotifyPropertyChanged.OnPropertyChanged(() => Number);
        }
    }
}