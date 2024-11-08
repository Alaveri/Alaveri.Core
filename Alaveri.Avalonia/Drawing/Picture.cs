using Alaveri.Avalonia.Drawing.Extensions;
using Alaveri.Core.Drawing;

namespace Alaveri.Avalonia.Drawing;

/// <summary>
/// Represents an editable image.
/// </summary>
/// <param name="height">The height of the image.</param>
/// <param name="width">The width of the image.</param>
/// <param name="pixelFormat">The pixel format of the image.</param>
public class Picture(int width, int height, PixelFormat pixelFormat) : IPicture
{
    /// <summary>
    /// The width of the image.
    /// </summary>
    public int Width { get; private set; } = width;

    /// <summary>
    /// The height of the image.
    /// </summary>
    public int Height { get; private set; } = height;

    /// <summary>
    /// The pixel format of the image.
    /// </summary>
    public PixelFormat PixelFormat { get; private set; } = pixelFormat;

    /// <summary>
    /// The list of layers in the image.
    /// </summary>
    public IList<IImageLayer> Layers { get; private set; } = [];

    /// <summary>
    /// The palette of the image, or null if the image is not indexed.
    /// </summary>
    public IImagePalette? Palette { get; private set; }

    /// <summary>
    /// The bits per pixel of the image.
    /// </summary>
    public int Bpp => PixelFormat.GetFormatInfo().Bpp;

    /// <summary>
    /// If true, the image is indexed.
    /// </summary>
    public bool IsIndexed => PixelFormat.GetFormatInfo().Indexed;

    /// <summary>
    /// The identifier of the pixel format.
    /// </summary>
    public string PixelFormatIdentifier => PixelFormat.GetFormatInfo().Identifier;
}