namespace Vis.Common.Configuration;

public class IntegerConfigurationItem : ConfigurationItem<int>
{
    public IntegerConfigurationItem(string environmentVariable, int defaultValue) : base(environmentVariable, defaultValue)
    {
    }

    protected override int _value(string environmentVariableContent)
    {
        try
        {
            return int.Parse(environmentVariableContent);
        }
        catch
        {
            return _defaultValue;
        }
    }
}