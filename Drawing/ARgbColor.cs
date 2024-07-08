namespace Alaveri.Drawing;

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
        set => Color = (Color & 0x00FFFFFF) | ((uint)value << 24);
    }

    /// <summary>
    /// The red component of the color.
    /// </summary>
    public byte Red
    {
        readonly get => (byte)(Color >> 16);
        set => Color = (Color & 0xFF00FFFF) | ((uint)value << 16);
    }

    /// <summary>
    /// The green component of the color.
    /// </summary>
    public byte Green
    {
        readonly get => (byte)(Color >> 8);
        set => Color = (Color & 0xFFFF00FF) | ((uint)value << 8);
    }

    /// <summary>
    /// The blue component of the color.
    /// </summary>
    public byte Blue
    {
        readonly get => (byte)Color;
        set => Color = (Color & 0xFFFFFF00) | value;
    }

    /// <summary>
    /// Initializes a new instance of the ARgbColor struct with the specified color components.
    /// </summary>
    /// <param name="red">The red component of the color.</param>
    /// <param name="green">The green component of the color.</param>
    /// <param name="blue">The blue component of the color.</param>
    /// <param name="alpha">The alpha component of the color.</param>
    public ARgbColor(byte red, byte green, byte blue, byte alpha = 255) : this((uint)((alpha << 24) | (red << 16) | (green << 8) | blue))
    {
    }
}
