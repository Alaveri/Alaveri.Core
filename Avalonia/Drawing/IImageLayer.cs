using Alaveri.Avalonia.Drawing.Extensions;

namespace Alaveri.Avalonia.Drawing;

/// <summary>
/// Represents an image layer.
/// </summary>
public interface IImageLayer
{
    /// <summary>
    /// The bitmap data.
    /// </summary>
    EditorBitmap Bitmap { get; }

    /// <summary>
    /// The height of the layer.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// If true, the image is indexed.
    /// </summary>
    bool IsIndexed => PixelFormat.GetFormatInfo().Indexed;

    /// <summary>
    /// The bits per pixel of the image.
    /// </summary>
    int Bpp => PixelFormat.GetFormatInfo().Bpp;

    /// <summary>
    /// The palette of the layer.
    /// </summary>
    IImagePalette? Palette { get; set; }

    /// <summary>
    /// The pixel format of the layer.
    /// </summary>
    AlaveriPixelFormat PixelFormat { get; }

    /// <summary>
    /// The width of the layer.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// If true, the layer is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// The filter applied to the layer.
    /// </summary>
    ImageFilter Filter { get; set; }

    /// <summary>
    /// The opacity of the layer. 
    /// </summary>
    byte Alpha { get; set; }
}