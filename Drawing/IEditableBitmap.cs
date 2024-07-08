namespace Alaveri.Drawing;

public interface IEditableBitmap
{
    int Width { get; }

    int Height { get; }

    ICanvas GetCanvas(ISurface surface);

    ISurface GetSurface(AlphaType? alphaType = null);    
}
