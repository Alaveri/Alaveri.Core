using System.Drawing;

namespace Alaveri.Drawing;

public readonly struct DrawingRect(double x, double y, double width, double height)
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public double Width { get; } = width;
    public double Height { get; } = height;

    public DrawingRect Translate(double dx, double dy) => new(X + dx, Y + dy, Width, Height);

    public DrawingRect Grow(double dw, double dh) => new(X, Y, Width + dw, Height + dh);

    public double Left => X;

    public double Top => Y;

    public double Right => X + Width;

    public double Bottom => Y + Height;

    public bool IntersectsPoint(DrawingPoint point) => point.X >= X && point.X <= Right && point.Y >= point.Y && point.Y <= Bottom;

    public bool IntersectsRect(DrawingRect rect) => rect.Left <= Right && rect.Right >= Left && rect.Top <= Bottom && rect.Bottom >= Top;

    public DrawingRect(Point location, Size size) : this(location.X, location.Y, size.Width, size.Height)
    {
    }

    public DrawingRect(Rectangle rectangle) : this(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)
    {
    }

    public static implicit operator DrawingRect(Rectangle rectangle) => new(rectangle);
}

public readonly struct DrawingPoint(double x, double y)
{
    public double X { get; } = x;
    public double Y { get; } = y;

    public DrawingPoint Translate(double dx, double dy) => new(X + dx, Y + dy);

    public bool IntersectsRect(DrawingRect rect) => rect.IntersectsPoint(this);

    public static implicit operator DrawingPoint(Point point) => new(point);


    public DrawingPoint(Point point) : this(point.X, point.Y)
    {
    }
}

