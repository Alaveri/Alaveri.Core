using Alaveri.Core.Drawing;

namespace Alaveri.Avalonia.Drawing;

/// <summary>
/// Represents an image palette.
/// </summary>
public interface IImagePalette
{
    /// <summary>
    /// Color indexer for the palette.
    /// </summary>
    /// <param name="index">The index of the color to retrieve.</param>
    /// <returns>An <see cref="ARgbColor"/> struct representing the color at the specified index.</returns>
    ARgbColor this[int index] { get; }

    /// <summary>
    /// The bits per pixel of the palette.
    /// </summary>
    int Bpp { get; }

    /// <summary>
    /// The colors in the palette.
    /// </summary>
    IReadOnlyList<ARgbColor> Colors { get; }

    /// <summary>
    /// The pixel format of the palette.
    /// </summary>
    AlaveriPixelFormat PixelFormat { get; }

    /// <summary>
    /// The number of colors in the palette.
    /// </summary>
    int ColorCount { get; }
}