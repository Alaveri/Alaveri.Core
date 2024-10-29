using Alaveri.Extensions;
using SkiaSharp;

namespace Alaveri.Drawing.Skia.Extensions;

public static class SkiaExtensions
{
    public static SKAlphaType ToSKAlphaType(this AlphaType alphaType)
    {
        return alphaType switch
        {
            AlphaType.Opaque => SKAlphaType.Opaque,
            AlphaType.Premultiplied => SKAlphaType.Premul,
            AlphaType.Unpremultiplied => SKAlphaType.Unpremul,
            _ => throw new ArgumentOutOfRangeException(nameof(alphaType))
        };
    }

    public static SKColor ToSKColor(this ARgbColor color)
    {
        return new SKColor(color.Red, color.Green, color.Blue, color.Alpha);
    }

    public static SKShaderTileMode ToSKShaderTileMode(this TileMode tileMode)
    {
        return tileMode switch
        {
            TileMode.Clamp => SKShaderTileMode.Clamp,
            TileMode.Repeat => SKShaderTileMode.Repeat,
            TileMode.Mirror => SKShaderTileMode.Mirror,
            _ => throw new ArgumentOutOfRangeException(nameof(tileMode))
        };
    }

    public static SKPaintStyle ToSkPaintStyle(this PaintStyle paintStyle)
    {
        return (SKPaintStyle)paintStyle;
    }

    public static ARgbColor ToARgbColor(this SKColor color)
    {
        return new ARgbColor(color.Red, color.Green, color.Blue, color.Alpha);
    }

    public static IShader ToShader(this SKShader? shader)
    {
        return new SkiaShader(shader);
    }

    public static SKPoint ToSkPoint(this DrawingPoint point)
    {
        return new SKPoint(point.X.AsSingle(), point.Y.AsSingle());
    }

    public static IMaskFilter ToMaskFilter(this SKMaskFilter maskFilter)
    {
        return new SkiaMaskFilter(maskFilter);
    }

    public static SKMaskFilter? ToSkMaskFilter(this IMaskFilter? maskFilter)
    {
        return maskFilter?.MaskFilter as SKMaskFilter;
    }

    public static SKPaint ToSkPaint(this IPaint paint)
    {
        var skPaint = new SKPaint
        {
            IsAntialias = paint.AntiAlias,
            Color = paint.Color.ToSKColor(),
            Style = paint.PaintStyle.ToSkPaintStyle(),
            StrokeWidth = paint.StrokeWidth.AsSingle(),
            StrokeCap = (SKStrokeCap)paint.StrokeCap,
            StrokeJoin = (SKStrokeJoin)paint.StrokeJoin,
            MaskFilter = paint.MaskFilter?.ToSkMaskFilter(),
            Shader = paint.Shader?.Shader as SKShader
        };
        return skPaint;
    }

}
