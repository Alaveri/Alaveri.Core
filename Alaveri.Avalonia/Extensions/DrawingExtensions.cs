using Avalonia;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Avalonia.Extensions;

public static class DrawingExtensions
{
    public static PixelPoint ToPixelPoint(this PointF point)
    {
        return new PixelPoint((int)point.X, (int)point.Y);
    }

    public static PixelPoint ToPixelPoint(this System.Drawing.Point point)
    {
        return new PixelPoint(point.X, point.Y);
    }

    public static System.Drawing.Point ToPoint(this PixelPoint point)
    {
        return new System.Drawing.Point(point.X, point.Y);
    }
}
