using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public class PixelMap(int width, int height, int bpp, byte[] data, IList<PngRgbColor> palette) : IPixelMap
{
    public int Width { get; } = width;

    public int Height { get; } = height;

    public int Bpp { get; } = bpp;

    public int BytesPerPixel { get => Bpp / 8; }

    public int DataSize => Width * Height * BytesPerPixel;

    public byte[] Data { get; } = data;

    public IList<PngRgbColor> Palette { get; } = palette;

    public PixelMap(int width, int height, int bpp) : this(width, height, bpp, new byte[width * height * bpp / 8], [])
    {
    }

    public PixelMap(int width, int height, int bpp, byte[] data) : this(width, height, bpp, data, [])
    {
    }

    public PixelMap(int width, int height, int bpp, IList<PngRgbColor> palette) : this(width, height, bpp, new byte[width * height * bpp / 8], palette)
    {
    }

    public async Task SaveAsync(Stream stream, PixelMapFormat format, CancellationToken ct = default)
    {
        var writer = PixelMapFactory.CreateWriter(stream, format);
        await writer.WriteAsync(this, ct);
    }

    public async Task SaveAsync(string filename, CancellationToken ct = default)
    {
        using var stream = File.Create(filename);
        var writer = PixelMapFactory.CreateWriter(stream, PixelMapFactory.GetFormat(filename));
        await writer.WriteAsync(this, ct);
    }

    public static async Task<IPixelMap> LoadAsync(Stream stream, PixelMapFormat format, CancellationToken ct = default)
    {
        var reader = PixelMapFactory.CreateReader(stream, format);
        return await reader.ReadMapAsync(ct);
    }

    public static async Task<IPixelMap> LoadAsync(string filename, CancellationToken ct = default)
    {
        using var stream = File.OpenRead(filename);
        var reader = PixelMapFactory.CreateReader(stream, PixelMapFactory.GetFormat(filename));
        return await reader.ReadMapAsync( ct);
    }

}
