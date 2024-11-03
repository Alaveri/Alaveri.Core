using System.Drawing;

namespace Alaveri.Core.Configuration;

/// <summary>
/// Represents the state of a Window, including position, location and WindowState.
/// </summary>
/// <param name="initialWidth">The initial width of the window.</param>
/// <param name="initialHeight">The initial height of the window.</param>
public abstract class StoredWindowState<TWindow>(float initialWidth, float initialHeight) : IStoredWindowState<TWindow> where TWindow : class
{
    /// <summary>
    /// The location of the window.
    /// </summary>
    public PointF Position { get; set; } = new(float.NaN, float.NaN);

    /// <summary>
    /// The size of the window.
    /// </summary>
    public SizeF Size { get; set; } = new SizeF(initialWidth, initialHeight);

    /// <summary>
    /// The location of the window when not maximized.
    /// </summary>
    public PointF RestoredPosition { get; set; }

    /// <summary>
    /// The size of the window.
    /// </summary>
    public SizeF RestoredSize { get; set; }

    /// <summary>
    /// The window's state.
    /// </summary>
    public GenericWindowState State { get; set; }

    /// <summary>
    /// Stores the Window's state.
    /// </summary>
    /// <param name="window">The window to store.</param>
    public abstract void StoreWindowState(TWindow window);

    /// <summary>
    /// Restores the Window state.
    /// </summary>
    /// <param name="window">The window to restore.</param>
    public abstract void RestoreWindowState(TWindow window);


    /// <summary>
    /// Initializes a new instance of the WindowState class.
    /// </summary>
    public StoredWindowState() : this(0, 0)
    {
    }
}
