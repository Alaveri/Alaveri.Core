using SkiaSharp;

namespace Alaveri.Avalonia.Drawing;

public struct PixelFormatInfo(int bpp, bool indexed, SKColorType skColorType, SKAlphaType skAlphaType, string identifier = "")
{
    public int Bpp { get; set; } = bpp;

    public bool Indexed { get; set; } = indexed;

    public SKColorType SKColorType { get; set; } = skColorType;

    public SKAlphaType SKAlphaType { get; set; } = skAlphaType;

    public string Identifier { get; set; } = identifier;
}

public sealed class AvailablePixelFormats
{
    public PixelFormatInfo this[AlaveriPixelFormat index] => Formats[(int)index];

    private static readonly PixelFormatInfo[] Formats =
    [
        new PixelFormatInfo(8, false, SKColorType.Rgba8888, SKAlphaType.Premul, "ARgb8"),
        new PixelFormatInfo(8, false, SKColorType.Rgb888x, SKAlphaType.Opaque, "Rgb8"),
        new PixelFormatInfo(6, false, SKColorType.Rgba8888, SKAlphaType.Premul, "ARgb6"),
        new PixelFormatInfo(6, false, SKColorType.Rgb888x, SKAlphaType.Opaque, "Rgb6"),
        new PixelFormatInfo(8, false, SKColorType.Gray8, SKAlphaType.Opaque, "Grayscale8"),
        new PixelFormatInfo(8, true, SKColorType.Rgb888x, SKAlphaType.Opaque, "Indexed8"),
        new PixelFormatInfo(4, true, SKColorType.Rgb888x, SKAlphaType.Opaque, "Indexed4"),
        new PixelFormatInfo(8, true, SKColorType.Rgb888x, SKAlphaType.Opaque, "Vga"),
        new PixelFormatInfo(4, true, SKColorType.Rgb888x, SKAlphaType.Opaque, "Ega"),
        new PixelFormatInfo(2, true, SKColorType.Rgb565, SKAlphaType.Opaque, "Cga"),
        new PixelFormatInfo(1, false, SKColorType.Gray8, SKAlphaType.Opaque, "Monochrome")
    ];
}
