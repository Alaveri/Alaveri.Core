using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Runtime.InteropServices;
using Alaveri.Apl.Compression;

namespace Alaveri.Apl.Images;

public class AplImage : IAplImage
{
    public byte[] Buffer { get; private set; }

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

    protected async Task LzwCompressImage(Stream dest, AplCompressionLevel compressionLevel)
    {
        var compressor = new LzwCompressor(compressionLevel);
        using var source = new MemoryStream(Buffer);
        await compressor.CompressStreamAsync(source, dest, (int)source.Length);
    }

    public async Task SaveToStreamAsync(Stream stream, AplCompression compression = AplCompression.Lzw,
        AplCompressionLevel compressionLevel = AplCompressionLevel.High)
    {
        var header = new AplImageHeader(this, compression, compressionLevel);
        await header.SaveToStreamAsync(stream);
        Palette?.SaveToStream(stream);
        switch (compression)
        {
            case AplCompression.None:
                stream.Write(Buffer);
                break;
            case AplCompression.Lzw:
                await LzwCompressImage(stream, compressionLevel);
                break;
            default:
                throw new NotSupportedException("Unsupported compression type.");
        }
    }

    public AplImage(IAplImageHeader header, IAplPalette palette) : this(header.Width, header.Height, header.Bpp, header.Planes, palette)
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