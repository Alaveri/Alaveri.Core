namespace Alaveri.Apl.Images;

public struct AplRgbColor(byte red, byte green, byte blue)
{
    public byte Red { get; set; } = red;
    public byte Green { get; set; } = green;
    public byte Blue { get; set; } = blue;

    public AplRgbColor(uint color) : this(
        (byte)((color & 0xFF0000) >> 16),
        (byte)((color & 0x00FF00) >> 8),
        (byte)(color & 0x0000FF))
    {
    }
}
