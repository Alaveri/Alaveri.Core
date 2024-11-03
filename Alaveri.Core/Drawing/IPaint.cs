namespace Alaveri.Core.Drawing;

public enum StrokeCap
{
    Butt,
    Round,
    Square
}

public enum PaintStyle
{
    Fill,
    Stroke,
    FillAndStroke
}
public enum StrokeJoin
{
    Miter,
    Round,
    Bevel
}

public enum BlurStyle
{
    Normal,
    Solid,
    Outer,
    Inner
}

public interface IPaint
{
    IGradient CreateLinearGradient(DrawingPoint startPoint, DrawingPoint endPoint, IList<ARgbColor> colors, TileMode tileMode);

    bool AntiAlias { get; }

    ARgbColor Color { get; }

    StrokeCap StrokeCap { get; }

    PaintStyle PaintStyle { get; }

    StrokeJoin StrokeJoin { get; }

    double StrokeWidth { get; }

    IMaskFilter? MaskFilter { get; }

    IShader? Shader { get; }
}
