using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Apl.Compression;

public class BitStreamWriter(Stream dest, int writeBufferSize = CompressionConstants.DefaultWriteBufferSize)
{
    public Stream Dest { get; private set; } = dest;

    protected ushort WritePos { get; private set; }

    protected sbyte WriteBitCounter { get; private set; }

    protected int WriteBitBuffer { get; private set; }

    protected byte[] WriteBuffer { get; private set; } = new byte[writeBufferSize];

    protected ushort WriteIndex { get; private set; }

    public int WriteTotal { get; private set; }

    public async Task WriteByteAsync(byte value, CancellationToken ct = default)
    {
        if (WritePos >= WriteBuffer.Length)
        {
            await Dest.WriteAsync(WriteBuffer.AsMemory(0, WritePos), ct);
            WritePos = 0;
            WriteIndex = 0;
        }
        WriteBuffer[WriteIndex] = value;
        WriteIndex++;
        WritePos++;
        WriteTotal++;
    }

    public async Task WriteBitsAsync(ushort num, sbyte bitCount, CancellationToken ct = default)
    {
        WriteBitBuffer |= num << WriteBitCounter;
        WriteBitCounter += bitCount;
        while (WriteBitCounter >= 8)
        {
            await WriteByteAsync((byte)(WriteBitBuffer & 0xFF), ct);
            WriteBitBuffer >>= 8;
            WriteBitCounter -= 8;
        }
    }

    public async Task FlushWriteBufferAsync(CancellationToken ct = default)
    {
        if (WritePos > 0)
        {
            await Dest.WriteAsync(WriteBuffer.AsMemory(0, WritePos), ct);
            WritePos = 0;
            WriteIndex = 0;
        }
    }

    public async Task EndWriteBitsAsync(CancellationToken ct = default)
    {
        if (WriteBitCounter > 0)
        {
            await WriteByteAsync((byte)WriteBitBuffer, ct);
            WriteBitBuffer = 0;
            WriteBitCounter = 0;
        }
        await FlushWriteBufferAsync(ct);
    }
}
