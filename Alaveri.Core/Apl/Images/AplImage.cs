using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Runtime.InteropServices;
using Alaveri.Core.Apl.Compression;
using SixLabors.ImageSharp;
using SkiaSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace Alaveri.Core.Apl.Images;

public class AplImage : IAplImage
{
    public byte[] Buffer { get; set; }

    public ushort Width { get; private set; }

    public ushort Height { get; private set; }

    public byte Bpp { get; private set; }

    public byte Planes { get; private set; }

    public IAplPalette? Palette { get; private set; }

    public int DataSize { get; private set; }

    public AplImage(ushort width, ushort height, byte bpp, byte planes, IAplPalette? palette = null)
    {
        Width = width;
        Height = height;
        Bpp = bpp;
        Planes = planes;
        Palette = palette;
        DataSize = width * height * bpp / 8;
        Buffer = new byte[DataSize];
    }

    protected static async Task LzwCompressImageAsync(Stream source, Stream dest, AplCompressionLevel compressionLevel)
    {
        var compressor = new LzwCompressor(compressionLevel);
        await compressor.CompressStreamAsync(source, dest, (int)source.Length);
    }

    public async Task SaveToStreamAsync(Stream stream, byte planes = 1, AplCompression compression = AplCompression.Lzw,
        AplCompressionLevel compressionLevel = AplCompressionLevel.High)
    {
        var header = new AplImageHeader(this, planes, compression, compressionLevel);
        await header.SaveToStreamAsync(stream);
        Palette?.SaveToStream(stream);
        using var bufferStream = new MemoryStream(Buffer);
        switch (compression)
        {
            case AplCompression.None:
                await bufferStream.CopyToAsync(stream);
                break;
            case AplCompression.Lzw:
                await LzwCompressImageAsync(bufferStream, stream, compressionLevel);
                break;
            default:
                throw new NotSupportedException("Unsupported compression type.");
        }
    }

    public static async Task<IAplImage> LoadFromStreamAsync(Stream stream, CancellationToken ct)
    {
        var header = await AplImageHeader.LoadFromStreamAsync(stream, ct);
        IAplPalette? palette = null;
        if (header.PaletteSize > 0)
            palette = await AplPalette.LoadFromStreamAsync(stream, header.PaletteSize, ct);
        var image = new AplImage(header, palette);
        using var dest = new MemoryStream(image.Buffer);
        switch (header.Compression)
        {
            case AplCompression.None:
                await stream.CopyToAsync(dest, ct);
                break;
            case AplCompression.Lzw:
                var decompressor = new LzwCompressor(header.CompressionLevel);
                await decompressor.DecompressStreamAsync(stream, dest, header.DataSize, ct);
                break;
            default:
                throw new NotSupportedException("Unsupported compression type.");
        }
        return image;
    }

    public static IAplImage FromSkiaBitmap(SKBitmap bitmap)
    {
        var image = new AplImage((ushort)bitmap.Width, (ushort)bitmap.Height, (byte)(bitmap.BytesPerPixel * 8), 1);
        var pixels = bitmap.GetPixels();
        var buffer = new byte[bitmap.RowBytes * bitmap.Height];
        Marshal.Copy(pixels, buffer, 0, buffer.Length);
        image.Buffer = buffer;
        return image;
    }

    public static IAplImage FromImageSharpImage(Image image)
    {
        throw new NotImplementedException();
    }

    public AplImage(IAplImageHeader header, IAplPalette? palette) : this(header.Width, header.Height, header.Bpp, header.Planes, palette)
    {
    }

    public AplImage(IAplImageHeader header) : this(header.Width, header.Height, header.Bpp, header.Planes)
    {
    }

    public AplImage(ushort width, ushort height, byte bpp) : this(width, height, bpp, 1)
    {
    }

    public AplImage(ushort width, ushort height) : this(width, height, 8, 1)
    {
    }

}