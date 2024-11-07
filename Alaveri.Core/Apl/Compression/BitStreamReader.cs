using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Apl.Compression;

public class BitStreamReader(Stream source, int readBufferSize = CompressionConstants.DefaultReadBufferSize)
{
    public Stream Source { get; private set; } = source;

    public int ReadTotal { get; private set; }

    protected int ReadSize { get; private set; } = readBufferSize;

    protected int ReadPos { get; private set; } = -1;

    protected sbyte ReadBitCounter { get; private set; }

    protected int ReadBitBuffer { get; private set; }

    protected byte[] ReadBuffer { get; private set; } = new byte[readBufferSize];

    protected int ReadIndex { get; private set; }

    public async Task<byte> ReadByteAsync(CancellationToken ct = default)
    {
        if (ReadPos == -1 || ReadPos >= ReadSize)
        {
            var bytesRead = await Source.ReadAsync(ReadBuffer, ct);
            if (bytesRead == 0)
                throw new EndOfStreamException("Read past stream end");
            ReadPos = 0;
            ReadIndex = 0;
            ReadSize = bytesRead;
        }
        var result = ReadBuffer[ReadIndex];
        ReadTotal++;
        ReadPos++;
        ReadIndex++;
        return result;
    }

    public async Task<ushort> ReadBitsAsync(sbyte bitCount, CancellationToken ct = default)
    {
        while (ReadBitCounter < bitCount)
        {
            var readResult = await ReadByteAsync(ct);
            ReadBitBuffer |= readResult << ReadBitCounter;
            ReadBitCounter += 8;
        }
        var result = (ushort)(ReadBitBuffer & (1 << bitCount) - 1);
        ReadBitBuffer >>= bitCount;
        ReadBitCounter -= bitCount;
        return result;
    }
}
