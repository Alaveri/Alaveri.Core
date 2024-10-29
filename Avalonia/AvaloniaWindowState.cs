using Alaveri.Avalonia.Extensions;
using Alaveri.Configuration;
using Alaveri.Extensions;
using Avalonia;
using Avalonia.Controls;
using System.Drawing;

namespace Alaveri.Avalonia;

public class AvaloniaStoredWindowState(float initialWidth, float initialHeight) : StoredWindowState<Window>(initialWidth, initialHeight)
{
    public static WindowState GetAvaloniaWindowState(GenericWindowState state)
    {
        return state switch
        {
            GenericWindowState.Normal => WindowState.Normal,
            GenericWindowState.Minimized => WindowState.Minimized,
            GenericWindowState.Maximized => WindowState.Maximized,
            _ => throw new NotImplementedException(),
        };
    }

    public override void RestoreWindowState(Window window)
    {
        if (!float.IsNaN(Position.X) && !float.IsNaN(Position.Y))
            window.Position = Position.ToPixelPoint();

        window.Width = Size.Width;
        window.Height = Size.Height;
        if (State == GenericWindowState.Maximized)
            window.WindowState = WindowState.Maximized;
    }

    public override void StoreWindowState(Window window)
    {
        if (window.WindowState == WindowState.Minimized)
            return;
        Position = window.Position.ToPoint();
        Size = new SizeF(window.Width.AsSingle(), window.Height.AsSingle());
        switch (window.WindowState)
        {
            case WindowState.Normal:
            case WindowState.Minimized:
                RestoredPosition = Position;
                RestoredSize = Size;
                State = GenericWindowState.Normal;
                break;
            case WindowState.Maximized:
                State = GenericWindowState.Maximized;
                break;
        }
    }

    public AvaloniaStoredWindowState() : this(0, 0)
    {
    }
}
