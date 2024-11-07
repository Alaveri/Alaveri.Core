using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using Alaveri.Core;
using Alaveri.Core.Apl.Compression;

namespace Alaveri.Core.Apl.Images;

public struct AplImageHeader : IAplImageHeader
{
    public byte[] Identifier { get; private set; } = AplImageConstants.AplImageIdentifier;
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

    public readonly MemoryStream WriteToMemoryStream()
    {
        var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.Default, true);
        writer.Write(Identifier);
        writer.Write(MajorVersion);
        writer.Write(MinorVersion);
        writer.Write(Width);
        writer.Write(Height);
        writer.Write(Bpp);
        writer.Write(Planes);
        writer.Write(HasPalette);
        writer.Write(PaletteSize);
        writer.Write((byte)Compression);
        writer.Write((byte)CompressionLevel);
        writer.Write(DataSize);
        writer.Write(HasExtendedData);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    public static AplImageHeader ReadFromMemoryStream(MemoryStream stream)
    {
        var header = new AplImageHeader();
        using var reader = new BinaryReader(stream, Encoding.Default, true);
        header.Identifier = reader.ReadBytes(6);
        if (header.Identifier != AplImageConstants.AplImageIdentifier)
            throw new FileFormatException("Image file header was missing or incorrect.");
        header.MajorVersion = reader.ReadByte();
        header.MinorVersion = reader.ReadByte();
        header.Width = reader.ReadUInt16();
        header.Height = reader.ReadUInt16();
        header.Bpp = reader.ReadByte();
        header.Planes = reader.ReadByte();
        header.HasPalette = reader.ReadBoolean();
        header.PaletteSize = reader.ReadUInt16();
        header.Compression = (AplCompression)reader.ReadByte();
        header.CompressionLevel = (AplCompressionLevel)reader.ReadByte();
        header.DataSize = reader.ReadInt32();
        header.HasExtendedData = reader.ReadBoolean();
        return header;
    }

    public static async Task<AplImageHeader> LoadFromStreamAsync(Stream stream, CancellationToken ct = default)
    {
        using var headerStream = new MemoryStream();
        await stream.CopyToAsync(headerStream, ct);
        headerStream.Seek(0, SeekOrigin.Begin);
        return ReadFromMemoryStream(headerStream);
    }

    public static AplImageHeader LoadFromStream(Stream stream)
    {
        using var headerStream = new MemoryStream();
        stream.CopyTo(headerStream);
        headerStream.Seek(0, SeekOrigin.Begin);
        return ReadFromMemoryStream(headerStream);
    }

    public async readonly Task SaveToStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var headerStream = WriteToMemoryStream();
        await headerStream.CopyToAsync(stream, cancellationToken);
    }

    public readonly void SaveToStream(Stream stream)
    {
        using var headerStream = WriteToMemoryStream();
        headerStream.CopyTo(stream);
    }

    public AplImageHeader(IAplImage image, byte planes, AplCompression compression = AplCompression.Lzw,
        AplCompressionLevel compressionLevel = AplCompressionLevel.High) : this()
    {
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
        MajorVersion = AplImageConstants.AplImageMajorVersion;
        MinorVersion = AplImageConstants.AplImageMinorVersion;
    }
}