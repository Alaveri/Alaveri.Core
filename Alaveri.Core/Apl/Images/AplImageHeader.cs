using System.Text;
using Alaveri.Core;
using Alaveri.Core.Apl.Compression;

namespace Alaveri.Core.Apl.Images;

public struct AplImageHeader : IAplImageHeader
{
    public string Identifier { get; private set; } = AplImageConstants.AplImageIdentifier;
    public byte MajorVersion { get; private set; }
    public byte MinorVersion { get; private set; }
    public ushort Width { get; private set; }
    public ushort Height { get; private set; }
    public byte Bpp { get; private set; }
    public byte Planes { get; private set; }
    public bool HasPalette { get; private set; }
    public ushort PaletteSize { get; private set; }
    public AplCompression Compression { get; private set; }
    public AplCompressionLevel CompressionLevel { get; private set; }
    public int DataSize { get; private set; }
    public bool HasExtendedData { get; private set; }

    public static async Task<AplImageHeader> LoadFromStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var header = new AplImageHeader();
        using var reader = new AsyncBinaryReader(stream, Encoding.ASCII, true);
        var id = await reader.ReadBytes(6, cancellationToken);
        header.Identifier = Encoding.ASCII.GetString(id);
        if (header.Identifier != AplImageConstants.AplImageIdentifier)
            throw new FileFormatException("Image file header was missing or incorrect.");

        header.MajorVersion = await reader.ReadByte(cancellationToken);
        header.MinorVersion = await reader.ReadByte(cancellationToken);
        if (header.MajorVersion > AplImageConstants.AplImageMajorVersion ||
            header.MajorVersion == AplImageConstants.AplImageMajorVersion &&
            header.MinorVersion > AplImageConstants.AplImageMinorVersion)
            throw new UnsupportedFileVersionException("Unsupported image version.", header.MajorVersion, header.MinorVersion);

        header.Width = await reader.ReadUInt16(cancellationToken);
        header.Height = await reader.ReadUInt16(cancellationToken);
        header.Bpp = await reader.ReadByte(cancellationToken);
        header.Planes = await reader.ReadByte(cancellationToken);
        header.HasPalette = await reader.ReadBoolean(cancellationToken);
        header.PaletteSize = await reader.ReadUInt16(cancellationToken);
        header.Compression = (AplCompression)await reader.ReadByte(cancellationToken);
        header.CompressionLevel = (AplCompressionLevel)await reader.ReadByte(cancellationToken);
        header.DataSize = await reader.ReadInt32(cancellationToken);
        header.HasExtendedData = await reader.ReadBoolean(cancellationToken);
        return header;
    }

    public async readonly Task SaveToStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var writer = new AsyncBinaryWriter(stream, Encoding.ASCII, true);
        await writer.Write(Encoding.ASCII.GetBytes(Identifier), cancellationToken);
        await writer.Write(MajorVersion, cancellationToken);
        await writer.Write(MinorVersion, cancellationToken);
        await writer.Write(Width, cancellationToken);
        await writer.Write(Height, cancellationToken);
        await writer.Write(Bpp, cancellationToken);
        await writer.Write(Planes, cancellationToken);
        await writer.Write(HasPalette, cancellationToken);
        await writer.Write(PaletteSize, cancellationToken);
        await writer.Write((byte)Compression, cancellationToken);
        await writer.Write((byte)CompressionLevel, cancellationToken);
        await writer.Write(DataSize, cancellationToken);
        await writer.Write(HasExtendedData, cancellationToken);
    }

    public AplImageHeader(IAplImage image, byte planes, AplCompression compression = AplCompression.Lzw,
        AplCompressionLevel compressionLevel = AplCompressionLevel.High)
    {
        MajorVersion = AplImageConstants.AplImageMajorVersion;
        MinorVersion = AplImageConstants.AplImageMinorVersion;
        Width = image.Width;
        Height = image.Height;
        Bpp = image.Bpp;
        Planes = planes;
        HasPalette = image.Palette != null;
        PaletteSize = (ushort)(image.Palette?.Colors.Count ?? 0);
        Compression = compression;
        CompressionLevel = compressionLevel;
        DataSize = image.DataSize;
        HasExtendedData = false;
    }

    public AplImageHeader()
    {
    }
}