namespace Alaveri.Core.Apl.Compression
{
    public interface ICompressor
    {
        bool AddTotalToStream { get; }
        int ProgressIncrement { get; set; }
        int ReadTotal { get; }
        int WriteTotal { get; }

        event EventHandler<AplCompressorProgressEventArgs>? Progress;

        Task<int> CompressStreamAsync(Stream source, Stream dest, int length, CancellationToken ct = default);
        Task DecompressStreamAsync(Stream source, Stream dest, int originalSize, CancellationToken ct = default);
        Task EndWriteBitsAsync(CancellationToken ct = default);
        Task FlushWriteBufferAsync(CancellationToken ct = default);
        Task<int> GetOriginalSizeAsync(Stream source, CancellationToken ct = default);
        Task<ushort> ReadBitsAsync(sbyte bitCount, CancellationToken ct = default);
        Task<byte> ReadByteAsync(CancellationToken ct = default);
        void UpdateProgress(int max, int current);
        Task WriteBitsAsync(ushort num, sbyte bitCount, CancellationToken ct = default);
        Task WriteByteAsync(byte value, CancellationToken ct = default);
    }
}