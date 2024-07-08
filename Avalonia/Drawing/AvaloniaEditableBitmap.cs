using Alaveri.Drawing;
using Alaveri.Drawing.Skia;
using Alaveri.Drawing.Skia.Extensions;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Skia;
using SkiaSharp;
using Platform = Avalonia.Platform;

namespace Alaveri.Avalonia.Drawing;

public class AvaloniaEditableBitmap(PixelSize size, Vector dpi, Platform.PixelFormat pixelFormat, Platform.AlphaFormat alphaFormat)
    : WriteableBitmap(size, dpi, pixelFormat, alphaFormat), IEditableBitmap
{
    public int Width { get; }

    public int Height { get; }

    public ISurface GetSurface(AlphaType? alphaType = null)
    {
        alphaType ??= AlphaType.Premultiplied;
        using var frameBuffer = Lock();
        var info = new SKImageInfo(Width, Height, frameBuffer.Format.ToSkColorType(), alphaType.Value.ToSKAlphaType());
        var surface = SKSurface.Create(info, frameBuffer.Address, frameBuffer.RowBytes);
        return new SkiaSurface(SKSurface.Create(info, frameBuffer.Address, frameBuffer.RowBytes));
    }

    public ICanvas GetCanvas(ISurface? surface = null)
    {
        surface ??= GetSurface();
        return surface.Canvas;
    }
}