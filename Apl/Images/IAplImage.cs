using Alaveri.Apl.Compression;

namespace Alaveri.Apl.Images;

public interface IAplImage
{
    byte Bpp { get; }
    byte[] Buffer { get; }
    int DataSize { get; }
    ushort Height { get; }
    IAplPalette? Palette { get; }
    byte Planes { get; }
    ushort Width { get; }

    Task SaveToStreamAsync(Stream stream, AplCompression compression = AplCompression.Lzw, AplCompressionLevel compressionLevel = AplCompressionLevel.High);
}