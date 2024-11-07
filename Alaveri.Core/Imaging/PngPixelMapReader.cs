using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public class PngPixelMapReader(Stream source) : PixelMapReader(source)
{
    private static void ReadPalette(IPngChunk chunk, List<PngRgbColor> palette)
    {
        var data = new PngRgbColor[chunk.Length / 3];
        using var stream = new MemoryStream(chunk.Data);
        stream.Read(MemoryMarshal.AsBytes(data.AsSpan()));
        palette.AddRange(data);
    }

    private static async Task DecompressChunksAsync(IEnumerable<IPngChunk> chunks, Stream dest, CancellationToken ct = default)
    {
        using var source = new MemoryStream();
        foreach (var chunk in chunks)
            source.Write(chunk.Data);
        source.Seek(0, SeekOrigin.Begin);
        using var deflateStream = new ZLibStream(source, CompressionMode.Decompress);
        await deflateStream.CopyToAsync(dest, (int)source.Length, ct);
        deflateStream.Close();
        dest.Seek(0, SeekOrigin.Begin);
    }

    private static byte[] ReverseFilter(MemoryStream source, PngHeader header)
    {
        source.Seek(0, SeekOrigin.Begin);
        var bytesPerPixel = header.Bpp / 8;
        var bytesPerLine = header.Width * bytesPerPixel;
        var result = new byte[header.Width * header.Height * bytesPerPixel];
        int position = 0;
        for (int index = 0; index < header.Height; index++)
        {
            source.ReadByte();
            source.Read(result, position, bytesPerLine);
            position += bytesPerLine;
        }
        return result;
    }

    private static async Task<byte[]> ProcessDataAsync(IEnumerable<IPngChunk> chunks, PngHeader header, CancellationToken ct = default)
    {
        using var dest = new MemoryStream();
        await DecompressChunksAsync(chunks, dest, ct);
        return ReverseFilter(dest, header);
    }

    private static async Task<PixelMap> ProcessChunksAsync(IList<IPngChunk> chunks, PngHeader header, CancellationToken ct = default)
    {
        byte[] data = [];
        var palette = new List<PngRgbColor>();
        var paletteChunk = chunks.First(chunks => chunks.Type == PngConstants.PaletteChunk);
        ReadPalette(paletteChunk, palette);
        var dataChunks = chunks.Where(chunks => chunks.Type == PngConstants.DataChunk);
        data = await ProcessDataAsync(dataChunks, header, ct);
        return new PixelMap(header.Width, header.Height, header.Bpp, data, palette);
    }

    private async Task<List<IPngChunk>> ReadChunksAsync(CancellationToken ct = default)
    {
        var chunks = new List<IPngChunk>();
        while (true)
        {
            var chunk = await PngChunk.LoadFromStreamAsync(Source, ct) ?? throw new InvalidDataException("Unsupported PNG chunk");
            chunks.Add(chunk);
            if (chunk.Type == PngConstants.EndChunk)
                break;
        }
        if (!chunks.Any(chunks => chunks.Type == PngConstants.DataChunk))
            throw new InvalidDataException("No data chunks found");
        if (!chunks.Any(chunks => chunks.Type == PngConstants.PaletteChunk))
            throw new InvalidDataException("No palette chunks found");
        return chunks;
    }

    private static async Task<PngHeader> ReadHeaderAsync(List<IPngChunk> chunks, CancellationToken ct)
    {
        if (chunks.First().Type != PngConstants.HeaderChunk)
            throw new InvalidDataException("Invalid PNG header");
        var headerChunk = chunks.First();
        var bufferStream = new MemoryStream(headerChunk.Data);
        bufferStream.Seek(0, SeekOrigin.Begin);
        var header = new PngHeader();
        await header.LoadFromStreamAsync(bufferStream, headerChunk.Length, ct);
        return header;
    }

    private static async Task<PixelMap> CreateMapAsync(List<IPngChunk> chunks, CancellationToken ct = default)
    {
        if (chunks.Count < 4)
            throw new InvalidDataException("Invalid number of PNG chunks");
        var header = await ReadHeaderAsync(chunks, ct);
        return await ProcessChunksAsync(chunks, header, ct);
    }

    private async Task CheckSignatureAsync(CancellationToken ct)
    {
        var signature = new byte[PngConstants.Signature.Length];
        await Source.ReadAsync(signature, ct);
        if (!signature.SequenceEqual(PngConstants.Signature))
            throw new InvalidDataException("Invalid PNG signature");
    }

    public override async Task<IPixelMap> ReadMapAsync(CancellationToken ct = default)
    {        
        await CheckSignatureAsync(ct);
        var chunks = await ReadChunksAsync(ct);
        return await CreateMapAsync(chunks, ct);
    }
}
