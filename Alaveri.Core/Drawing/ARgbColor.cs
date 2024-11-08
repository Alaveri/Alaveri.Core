using SkiaSharp;

namespace Alaveri.Core.Drawing;

/// <summary>
/// Represents a 32-bit color with alpha, red, green, and blue components.
/// </summary>
/// <param name="color"></param>
public struct ARgbColor(uint color)
{
    /// <summary>
    /// The color value.
    /// </summary>
    public uint Color { get; set; } = color;

    /// <summary>
    /// The alpha component of the color.
    /// </summary>
    public byte Alpha
    {
        readonly get => (byte)(Color >> 24);
        set => Color = Color & 0x00FFFFFF | (uint)value << 24;
    }

    /// <summary>
    /// The red component of the color.
    /// </summary>
    public byte Red
    {
        readonly get => (byte)(Color >> 16);
        set => Color = Color & 0xFF00FFFF | (uint)value << 16;
    }

    /// <summary>
    /// The green component of the color.
    /// </summary>
    public byte Green
    {
        readonly get => (byte)(Color >> 8);
        set => Color = Color & 0xFFFF00FF | (uint)value << 8;
    }

    /// <summary>
    /// The blue component of the color.
    /// </summary>
    public byte Blue
    {
        readonly get => (byte)Color;
        set => Color = Color & 0xFFFFFF00 | value;
    }

    public static ARgbColor FromSKColor(SKColor color)
    {
        return new ARgbColor(color.Red, color.Green, color.Blue, color.Alpha);
    }

    public static ARgbColor FromHslColor(HslColor color)
    {
        double chroma = (1 - Math.Abs(2 * color.Lightness - 1)) * color.Saturation;
        double component = chroma * (1 - Math.Abs(color.Hue / 60 % 2 - 1));
        double lightness = color.Lightness - chroma / 2;
        double red, green, blue;
        if (color.Hue < 60)
        {
            red = chroma;
            green = component;
            blue = 0;
        }
        else if (color.Hue < 120)
        {
            red = component;
            green = chroma;
            blue = 0;
        }
        else if (color.Hue < 180)
        {
            red = 0;
            green = chroma;
            blue = component;
        }
        else if (color.Hue < 240)
        {
            red = 0;
            green = component;
            blue = chroma;
        }
        else if (color.Hue < 300)
        {
            red = component;
            green = 0;
            blue = chroma;
        }
        else
        {
            red = chroma;
            green = 0;
            blue = component;
        }
        return new ARgbColor(
            (byte)((red + lightness) * 255),
            (byte)((green + lightness) * 255),
            (byte)((blue + lightness) * 255),
            (byte)(color.Alpha * 255));
    }

    public static ARgbColor FromHsvColor(HsvColor color)
    {
        double chroma = color.Value * color.Saturation;
        double component = chroma * (1 - Math.Abs(color.Hue / 60 % 2 - 1));
        double lightness = color.Value - chroma;
        double red, green, blue;
        if (color.Hue < 60)
        {
            red = chroma;
            green = component;
            blue = 0;
        }
        else if (color.Hue < 120)
        {
            red = component;
            green = chroma;
            blue = 0;
        }
        else if (color.Hue < 180)
        {
            red = 0;
            green = chroma;
            blue = component;
        }
        else if (color.Hue < 240)
        {
            red = 0;
            green = component;
            blue = chroma;
        }
        else if (color.Hue < 300)
        {
            red = component;
            green = 0;
            blue = chroma;
        }
        else
        {
            red = chroma;
            green = 0;
            blue = component;
        }
        return new ARgbColor(
            (byte)((red + lightness) * 255),
            (byte)((green + lightness) * 255),
            (byte)((blue + lightness) * 255),
            (byte)(color.Alpha * 255));
    }

    /// <summary>
    /// Initializes a new instance of the ARgbColor struct with the specified color components.
    /// </summary>
    /// <param name="red">The red component of the color.</param>
    /// <param name="green">The green component of the color.</param>
    /// <param name="blue">The blue component of the color.</param>
    /// <param name="alpha">The alpha component of the color.</param>
    public ARgbColor(byte red, byte green, byte blue, byte alpha = 255) : this((uint)(alpha << 24 | red << 16 | green << 8 | blue))
    {
    }
}
