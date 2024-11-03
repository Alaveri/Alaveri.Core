using SkiaSharp;

namespace Alaveri.Core.Drawing.Skia;

public class SkiaSurface(SKSurface surface) : ISurface
{
    private SKSurface Surface { get; } = surface;

    public ICanvas Canvas => new SkiaCanvas(Surface.Canvas);

}
