using Alaveri.Avalonia.Drawing.Extensions;

namespace Alaveri.Avalonia.Drawing;

/// <summary>
/// Represents an image layer.
/// </summary>
/// <remarks>
/// Initializes a new instance of the ImageLayer class using the specified width, height, pixel format, and palette.
/// </remarks>
/// <param name="width">The width of the layer.</param>
/// <param name="height">The height of the layer.</param>
/// <param name="pixelFormat">The pixel format of the layer.</param>
/// <param name="palette">The palette of the layer.</param>
public class ImageLayer(int width, int height, AlaveriPixelFormat pixelFormat, IImagePalette? palette = null) : IImageLayer
{
    /// <summary>
    /// The width of the layer.
    /// </summary>
    public int Width { get; private set; } = width;

    /// <summary>
    /// The height of the layer.
    /// </summary>
    public int Height { get; private set; } = height;

    /// <summary>
    /// The pixel format of the layer.
    /// </summary>
    public AlaveriPixelFormat PixelFormat { get; private set; } = pixelFormat;

    /// <summary>
    /// The palette of the layer.
    /// </summary>
    public IImagePalette? Palette { get; set; } = palette;

    /// <summary>
    /// The bits per pixel of the image.
    /// </summary>
    public int Bpp => PixelFormat.GetFormatInfo().Bpp;

    /// <summary>
    /// If true, the image is indexed.
    /// </summary>
    public bool IsIndexed => PixelFormat.GetFormatInfo().Indexed;

    /// <summary>
    /// The opacity of the layer.
    /// </summary>
    public byte Alpha { get; set; } = 255;

    /// <summary>
    /// The filter to apply to the layer.
    /// </summary>
    public ImageFilter Filter { get; set; } = ImageFilter.Normal;

    /// <summary>
    /// If true, the layer is visible.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public static ImageLayer CreateFrom(IPicture image)
    {
        var layer = new ImageLayer(image.Width, image.Height, image.PixelFormat, image.Palette);
        return layer;
    }
}
