using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Skia;
using SkiaSharp;
using Platform = Avalonia.Platform;

namespace Alaveri.Avalonia.Drawing;

public class EditorBitmap(PixelSize size, Vector dpi, Platform.PixelFormat pixelFormat, AlphaFormat alphaFormat) : WriteableBitmap(size, dpi, pixelFormat, alphaFormat)
{
    private readonly PixelSize _size = size;

    public int Width =>_size.Width;

    public int Height => _size.Height;

    public SKCanvas GetCanvas(SKSurface? surface = null)
    {
        surface ??= GetSurface();
        return surface.Canvas;
    }

    public SKSurface GetSurface(SKAlphaType? alphaType = null, SKColorSpace? colorSpace = null)
    {
        alphaType ??= AlphaFormat?.ToSkAlphaType() ?? SKAlphaType.Premul;
        using var frameBuffer = Lock();
        var info = new SKImageInfo(Width, Height, frameBuffer.Format.ToSkColorType(), alphaType.Value, colorSpace);
        return SKSurface.Create(info, frameBuffer.Address, frameBuffer.RowBytes);
    }

    public EditorBitmap(int width, int height) : this(new PixelSize(width, height), default, Platform.PixelFormat.Rgba8888,
        Platform.AlphaFormat.Premul)
    {
    }
}
