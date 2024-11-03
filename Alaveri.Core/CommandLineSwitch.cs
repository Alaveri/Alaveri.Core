namespace Alaveri.Core;

public class CommandLineSwitch(string switchName, params string[] values)
{
    public string? Switch { get; } = switchName;

    public IList<string> Values { get; } = values;
}
