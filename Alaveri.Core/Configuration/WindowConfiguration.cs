using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Configuration;

/// <summary>
/// Represents a configuration containing the state of various windows.
/// </summary>
/// <typeparam name="TWindow">The type of window.</typeparam>
/// <typeparam name="TStoredWindowState">The type of StoredWindowState to use.</typeparam>
public class WindowConfiguration<TWindow, TStoredWindowState> : BaseConfiguration, IWindowConfiguration<TWindow, TStoredWindowState> where TWindow : class
    where TStoredWindowState : StoredWindowState<TWindow>, new()
{
    /// <summary>
    /// Gets the state of all windows.
    /// </summary>
    public IDictionary<string, TStoredWindowState> WindowStates { get; } =
        new Dictionary<string, TStoredWindowState>(StringComparer.InvariantCultureIgnoreCase);

    /// <summary>
    /// Gets the state of a window.
    /// </summary>
    /// <param name="index">The name of the window.</param>
    /// <returns>An object of type <see cref="TWindow"/> containing the window's state.</returns>
    public StoredWindowState<TWindow> this[string index] => WindowStates[index];

    /// <summary>
    /// Restores the state of a window.
    /// </summary>
    /// <param name="windowName">The name used to identify the window.</param>
    /// <param name="window">The window to restore.</param>
    public void RestoreWindowState(string windowName, TWindow window, float initialWidth = 1024f, float initialHeight = 768f)
    {
        if (!WindowStates.TryGetValue(windowName, out var state))
        {
            state = new() { Size = new SizeF(initialWidth, initialHeight) };
            WindowStates.Add(windowName, state);
        }
        state.RestoreWindowState(window);
    }

    /// <summary>
    /// Stores the state of a window.
    /// </summary>
    /// <param name="windowName">The name used to identify the window.</param>
    /// <param name="window">The window to store.</param>
    public void StoreWindowState(string windowName, TWindow window)
    {
        if (!WindowStates.TryGetValue(windowName, out var state))
        {
            state = new();
            WindowStates.Add(windowName, state);
        }
        state.StoreWindowState(window);
    }
}
