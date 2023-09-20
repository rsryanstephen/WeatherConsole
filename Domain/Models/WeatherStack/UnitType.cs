namespace Domain.Models.WeatherStack;

public sealed class UnitType
{
    private readonly string _value;
    
    public static readonly UnitType Metric = new UnitType("m");
    public static readonly UnitType Scientific =  new UnitType("s");
    public static readonly UnitType Fahrenheit =  new UnitType("f");
    
    private UnitType(string value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value;
    }
}