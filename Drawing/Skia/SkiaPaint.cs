using Alaveri.Drawing.Skia.Extensions;
using SkiaSharp;

namespace Alaveri.Drawing.Skia;

public class SkiaPaint(SKPaint paint) : IPaint
{
    public SKPaint SKPaint { get; } = paint;

    public bool AntiAlias => SKPaint.IsAntialias;

    public ARgbColor Color => SKPaint.Color.ToARgbColor();

    public StrokeCap StrokeCap => (StrokeCap)SKPaint.StrokeCap;

    public PaintStyle PaintStyle => (PaintStyle)SKPaint.Style;

    public StrokeJoin StrokeJoin => (StrokeJoin)SKPaint.StrokeJoin;

    public double StrokeWidth => SKPaint.StrokeWidth;

    public IMaskFilter? MaskFilter => SKPaint.MaskFilter?.ToMaskFilter();

    public IShader? Shader => SKPaint.Shader?.ToShader();

    public IGradient CreateLinearGradient(DrawingPoint startPoint, DrawingPoint endPoint, IList<ARgbColor> colors, TileMode tileMode)
    {
        using var shader = SKShader.CreateLinearGradient(startPoint.ToSkPoint(), endPoint.ToSkPoint(), [.. colors.Select(color => color.ToSKColor())],
            tileMode.ToSKShaderTileMode());
        return new SkiaLinearGradient(shader);
    }
}
