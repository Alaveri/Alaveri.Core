namespace Alaveri.Core.Configuration;

/// <summary>
/// Represents a configuration containing the state of various windows.
/// </summary>
/// <typeparam name="TWindow">The type of window.</typeparam>
/// <typeparam name="TStoredWindowState">The type of StoredWindowState to use.</typeparam>
public interface IWindowConfiguration<TWindow, TStoredWindowState> : IConfiguration
    where TWindow : class
    where TStoredWindowState : StoredWindowState<TWindow>, new()
{
    /// <summary>
    /// Gets the state of a window.
    /// </summary>
    /// <param name="index">The name of the window.</param>
    /// <returns>An object of type <see cref="TWindow"/> containing the window's state.</returns>
    StoredWindowState<TWindow> this[string index] { get; }

    /// <summary>
    /// Gets the state of all windows.
    /// </summary>
    IDictionary<string, TStoredWindowState> WindowStates { get; }

    /// <summary>
    /// Restores the state of a window.
    /// </summary>
    /// <param name="windowName">The name used to identify the window.</param>
    /// <param name="window">The window to restore.</param>
    void RestoreWindowState(string windowName, TWindow window, float initialWidth = 1024, float initialHeight = 768);

    /// <summary>
    /// Stores the state of a window.
    /// </summary>
    /// <param name="windowName">The name used to identify the window.</param>
    /// <param name="window">The window to store.</param>
    void StoreWindowState(string windowName, TWindow window);
}