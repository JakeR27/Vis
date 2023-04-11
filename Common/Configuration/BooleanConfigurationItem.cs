namespace Vis.Common.Configuration;

public class BooleanConfigurationItem : ConfigurationItem<bool>
{
    public BooleanConfigurationItem(string environmentVariable, bool defaultValue) : base(environmentVariable, defaultValue)
    {
    }

    protected override bool _value(string environmentVariableContent)
    {
        return environmentVariableContent.ToLower().Trim() switch
        {
            "true" => true,
            "1" => true,
            "false" => false,
            "0" => false,
            _ => _defaultValue
        };
    }
}