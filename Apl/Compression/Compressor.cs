using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Apl.Compression;

public delegate void ProgressUpdateHandler(Compressor sender, uint total, uint current);

public abstract class Compressor
{
    public const ushort DefaultReadBufferSize = 0x1000;
    public const ushort DefaultWriteBufferSize = 0x1000;
    public const ushort PascalMaxVarSize = 0xFFF8;
    public ushort ReadBufferSize { get; }
    public ushort WriteBufferSize { get; }
    public ushort ReadSize { get; private set; }
    protected Stream? Source { get; set; }
    protected Stream? Dest { get; set; }
    protected StreamReader? Reader { get; set; }
    protected StreamWriter? Writer { get; set; }
    protected ushort ReadPosition { get; set; }
    protected ushort WritePosition { get; set; }
    protected ushort ReadBitCounter { get; set; }
    protected ushort WriteBitCounter { get; set; }
    protected uint ReadBitBuffer { get; set; }
    protected uint WriteBitBuffer { get; set; }
    protected byte[] ReadBuffer { get; set; } = [];
    protected byte[] WriteBuffer { get; set; } = [];
    protected ushort ReadBufferIndex { get; set; }
    protected ushort WriteBufferIndex { get; set; }
    protected ushort ProgressCounter { get; set; }
    protected uint ReadTotal { get; set; }
    protected uint WriteTotal { get; set; }
    public ushort ProgressIncrement { get; set; }

    public event ProgressUpdateHandler? ProgressUpdate;

    public abstract uint GetOriginalSize(Stream source);

    protected virtual void InitCompression()
    {
        ReadPosition = PascalMaxVarSize;
        WritePosition = 0;
        WriteTotal = 0;
        ReadTotal = 0;
        WriteBufferIndex = 0;
        ReadBufferIndex = 0;
        WriteBitBuffer = 0;
        ReadBitCounter = 0;
        WriteBitCounter = 0;
        ReadBitBuffer = 0;
        ProgressCounter = 0;
        ReadSize = ReadBufferSize;
        Source = null;
        Dest = null;
    }

    protected void FlushWriteBuffer()
    {
        if (WritePosition > 0)
        {
            Dest?.Write(WriteBuffer, 0, WritePosition);
            WritePosition = 0;
            WriteBufferIndex = 0;
        }
    }

    protected void EndWriteBits()
    {
        if (WriteBitCounter > 0)
        {
            var num = (byte)(WriteBitBuffer & 0xFF);
            WriteByte(num);
            WriteBitBuffer = 0;
            WriteBitCounter = 0;
        }
        FlushWriteBuffer();
        if (ReadTotal % ProgressIncrement == 0)
            UpdateProgress((uint)(Source?.Length ?? 0), ReadTotal);
    }

    protected void UpdateProgress(uint total, uint current)
    {
        if (current > total || ProgressCounter >= ProgressIncrement)
        {
            ProgressUpdate?.Invoke(this, total, current);
            ProgressCounter = 0;
            return;
        }
        ProgressCounter++;
    }

    protected void WriteByte(byte value)
    {
        if (WritePosition >= WriteBufferSize)
        {
            Dest?.Write(WriteBuffer, 0, WriteBufferSize);
            WritePosition = 0;
            WriteBufferIndex = 0;
        }
        WriteBuffer[WriteBufferIndex] = value;
        WriteBufferIndex++;
        WritePosition++;
        WriteTotal++;
    }

    protected void WriteBits(ushort bits, byte count)
    {
        WriteBitBuffer |= (uint)bits << WriteBitCounter;
        WriteBitCounter += count;
        while (WriteBitCounter >= 8)
        {
            var num = (byte)(WriteBitBuffer & 0xFF);
            WriteByte(num);
            WriteBitBuffer >>= 8;
            WriteBitCounter -= 8;
        }
    }

    protected byte ReadByte()
    {
        if (ReadPosition == PascalMaxVarSize || ReadPosition >= ReadBufferSize)
        {
            var bytesRead = Source?.Read(ReadBuffer, 0, ReadBufferSize) ?? 0;
            if (bytesRead == 0)
                throw new EndOfStreamException("Unexpected end of stream.");
            ReadPosition = 0;
            ReadBufferIndex = 0;
            ReadSize = (ushort)bytesRead;
        }
        var result = ReadBuffer[ReadBufferIndex];
        ReadPosition++;
        ReadTotal++;
        ReadBufferIndex++;
        return result;
    }

    protected ushort ReadBits(byte count)
    {
        while (ReadBitCounter < count)
        {
            var readResult = ReadByte();
            ReadBitBuffer |= (uint)readResult << ReadBitCounter;
            ReadBitCounter += 8;
        }
        var result = (ushort)(ReadBitBuffer & ((1u << count) - 1));
        ReadBitBuffer >>= count;
        ReadBitCounter -= count;
        return result;
    }

    public abstract uint Compress(Stream source, Stream dest, uint length);

    public abstract void Decompress(Stream source, Stream dest);

    public abstract Task<uint> CompressAsync(Stream source, Stream dest, uint length);

    public abstract Task DecompressAsync(Stream source, Stream dest);

    public Compressor(ushort readBufferSize, ushort writeBufferSize)
    {
        ReadBufferSize = readBufferSize;
        WriteBufferSize = writeBufferSize;
        ReadBuffer = new byte[ReadBufferSize];
        WriteBuffer = new byte[WriteBufferSize];
    }

    public Compressor() : this(DefaultReadBufferSize, DefaultWriteBufferSize)
    {
    }
}
