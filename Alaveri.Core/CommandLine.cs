namespace Alaveri.Core;

public class CommandLine
{
    public IList<CommandLineSwitch> Switches { get; }

    public IList<string> Arguments { get; }

    public static bool IsSwitch(string arg)
    {
        return arg.StartsWith('-') || arg.StartsWith('/') || arg.StartsWith("--");
    }

    public void ParseArguments(IList<string> args)
    {
        if (args == null || args.Count == 0)
            return;

        var index = 1;
        while (index < args.Count)
        {
            var arg = args[index];
            if (IsSwitch(arg))
            {
                var command = new CommandLineSwitch(arg);
                Switches.Add(command);
                index++;
                while (index < args.Count && !IsSwitch(args[index]))
                {
                    command.Values.Add(args[index]);
                    index++;
                }
            }
            else
                Arguments.Add(arg);
        }
    }

    public bool HasSwitch(IEnumerable<string> switchValues)
    {
        return Switches.Any(item => switchValues.Contains(item.Switch, StringComparer.OrdinalIgnoreCase));
    }

    public CommandLine(params string[] args)
    {
        Switches = [];
        Arguments = args;
        ParseArguments(args);
    }
}
