using Alaveri.Avalonia.Drawing.Extensions;
using Alaveri.Core.Drawing;

namespace Alaveri.Avalonia.Drawing;

/// <summary>
/// Represents the possible CGA palettes.
/// </summary>
public enum CgaPaletteType
{
    /// <summary>
    /// Low intensity palette 0.
    /// </summary>
    Palette0LowIntensity,

    /// <summary>
    /// High intensity palette 0.
    /// </summary>
    Palette0HighIntensity,

    /// <summary>
    /// Low intensity palette 1.
    /// </summary>
    Palette1LowIntensity,

    /// <summary>
    /// High intensity palette 1.
    /// </summary>
    Palette1HighIntensity,

    /// <summary>
    /// Low intensity palette 2.
    /// </summary>
    Palette2LowIntensity,

    /// <summary>
    /// High intensity palette 2.
    /// </summary>
    Palette2HighIntensity
}

/// <summary>
/// Represents an image palette.
/// </summary>
public sealed class ImagePalette : IImagePalette
{
    /// <summary>
    /// Initializes a new instance of the ImagePalette class with the specified pixel format.
    /// </summary>
    /// <param name="pixelFormat">The pixel format of the palette.</param>
    /// <exception cref="ArgumentException">Thrown if PixelFormat is not an indexed format.</exception>
    public ImagePalette(PixelFormat pixelFormat)
    {
        PixelFormat = pixelFormat;
        if (!IsIndexed)
            throw new ArgumentException($"{nameof(PixelFormat)} must be indexed.");
        Colors = new List<ARgbColor>(ColorCount).AsReadOnly();
    }

    /// <summary>
    /// The pixel format of the palette.
    /// </summary>
    public PixelFormat PixelFormat { get; }

    /// <summary>
    /// The colors in the palette.
    /// </summary>
    public IReadOnlyList<ARgbColor> Colors { get; private set; }

    /// <summary>
    /// The bits per pixel of the image.
    /// </summary>
    public int Bpp => PixelFormat.GetFormatInfo().Bpp;


    /// <summary>
    /// If true, the image is indexed.
    /// </summary>
    public bool IsIndexed => PixelFormat.GetFormatInfo().Indexed;

    /// <summary>
    /// The number of colors in the palette.
    /// </summary>
    public int ColorCount => 1 << Bpp;

    /// <summary>
    /// The indexer for the palette representing the color at the specified index.
    /// </summary>
    /// <param name="index">The index of the color to retrieve.</param>
    /// <returns>The <see cref="ARgbColor"/> value representing the color at the specified index.</returns>
    public ARgbColor this[int index] => Colors[index];

    /// <summary>
    /// Creates the standard palette for APL VGA 256-color images.
    /// </summary>
    /// <returns>A new instance of the ImagePalette class representing the standard APL VGA palette.</returns>
    public static IImagePalette CreateStandardAplVga()
    {
        var result = new ImagePalette(PixelFormat.Vga);
        return result;
    }

    /// <summary>
    /// Creates the standard palette for EGA 16-color images.
    /// </summary>
    /// <returns>A new instance of the ImagePalette class representing the standard EGA palette.</returns>
    public static IImagePalette CreateStandardEga()
    {
        var result = new ImagePalette(PixelFormat.Ega)
        {
            Colors =
            [
                new(0xFF000000),
                new(0xFF0000AA),
                new(0xFF00AA00),
                new(0xFF00AAAA),
                new(0xFFAA0000),
                new(0xFFAA00AA),
                new(0xFFAA5500),
                new(0xFFAAAAAA),
                new(0xFF555555),
                new(0xFF5555FF),
                new(0xFF55FF55),
                new(0xFF55FFFF),
                new(0xFFFF5555),
                new(0xFFFF55FF),
                new(0xFFFFFF55),
                new(0xFFFFFFFF)
            ]
        };
        return result;
    }

    /// <summary>
    /// Creates the standard palette for CGA 4-color images.
    /// </summary>
    /// <param name="type">The type of CGA palette to create.</param>
    /// <returns>A new instance of the ImagePalette class representing the standard CGA palette.</returns>
    public static IImagePalette CreateStandardCga(CgaPaletteType type)
    {
        var result = new ImagePalette(PixelFormat.Cga);
        switch (type)
        {
            case CgaPaletteType.Palette0LowIntensity:
                result.Colors =
                [
                    new(0xFF000000),
                    new(0xFF00AA00),
                    new(0xFFAA0000),
                    new(0xFFAA5500)
                ];
                break;
            case CgaPaletteType.Palette0HighIntensity:
                result.Colors =
                [
                    new(0xFF000000),
                    new(0xFF55FF55),
                    new(0xFFFF5555),
                    new(0xFFFFFF55)
                ];
                break;
            case CgaPaletteType.Palette1LowIntensity:
                result.Colors =
                [
                    new(0xFF000000),
                    new(0xFF00AAAA),
                    new(0xFFAA00AA),
                    new(0xFFAAAAAA)
                ];
                break;
            case CgaPaletteType.Palette1HighIntensity:
                result.Colors =
                [
                    new(0xFF000000),
                    new(0xFF55FFFF),
                    new(0xFFFF55FF),
                    new(0xFFFFFFFF)
                ];
                break;
            case CgaPaletteType.Palette2LowIntensity:
                result.Colors =
                [
                    new(0xFF000000),
                    new(0xFF00AAAA),
                    new(0xFFAA0000),
                    new(0xFFAAAAAA)
                ];
                break;
            case CgaPaletteType.Palette2HighIntensity:
                result.Colors =
                [
                    new(0xFF000000),
                    new(0xFF55FFFF),
                    new(0xFFFF5555),
                    new(0xFFFFFFFF)
                ];
                break;
        }
        return result;
    }
}