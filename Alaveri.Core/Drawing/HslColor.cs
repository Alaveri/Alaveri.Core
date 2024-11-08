using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Drawing;

public class HslColor(double hue, double saturation, double lightness, double alpha)
{
    public double Alpha { get; } = alpha;
    public double Hue { get; } = hue;
    public double Saturation { get; } = saturation;
    public double Lightness { get; } = lightness;

    public static HslColor FromSKColor(SKColor color)
    {
        return FromARgbColor(new ARgbColor(color.Red, color.Green, color.Blue, color.Alpha));
    }

    public static HslColor FromHsvColor(HsvColor color)
    {
        double chroma = color.Value * color.Saturation;
        double lightness = color.Value - chroma / 2;
        double hue = color.Hue;
        double saturation = color.Value == 0 ? 0 : chroma / color.Value;
        return new HslColor(hue, saturation, lightness, color.Alpha);
    }

    public static HslColor FromARgbColor(ARgbColor color)
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
        double lightness = (max + min) / 2;
        double saturation = chroma == 0 ? 0 : chroma / (1 - Math.Abs(2 * lightness - 1));
        return new HslColor(hue, saturation, lightness, color.Alpha / 255.0);
    }
}
