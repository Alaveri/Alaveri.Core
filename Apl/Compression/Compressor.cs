using Alaveri.Core.Extensions;

namespace Alaveri.Apl.Compression;

public class AplCompressorProgressEventArgs : EventArgs
{
    public int Max { get; set; }
    public int Current { get; set; }
}

public abstract class Compressor(ushort readBufferSize, ushort writeBufferSize) : ICompressor
{
    public const ushort DefaultProgressIncrement = 1024;

    protected ushort ReadBufferSize { get; private set; } = readBufferSize;

    protected ushort WriteBufferSize { get; private set; } = writeBufferSize;

    protected Stream? Source { get; private set; }

    protected Stream? Dest { get; private set; }

    protected ushort ReadSize { get; private set; }

    protected ushort ReadPos { get; private set; } = AplConstants.MaxVarSize;

    protected ushort WritePos { get; private set; }

    protected sbyte ReadBitCounter { get; private set; }

    protected sbyte WriteBitCounter { get; private set; }

    protected int ReadBitBuffer { get; private set; }

    protected int WriteBitBuffer { get; private set; }

    protected byte[]? ReadBuffer { get; private set; }

    protected byte[]? WriteBuffer { get; private set; }

    protected ushort ReadIndex { get; private set; }

    protected ushort WriteIndex { get; private set; }

    protected int ProgressCounter { get; private set; }

    public int ProgressIncrement { get; set; } = DefaultProgressIncrement;

    public event EventHandler<AplCompressorProgressEventArgs>? Progress;

    public int ReadTotal { get; private set; }

    public int WriteTotal { get; private set; }

    public bool AddTotalToStream { get; private set; }

    protected virtual void InitCompression()
    {
        ReadPos = AplConstants.MaxVarSize;
        WritePos = 0;
        WriteTotal = 0;
        ReadTotal = 0;
        WriteIndex = 0;
        ReadIndex = 0;
        ReadSize = ReadBufferSize;
        WriteBitBuffer = 0;
        ReadBitBuffer = 0;
        WriteBitCounter = 0;
        ReadBitCounter = 0;
        ProgressCounter = 0;
        ReadBuffer = new byte[ReadBufferSize];
        WriteBuffer = new byte[WriteBufferSize];
    }

    public virtual Task<int> CompressStreamAsync(Stream source, Stream dest, int length, CancellationToken ct = default)
    {
        InitCompression();
        Source = source;
        Dest = dest;
        return Task.FromResult(0);
    }

    public virtual Task DecompressStreamAsync(Stream source, Stream dest, int originalSize, CancellationToken ct = default)
    {
        InitCompression();
        Source = source;
        Dest = dest;
        return Task.CompletedTask;
    }

    public virtual void UpdateProgress(int max, int current)
    {
        if (current >= max || ProgressCounter >= ProgressIncrement)
        {
            Progress?.AsyncInvoke(this, new AplCompressorProgressEventArgs
            {
                Max = max,
                Current = current
            });
            ProgressCounter = 0;
            return;
        }
        ProgressCounter++;
    }

    public async Task<byte> ReadByteAsync(CancellationToken ct = default)
    {
        if (Source == null)
            throw new InvalidOperationException($"{nameof(Source)} cannot be null");
        if (ReadBuffer == null)
            throw new InvalidOperationException($"{nameof(ReadBuffer)} cannot be null");

        if (ReadPos == AplConstants.MaxVarSize || ReadPos >= ReadSize)
        {
            var bytesRead = await Source.ReadAsync(ReadBuffer.AsMemory(0, ReadBufferSize), ct);
            if (bytesRead == 0)
                throw new EndOfStreamException();
            ReadPos = 0;
            ReadIndex = 0;
            ReadSize = (ushort)bytesRead;
        }
        ReadTotal++;
        ReadPos++;
        var result = ReadBuffer[ReadIndex];
        ReadIndex++;
        return result;
    }

    public async Task WriteByteAsync(byte value, CancellationToken ct = default)
    {
        if (WriteBuffer == null)
            throw new InvalidOperationException($"{nameof(WriteBuffer)} cannot be null");
        if (Dest == null)
            throw new InvalidOperationException($"{nameof(Dest)} cannot be null");
        if (WritePos >= WriteBufferSize)
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
        if (WriteBuffer == null)
            throw new InvalidOperationException($"{nameof(WriteBuffer)} cannot be null");
        if (Dest == null)
            throw new InvalidOperationException($"{nameof(Dest)} cannot be null");

        if (WritePos > 0)
        {
            await Dest.WriteAsync(WriteBuffer.AsMemory(0, WritePos), ct);
            WritePos = 0;
            WriteIndex = 0;
        }
    }

    public async Task EndWriteBitsAsync(CancellationToken ct = default)
    {
        if (Source == null)
            throw new InvalidOperationException($"{nameof(Source)} cannot be null");

        if (WriteBitCounter > 0)
        {
            await WriteByteAsync((byte)WriteBitBuffer, ct);
            WriteBitBuffer = 0;
            WriteBitCounter = 0;
        }
        await FlushWriteBufferAsync(ct);
        if (ReadTotal % ProgressIncrement == 0)
            UpdateProgress((int)Source.Length, ReadTotal);
    }

    public async Task<ushort> ReadBitsAsync(sbyte bitCount, CancellationToken ct = default)
    {
        if (Source == null)
            throw new InvalidOperationException($"{nameof(Source)} cannot be null");

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

    public abstract Task<int> GetOriginalSizeAsync(Stream source, CancellationToken ct = default);

    public Compressor() : this(CompressionConstants.DefaultReadBufferSize, CompressionConstants.DefaultWriteBufferSize)
    {
    }
}
