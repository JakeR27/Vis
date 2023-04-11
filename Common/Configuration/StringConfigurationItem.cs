namespace Vis.Common.Configuration;

public class StringConfigurationItem : ConfigurationItem<string>
{
    public StringConfigurationItem(string environmentVariable, string defaultValue) : base(environmentVariable, defaultValue)
    {
    }

    protected override string _value(string environmentVariableContent)
    {
        return environmentVariableContent.Trim();
    }
}