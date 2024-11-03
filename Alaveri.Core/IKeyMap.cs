using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Alaveri.Core
{
    public interface IKeyMap<TKeyGesture> where TKeyGesture : class
    {
        TKeyGesture? this[string index] { get; }

        IDictionary<string, KeyBinding<TKeyGesture>> CustomMapping { get; set; }
        ObservableCollection<KeyBinding<TKeyGesture>> Mapping { get; set; }
    }
}