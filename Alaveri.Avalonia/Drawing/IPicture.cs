using Alaveri.Avalonia.Drawing.Extensions;
using Alaveri.Core.Drawing;

namespace Alaveri.Avalonia.Drawing;

/// <summary>
/// Represents an image that can be edited.
/// </summary>
public interface IPicture
{
    /// <summary>
    /// The height of the image.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// If true, the image is indexed.
    /// </summary>
    bool IsIndexed => PixelFormat.GetFormatInfo().Indexed;

    /// <summary>
    /// The pits per pixel for the image.
    /// </summary>
    int Bpp => PixelFormat.GetFormatInfo().Bpp;

    /// <summary>
    /// The layers in the image.
    /// </summary>
    IList<IImageLayer> Layers { get; }

    /// <summary>
    /// The palette of the image, or null if the image is not indexed.
    /// </summary>
    IImagePalette? Palette { get; }

    /// <summary>
    /// The pixel format of the image.
    /// </summary>
    PixelFormat PixelFormat { get; }

    /// <summary>
    /// The width of the image.
    /// </summary>
    int Width { get; }
}