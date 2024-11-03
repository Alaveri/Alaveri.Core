using Alaveri.Core.Apl.Compression;

namespace Alaveri.Core.Apl.Images;

public interface IAplImage
{
    byte Bpp { get; }
    byte[] Buffer { get; }
    int DataSize { get; }
    ushort Height { get; }
    IAplPalette? Palette { get; }
    byte Planes { get; }
    ushort Width { get; }

    Task SaveToStreamAsync(Stream stream, byte planes = 1, AplCompression compression = AplCompression.Lzw, AplCompressionLevel compressionLevel = AplCompressionLevel.High);
}