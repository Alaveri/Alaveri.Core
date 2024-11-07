using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public static class PngUtils
{
    public static uint GetUIntValue(byte[] bytes)
    {
        return BitConverter.IsLittleEndian ? BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0) : BitConverter.ToUInt32(bytes, 0);
    }

    public static int GetIntValue(byte[] bytes)
    {
        return BitConverter.IsLittleEndian ? BitConverter.ToInt32(bytes.Reverse().ToArray(), 0) : BitConverter.ToInt32(bytes, 0);
    }

    public static ushort GetUShortValue(byte[] bytes)
    {
        return BitConverter.IsLittleEndian ? BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0) : BitConverter.ToUInt16(bytes, 0);
    }

    public static async Task<uint> ReadUIntValueAsync(Stream stream)
    {
        byte[] buffer = new byte[4];
        await stream.ReadAsync(buffer);
        return GetUIntValue(buffer);
    }

    public static async Task<uint> ReadUIntValueAsync(Stream stream, CancellationToken ct = default)
    {
        byte[] buffer = new byte[4];
        await stream.ReadAsync(buffer, ct);
        return GetUIntValue(buffer);
    }

    public static async Task<int> ReadIntValueAsync(Stream stream, CancellationToken ct = default)
    {
        byte[] buffer = new byte[4];
        await stream.ReadAsync(buffer, ct);
        return GetIntValue(buffer);
    }

    public static async Task<ushort> ReadUInt16ValueAsync(Stream stream, CancellationToken ct = default)
    {
        byte[] buffer = new byte[2];
        await stream.ReadAsync(buffer, ct);
        return GetUShortValue(buffer);
    }
}