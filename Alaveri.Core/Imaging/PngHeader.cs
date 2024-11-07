using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.IO.Hashing;

namespace Alaveri.Core.Imaging;

public class PngHeader
{
    public string Type { get; protected set; } = "IHDR";

    public int Width { get; private set; }

    public int Height { get; private set; }

    public byte BitDepth { get; private set; } // Bits per sample/palette index

    public PngColorType ColorType { get; private set; }

    public PngCompressionMethod CompressionMethod { get; private set; }

    public PngFilterMethod FilterMethod { get; private set; }

    public PngInterlaceMethod InterlaceMethod { get; private set; }

    public bool PaletteUsed => ((int)ColorType & 1) != 0;

    public bool ColorUsed => ((int)ColorType & 2) != 0;

    public bool AlphaChannelUsed => ((int)ColorType & 4) != 0;

    public byte Bpp => PngConstants.BitsPerPixel(ColorType, BitDepth);

    public MemoryStream Data { get; private set; } = new MemoryStream();

    public async Task LoadFromStreamAsync(Stream stream, int length, CancellationToken ct = default)
    {
        Data = new MemoryStream();
        Data.Write(Encoding.ASCII.GetBytes(Type));
        var data = new byte[length];
        await stream.ReadAsync(data.AsMemory(), ct);
        Data.Write(data);
        Data.Seek(0, SeekOrigin.Begin);

        using var dataReader = new BinaryReader(Data, Encoding.ASCII, true);
        var type = dataReader.ReadBytes(4);
        if (Encoding.ASCII.GetString(type) != Type)
            throw new NotSupportedException("Invalid PNG chunk type.");

        Width = PngUtils.GetIntValue(dataReader.ReadBytes(4));
        Height = PngUtils.GetIntValue(dataReader.ReadBytes(4));
        BitDepth = dataReader.ReadByte();
        ColorType = (PngColorType)dataReader.ReadByte();
        CompressionMethod = (PngCompressionMethod)dataReader.ReadByte();
        FilterMethod = (PngFilterMethod)dataReader.ReadByte();
        var method = dataReader.ReadByte();
        InterlaceMethod = (PngInterlaceMethod)method;
        if (CompressionMethod != PngCompressionMethod.Deflate)
            throw new NotSupportedException("Unsupported PNG compression method.");
        if (!Enum.IsDefined(typeof(PngColorType), ColorType))
            throw new NotSupportedException("Unsupported PNG color type.");
        if (!Enum.IsDefined(typeof(PngFilterMethod), FilterMethod))
            throw new NotSupportedException("Unsupported PNG filter method.");
        if (!Enum.IsDefined(typeof(PngInterlaceMethod), InterlaceMethod))
            throw new NotSupportedException("Unsupported PNG interlace method.");
        if (PngConstants.AllowedBitDepths(ColorType).Length == 0)
            throw new NotSupportedException("Unsupported PNG bit depth.");
    }

    public async Task LoadFromFileAsync(string filename, int length, CancellationToken ct = default)
    {
        using var stream = File.OpenRead(filename);
        await LoadFromStreamAsync(stream, length, ct);
    }

    public async Task SaveToStreamAsync(Stream stream, CancellationToken ct = default)
    {
        var dest = new MemoryStream();
        using var writer = new BinaryWriter(dest, Encoding.ASCII, true);
        if (CompressionMethod != PngCompressionMethod.Deflate)
            throw new NotSupportedException("Unsupported PNG compression method.");
        if (!Enum.IsDefined(typeof(PngColorType), ColorType))
            throw new NotSupportedException("Unsupported PNG color type.");
        if (!Enum.IsDefined(typeof(PngFilterMethod), FilterMethod))
            throw new NotSupportedException("Unsupported PNG filter method.");
        if (!Enum.IsDefined(typeof(PngInterlaceMethod), InterlaceMethod))
            throw new NotSupportedException("Unsupported PNG interlace method.");
        if (PngConstants.AllowedBitDepths(ColorType).Length == 0)
            throw new NotSupportedException("Unsupported PNG bit depth.");
        
        writer.Write(Width);
        writer.Write(Height);
        writer.Write(BitDepth);
        writer.Write((byte)ColorType);
        writer.Write((byte)CompressionMethod);
        writer.Write((byte)FilterMethod);
        writer.Write((byte)InterlaceMethod);
        writer.Write(Crc32.HashToUInt32(dest.ToArray()));
        dest.Seek(0, SeekOrigin.Begin);
        await dest.CopyToAsync(stream, ct);
    }

    public async Task SaveToFileAsync(string filename, CancellationToken ct = default)
    {
        using var stream = File.Create(filename);
        await SaveToStreamAsync(stream, ct);
    }
}
