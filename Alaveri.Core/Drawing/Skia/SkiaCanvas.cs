using SkiaSharp;
using Alaveri.Core.Drawing;
using Alaveri.Core.Drawing.Skia.Extensions;

namespace Alaveri.Core.Drawing.Skia;

public class SkiaCanvas(SKCanvas canvas) : ICanvas
{
    private SKCanvas Canvas { get; } = canvas;

    public void DrawLine(float x0, float y0, float x1, float y1, IPaint paint)
    {
        Canvas.DrawLine(x0, y0, x1, y1, paint.ToSkPaint());
    }
}