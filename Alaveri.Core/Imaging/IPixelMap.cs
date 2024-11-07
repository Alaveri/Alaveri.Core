


namespace Alaveri.Core.Imaging
{
    public interface IPixelMap
    {
        int Bpp { get; }
        int BytesPerPixel { get; }
        int DataSize { get; }
        int Height { get; }
        byte[] Data { get; }
        int Width { get; }
        IList<PngRgbColor> Palette { get; }

        Task SaveAsync(Stream stream, PixelMapFormat format, CancellationToken ct = default);
        Task SaveAsync(string filename, CancellationToken ct = default);
    }
}