using Avalonia;
using Avalonia.Media.Imaging;
using SkiaSharp;
using Platform = Avalonia.Platform;
using Alaveri.Core.Drawing;
using Alaveri.Core.Drawing.Skia.Extensions;
using Alaveri.Core.Drawing.Skia;
using Alaveri.Avalonia.Drawing.Extensions;

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
        var info = new SKImageInfo(Width, Height, frameBuffer.Format.ToSKColorType(), alphaType.Value.ToSKAlphaType());
        var surface = SKSurface.Create(info, frameBuffer.Address, frameBuffer.RowBytes);
        return new SkiaSurface(SKSurface.Create(info, frameBuffer.Address, frameBuffer.RowBytes));
    }

    public ICanvas GetCanvas(ISurface? surface = null)
    {
        surface ??= GetSurface();
        return surface.Canvas;
    }
}