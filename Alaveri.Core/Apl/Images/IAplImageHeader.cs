using Alaveri.Core.Apl.Compression;

namespace Alaveri.Core.Apl.Images;

public interface IAplImageHeader
{
    byte Bpp { get; }
    AplCompression Compression { get; }
    AplCompressionLevel CompressionLevel { get; }
    int DataSize { get; }
    bool HasExtendedData { get; }
    bool HasPalette { get; }
    ushort Height { get; }
    string Identifier { get; }
    byte MajorVersion { get; }
    byte MinorVersion { get; }
    ushort PaletteSize { get; }
    byte Planes { get; }
    ushort Width { get; }

    Task SaveToStreamAsync(Stream stream, CancellationToken cancellationToken = default);
}