using Avalonia;
using Avalonia.Controls;

namespace Alaveri.Avalonia;

public struct AvaloniaSize(double width, double height)
{
    public double Width { get; set; } = width;
    public double Height { get; set; } = height;
    public AvaloniaSize() : this(0, 0)
    {
    }
}

/// <summary>
/// Represents the state of a Window, including position, location and WindowState.
/// </summary>
/// <param name="initialWidth">The initial width of the window.</param>
/// <param name="initialHeight">The initial height of the window.</param>
public class StoredWindowState(int initialWidth, int initialHeight)
{
    /// <summary>
    /// The location of the window.
    /// </summary>
    public PixelPoint Position { get; set; } = new PixelPoint(int.MaxValue, int.MaxValue);

    /// <summary>
    /// The size of the window.
    /// </summary>
    public AvaloniaSize Size { get; set; } = new AvaloniaSize(initialWidth, initialHeight);

    /// <summary>
    /// The location of the window when not maximized.
    /// </summary>
    public PixelPoint RestoredPosition { get; set; }

    /// <summary>
    /// The size of the window.
    /// </summary>
    public AvaloniaSize RestoredSize { get; set; }

    /// <summary>
    /// The window's state.
    /// </summary>
    public WindowState WindowState { get; set; }

    /// <summary>
    /// Stores the Window's state.
    /// </summary>
    /// <param name="window">The window to store.</param>
    public void StoreWindowState(Window window)
    {
        if (window.WindowState != WindowState.Minimized)
        {
            Position = window.Position;
            Size = new AvaloniaSize(window.Width, window.Height);
            switch (window.WindowState)
            {
                case WindowState.Normal:
                case WindowState.Minimized:
                    RestoredPosition = Position;
                    RestoredSize = Size;
                    WindowState = WindowState.Normal;
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Maximized;
                    break;
            }
        }
    }

    /// <summary>
    /// Restores the Window state.
    /// </summary>
    /// <param name="window">The window to restore.</param>
    public void RestoreWindowState(Window window)
    {
        if (Position.X != int.MaxValue && Position.Y != int.MaxValue)
            window.Position = Position;
        
        window.Width = Size.Width;
        window.Height = Size.Height;
        if (WindowState == WindowState.Maximized)
            window.WindowState = WindowState.Maximized;
    }

    /// <summary>
    /// Initializes a new instance of the WindowState class.
    /// </summary>
    public StoredWindowState() : this(0, 0)
    {
    }
}
