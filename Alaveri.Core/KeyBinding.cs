using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alaveri.Core;

public class KeyBinding<TKeyGesture>(TKeyGesture keyGesture, ICommand command) where TKeyGesture : class
{
    public TKeyGesture KeyGesture { get; set; } = keyGesture;

    [JsonIgnore]
    public ICommand Command { get; private set; } = command;

    public string CommandName
    {
        get
        {
            var command = KeyMap.AvailableCommands
                .ToList()
                .FirstOrDefault(binding => binding.Value == Command);
            return command.Key ?? string.Empty;
        }
        set
        {
            if (value == null)
                return;
            KeyMap.AvailableCommands.TryGetValue(value, out var command);
            if (command != null)
                Command = command;
        }
    }
}
