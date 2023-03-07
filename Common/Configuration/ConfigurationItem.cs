namespace Vis.Common.Configuration;

public abstract class ConfigurationItem<TItem>
{
    protected string _environmentVariable;
    protected TItem _defaultValue;

    protected ConfigurationItem(string environmentVariable, TItem defaultValue)
    {
        _environmentVariable = environmentVariable;
        _defaultValue = defaultValue;
        
        Logs.LogDebug($"Configuration item ({environmentVariable} : {typeof(TItem)}) created with default {defaultValue}");
    }

    private string? GetFromEnvironment()
    {
        return Environment.GetEnvironmentVariable(_environmentVariable);
    }

    public TItem Value()
    {
        var env = GetFromEnvironment();
        var output = env is null ? _defaultValue : _value(env);
        Logs.LogDebug($"Configuration item ({_environmentVariable}) has value {output}");
        return output;
    }

    protected abstract TItem _value(string environmentVariableContent);

}