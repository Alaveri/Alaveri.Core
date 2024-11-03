namespace Alaveri.Avalonia.Drawing;

/// <summary>
/// Represents the pixel format of an image.
/// </summary>
public enum AlaveriPixelFormat
{
    /// <summary>
    /// Transparent 32-bit color.
    /// </summary>
    ARgb8,
    /// <summary>
    /// Opaque 24-bit color.
    /// </summary>
    Rgb8,
    /// <summary>
    /// Transparent 16-bit color.
    /// </summary>
    ARgb6,
    /// <summary>
    /// Opaque 16-bit color.
    /// </summary>
    Rgb6,
    /// <summary>
    /// 256 color grayscale.
    /// </summary>
    Grayscale8,
    /// <summary>
    /// Indexed 256 color.
    /// </summary>
    Indexed8,
    /// <summary>
    /// Indexed 16 color.
    /// </summary>
    Indexed4,
    /// <summary>
    /// VGA 256 color.
    /// </summary>
    Vga,
    /// <summary>
    /// EGA 16 color.
    /// </summary>
    Ega,
    /// <summary>
    /// CGA 4 color.
    /// </summary>
    Cga,
    /// <summary>
    /// Monochrome
    /// </summary>
    Monochrome
}

