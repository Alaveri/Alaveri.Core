using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Alaveri.Core;

public static class KeyMap
{
    public static IDictionary<string, ICommand> AvailableCommands { get; set; } = new Dictionary<string, ICommand>();
}

public class KeyMap<TKeyGesture>() : IKeyMap<TKeyGesture> where TKeyGesture : class
{
    [JsonIgnore]
    public ObservableCollection<KeyBinding<TKeyGesture>> Mapping { get; set; } = [];

    [JsonIgnore]
    public TKeyGesture? this[string index] => Mapping
        .First(binding => binding.CommandName == index).KeyGesture;

    public IDictionary<string, KeyBinding<TKeyGesture>> CustomMapping
    {
        get => Mapping.ToDictionary(binding => binding.CommandName);
        set
        {
            var bindings = value
                .Join(Mapping,
                    custom => custom.Value.Command,
                    mapping => mapping.Command,
                    (NewValue, ExistingValue) => new { NewValue, ExistingValue })
                .ToList();
            bindings.ForEach(binding => binding.ExistingValue.KeyGesture = binding.NewValue.Value.KeyGesture);
        }
    }
}

