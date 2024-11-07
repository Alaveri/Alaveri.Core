using Alaveri.Core.Extensions;

namespace Alaveri.Core.Apl.Compression;

public class AplCompressorProgressEventArgs : EventArgs
{
    public int Max { get; set; }
    public int Current { get; set; }
}

public abstract class Compressor(ushort readBufferSize = CompressionConstants.DefaultReadBufferSize,
    ushort writeBufferSize = CompressionConstants.DefaultWriteBufferSize) : ICompressor
{
    public const ushort DefaultProgressIncrement = 1024;

    protected int ProgressCounter { get; private set; }

    public int ProgressIncrement { get; set; } = DefaultProgressIncrement;

    public event EventHandler<AplCompressorProgressEventArgs>? Progress;

    public bool AddTotalToStream { get; private set; }

    public BitStreamReader BitReader { get; private set; } = new BitStreamReader(Stream.Null);

    public BitStreamWriter BitWriter { get; private set; } = new BitStreamWriter(Stream.Null);

    protected virtual void InitCompression(Stream source, Stream dest)
    {
        BitReader = new BitStreamReader(source, readBufferSize);
        BitWriter = new BitStreamWriter(dest, writeBufferSize);
    }

    public virtual Task<int> CompressStreamAsync(Stream source, Stream dest, int length, CancellationToken ct = default)
    {
        InitCompression(source, dest);
        return Task.FromResult(0);
    }

    public virtual Task DecompressStreamAsync(Stream source, Stream dest, int originalSize, CancellationToken ct = default)
    {
        InitCompression(source, dest);
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

    public abstract Task<int> GetOriginalSizeAsync(Stream source, CancellationToken ct = default);

    public Compressor() : this(CompressionConstants.DefaultReadBufferSize, CompressionConstants.DefaultWriteBufferSize)
    {
    }
}
