using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Drawing;

public class HsvColor(double hue, double saturation, double value, double alpha)
{
    public double Alpha { get; } = alpha;

    public double Hue { get; } = hue;

    public double Saturation { get; } = saturation;

    public double Value { get; } = value;

    public static HsvColor FromSKColor(SKColor color)
    {
        return FromARgbColor(new ARgbColor(color.Red, color.Green, color.Blue, color.Alpha));
    }

    public static HsvColor FromHslColor(HslColor color)
    {
        double chroma = (1 - Math.Abs(2 * color.Lightness - 1)) * color.Saturation;
        double value = chroma + color.Lightness;
        double saturation = value == 0 ? 0 : chroma / value;
        return new HsvColor(color.Hue, saturation, value, color.Alpha);
    }
    
    public static HsvColor FromARgbColor(ARgbColor color)
    {
        double red = color.Red / 255.0;
        double green = color.Green / 255.0;
        double blue = color.Blue / 255.0;
        double max = Math.Max(red, Math.Max(green, blue));
        double min = Math.Min(red, Math.Min(green, blue));
        double chroma = max - min;
        double hue = 0;
        if (chroma != 0)
        {
            if (max == red)
                hue = 60 * ((green - blue) / chroma % 6);
            else if (max == green)
                hue = 60 * ((blue - red) / chroma + 2);
            else if (max == blue)
                hue = 60 * ((red - green) / chroma + 4);
        }
        double value = max;
        double saturation = chroma == 0 ? 0 : chroma / value;
        return new HsvColor(hue, saturation, value, color.Alpha / 255.0);
    }
}
