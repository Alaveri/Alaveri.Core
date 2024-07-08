using Avalonia.Media;
using SkiaSharp;

namespace Alaveri.Avalonia.Drawing;

/// <summary>
/// Represents a color definition supporting multiple color spaces.
/// </summary>
public readonly struct ColorDef
{
    /// <summary>
    /// Color backing field.
    /// </summary>
    private readonly HslColor _hslColor = Colors.White.ToHsl();

    /// <summary>
    /// The hue component of the color.
    /// </summary>
    public double Hue => _hslColor.H;

    /// <summary>
    /// The HSL saturation component of the color. See also <see cref="HsvSaturation"/>.
    /// </summary>
    public double HslSaturation => _hslColor.S;

    /// <summary>
    /// The lightness component of the color.
    /// </summary>
    public double Lightness => _hslColor.L;

    /// <summary>
    /// The alpha component of the color.
    /// </summary>
    public double Alpha => _hslColor.A;

    /// <summary>
    /// The color definition as an HSL color.
    /// </summary>
    public HslColor AsHslColor => _hslColor;

    /// <summary>
    /// The color definition as an HSV color.
    /// </summary>
    public HsvColor AsHsvColor => _hslColor.ToHsv();

    /// <summary>
    /// The color definition as an ARGB color.
    /// </summary>
    public Color AsArgbColor => _hslColor.ToRgb();

    /// <summary>
    /// The color definition as a skia SKColor.
    /// </summary>
    public SKColor AsSkColor
    {
        get
        {
            var color = AsArgbColor;
            return new SKColor(color.R, color.G, color.B, color.A);
        }
    }

    /// <summary>
    /// The color definition with a new red component.
    /// </summary>
    /// <param name="red">The red component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified red component.</returns>
    public ColorDef WithRed(byte red) => new(red, Green, Blue, Alpha);

    /// <summary>
    /// The color definition with a new green component.
    /// </summary>
    /// <param name="green">The green component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified green component.</returns>
    public ColorDef WithGreen(byte green) => new(Red, green, Blue, Alpha);


    /// <summary>
    /// The color definition with a new blue component.
    /// </summary>
    /// <param name="blue">The blue component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified blue component.</returns>
    public ColorDef WithBlue(byte blue) => new(Red, Green, blue, Alpha);

    /// <summary>
    /// The color definition with a new RGB alpha component (0-255).
    /// </summary>
    /// <param name="alpha">The alpha component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified alpha component.</returns>
    public ColorDef WithRgbAlpha(byte alpha) => new(Red, Green, Blue, alpha);

    /// <summary>
    /// The color definition with a new HSL hue component (0-360).
    /// </summary>
    /// <param name="hue">The hue component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified HSL hue component.</returns>
    public ColorDef WithHslHue(double hue) => new(hue, HslSaturation, Lightness, Alpha);

    /// <summary>
    /// The color definition with a new HSL saturation component (0-1).
    /// </summary>
    /// <param name="saturation">The saturation component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified HSL saturation component.</returns>
    public ColorDef WithHslSaturation(double saturation) => new(Hue, saturation, Lightness, Alpha);

    /// <summary>
    /// The color definition with a new HSL lightness component (0-1).
    /// </summary>
    /// <param name="lightness">The lightness component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified HSL lightness component.</returns>
    public ColorDef WithLightness(double lightness) => new(Hue, HslSaturation, lightness, Alpha);

    /// <summary>
    /// The color definition with a new HSV hue component (0-360).
    /// </summary>
    /// <param name="hue"></param>
    /// <returns>A new <see cref="ColorDef"/> with the specified HSV hue component.</returns>
    public ColorDef WithHsvHue(double hue)
    {
        var hsv = AsHsvColor;
        return new HsvColor(hue, hsv.S, hsv.V, Alpha);
    }

    /// <summary>
    /// The color definition with a new HSV saturation component (0-1).
    /// </summary>
    /// <param name="saturation">The saturation component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified HSV saturation component.</returns>
    public ColorDef WithHsvSaturation(double saturation)
    {
        var hsv = AsHsvColor;
        return new HsvColor(hsv.H, saturation, hsv.V, Alpha);
    }

    /// <summary>
    /// The color definition with a new HSV value component (0-1).
    /// </summary>
    /// <param name="value">The value component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified HSV value component.</returns>
    public ColorDef WithHsvValue(double value) => new(Hue, HsvSaturation, value, Alpha);

    /// <summary>
    /// The color definition as HSL with a new alpha component (0-1).
    /// </summary>
    /// <param name="alpha">The alpha component of the color.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified alpha component.</returns>
    public ColorDef WithHslAlpha(double alpha) => new(Hue, HslSaturation, Lightness, alpha);

    /// <summary>
    /// The color definition as HSV with a new alpha component (0-1).
    /// </summary>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public ColorDef WithHsvAlpha(double alpha) => new(Hue, HsvSaturation, Value, alpha);

    /// <summary>
    /// The color definition as a 32-bit ARGB value.
    /// </summary>
    public uint AsArgb => AsArgbColor.ToUInt32();

    /// <summary>
    /// The color definition as a 32-bit RGBA value.
    /// </summary>
    public uint AsRgbA => (byte)(Alpha * 255) | AsArgbColor.ToUInt32() & 0xFFFFFF00;

    /// <summary>
    /// The red component of the color (0-255).
    /// </summary>
    public byte Red => AsArgbColor.R;

    /// <summary>
    /// The green component of the color (0-255).
    /// </summary>
    public byte Green => AsArgbColor.G;

    /// <summary>
    /// The blue component of the color (0-255).
    /// </summary>
    public byte Blue => AsArgbColor.B;

    /// <summary>
    /// The alpha component of the color as ARGB (0-255).
    /// </summary>
    public byte RgbAlpha => AsArgbColor.A;

    /// <summary>
    /// The HSV value component of the color (0-1).
    /// </summary>
    public double Value => _hslColor.ToHsv().V;

    /// <summary>
    /// The HSV saturation component of the color. See also <see cref="HslSaturation"/>.
    /// </summary>
    public double HsvSaturation => _hslColor.ToHsv().S;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorDef"/> struct with the specified Color.
    /// </summary>
    /// <param name="color">The color to use for the definition.</param>
    public ColorDef(Color color)
    {
        _hslColor = color.ToHsl();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorDef"/> struct with the specified HslColor.
    /// </summary>
    /// <param name="color">The color to use for the definition.</param>
    public ColorDef(HslColor color)
    {
        _hslColor = color;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorDef"/> struct with the specified HsvColor.
    /// </summary>
    /// <param name="color">The color to use for the definition.</param>
    public ColorDef(HsvColor color)
    {
        _hslColor = color.ToHsl();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorDef"/> struct with the specified SKColor.
    /// </summary>
    /// <param name="color">The color to use for the definition.</param>
    public ColorDef(SKColor color)
    {
        _hslColor = new Color(color.Alpha, color.Red, color.Green, color.Blue).ToHsl();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorDef"/> struct with the specified red, green, blue, and alpha components (0-255).
    /// </summary>
    /// <param name="red">The red component of the color.</param>
    /// <param name="green">The green component of the color.</param>
    /// <param name="blue">The blue component of the color.</param>
    /// <param name="alpha">The alpha component of the color or 255 if not specified.</param>
    public ColorDef(byte red, byte green, byte blue, byte? alpha = null)
    {
        _hslColor = new Color(alpha ?? 255, red, green, blue).ToHsl();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorDef"/> struct with the specified hue, saturation, lightness, and alpha components (0-1).
    /// </summary>
    /// <param name="hue">The hue component of the color.</param>
    /// <param name="saturation">The saturation component of the color.</param>
    /// <param name="lightness">The lightness component of the color.</param>
    /// <param name="alpha">The alpha component of the color or 1 if not specified.</param>
    public ColorDef(double hue, double saturation, double lightness, double? alpha = null)
    {
        _hslColor = new HslColor(alpha ?? 1, hue, saturation, lightness);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorDef"/> struct with the specified ARGB color value.
    /// </summary>
    /// <param name="argb">The ARGB color value to use for the definition.</param>
    public ColorDef(uint argb)
    {
        var alpha = (byte)((argb & 0xFF000000) >> 24);
        var red = (byte)((argb & 0x00FF0000) >> 16);
        var green = (byte)((argb & 0x0000FF00) >> 8);
        var blue = (byte)(argb & 0x000000FF);
        _hslColor = new Color(alpha, red, green, blue).ToHsl();
    }

    /// <summary>
    /// Creates a new color definition from the specified RGBA color value.
    /// </summary>
    /// <param name="rgba">The RGBA color value to use for the definition.</param>
    /// <returns>A new <see cref="ColorDef"/> with the specified RGBA color value.</returns>
    public static ColorDef FromRgba(uint rgba)
    {
        var red = (byte)((rgba & 0xFF000000) >> 24);
        var green = (byte)((rgba & 0x00FF0000) >> 16);
        var blue = (byte)((rgba & 0x0000FF00) >> 8);
        var alpha = (byte)(rgba & 0x000000FF);
        return new ColorDef(red, green, blue, alpha);
    }

    public static implicit operator Color(ColorDef color) => color.AsArgbColor;

    public static implicit operator HslColor(ColorDef color) => color.AsHslColor;

    public static implicit operator HsvColor(ColorDef color) => color.AsHsvColor;

    public static implicit operator SKColor(ColorDef color) => color.AsSkColor;

    public static implicit operator uint(ColorDef color) => color.AsArgb;

    public static implicit operator ColorDef(Color color) => new(color);

    public static implicit operator ColorDef(HslColor color) => new(color);

    public static implicit operator ColorDef(HsvColor color) => new(color);

    public static implicit operator ColorDef(SKColor color) => new(color);

    public static implicit operator ColorDef(uint color) => new(color);
}
