using Avalonia.Media;
using Avalonia;
using Alaveri.Core.Drawing;
using HslColor = Avalonia.Media.HslColor;
using HsvColor = Avalonia.Media.HsvColor;

namespace Alaveri.Avalonia.Drawing.Extensions;

public static class DrawingExtensions
{
    public static Rect AtOrigin(this Rect rect)
    {
        return rect.Translate(new Vector(-rect.X, -rect.Y));
    }
    public static Rect Grow(this Rect rect, float dw, float dh)
    {
        return new Rect(rect.X, rect.Y, rect.Width + dw, rect.Height + dh);
    }

    public static bool IntersectsPoint(this Rect rect, double x, double y)
    {
        return x >= rect.Left && x <= rect.Right && y >= rect.Top && y <= rect.Bottom;
    }

    public static bool IsEmpty(this Rect rect)
    {
        return rect.Width == 0 || rect.Height == 0;
    }

    public static bool IntersectsPoint(this Rect rect, Point point)
    {
        return rect.IntersectsPoint(point.X, point.Y);
    }

    public static IPen WithBrush(this IPen pen, IBrush brush)
    {
        return new Pen(brush, pen.Thickness, pen.DashStyle, pen.LineCap, pen.LineJoin, pen.MiterLimit);
    }

    public static IPen WithColor(this IPen pen, ColorDef color)
    {
        return pen.WithBrush(new SolidColorBrush(color));
    }

    public static IPen WithThickness(this IPen pen, double thickness)
    {
        return new Pen(pen.Brush, thickness, pen.DashStyle, pen.LineCap, pen.LineJoin, pen.MiterLimit);
    }

    public static IPen WithDashStyle(this IPen pen, DashStyle dashStyle)
    {
        return new Pen(pen.Brush, pen.Thickness, dashStyle, pen.LineCap, pen.LineJoin, pen.MiterLimit);
    }

    public static IPen WithLineCap(this IPen pen, PenLineCap lineCap)
    {
        return new Pen(pen.Brush, pen.Thickness, pen.DashStyle, lineCap, pen.LineJoin, pen.MiterLimit);
    }

    public static IPen WithLineJoin(this IPen pen, PenLineJoin lineJoin)
    {
        return new Pen(pen.Brush, pen.Thickness, pen.DashStyle, pen.LineCap, lineJoin, pen.MiterLimit);
    }

    public static IPen WithMiterLimit(this IPen pen, double miterLimit)
    {
        return new Pen(pen.Brush, pen.Thickness, pen.DashStyle, pen.LineCap, pen.LineJoin, miterLimit);
    }
}

public static class PixelFormatExtensions
{
    private static AvailablePixelFormats AvailablePixelFormats { get; } = new AvailablePixelFormats();

    public static PixelFormatInfo GetFormatInfo(this PixelFormat pixelFormat) => AvailablePixelFormats[pixelFormat];
}

public static class ColorDefExtensions
{
    public static Color ToAvaloniaColor(this ColorDef color)
    {
        return Color.FromArgb(color.RgbAlpha, color.Red, color.Green, color.Blue);
    }

    public static HslColor ToAvaloniaHsl(this ColorDef color)
    {
        return new HslColor(color.Hue, color.HslSaturation, color.Lightness, color.Alpha);
    }

    public static HsvColor ToAvaloniaHsv(this ColorDef color)
    {
        return new HsvColor(color.Hue, color.HsvSaturation, color.Value, color.Alpha);
    }

    public static ColorDef ToColorDef(this Color color)
    {
        return new ColorDef(color.R, color.G, color.B, color.A);
    }
}