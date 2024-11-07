using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Hashing;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.PortableExecutable;

namespace Alaveri.Core.Imaging;

public class PngChunk(string type) : IPngChunk
{
    public string Type { get; private set; } = type;

    public int Length { get; private set; }

    public byte[] Data { get; private set; } = [];

    private static async Task<byte[]> GetDataAsync(Stream stream, string type, int length, CancellationToken ct = default)
    {
        var buffer = new byte[length + 4];
        using var result = new MemoryStream(buffer);
        result.Write(Encoding.ASCII.GetBytes(type));
        var dataBytes = new byte[length];
        await stream.ReadAsync(dataBytes.AsMemory(), ct);
        result.Write(dataBytes);
        var bufferCrc32 = Crc32.HashToUInt32(buffer);
        var data = buffer.AsMemory(4).ToArray();
        var crcBytes = new byte[4];
        var crc = await PngUtils.ReadUIntValueAsync(stream, ct);
        if (crc != bufferCrc32)
            throw new InvalidDataException("CRC mismatch");
        return data;
    }

    public static async Task<IPngChunk> LoadFromStreamAsync(Stream stream, CancellationToken ct = default)
    {
        var length = await PngUtils.ReadIntValueAsync(stream, ct);
        var typeBytes = new byte[4];
        await stream.ReadAsync(typeBytes, ct);
        var type = Encoding.ASCII.GetString(typeBytes);
        var data = await GetDataAsync(stream, type, length, ct);
        var result = new PngChunk(type) { Length = length, Data = data };
        return result;
    }
}
