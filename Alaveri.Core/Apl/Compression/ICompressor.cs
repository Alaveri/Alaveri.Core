
namespace Alaveri.Core.Apl.Compression
{
    public interface ICompressor
    {
        bool AddTotalToStream { get; }
        BitStreamReader BitReader { get; }
        BitStreamWriter BitWriter { get; }
        int ProgressIncrement { get; set; }

        event EventHandler<AplCompressorProgressEventArgs>? Progress;

        Task<int> CompressStreamAsync(Stream source, Stream dest, int length, CancellationToken ct = default);
        Task DecompressStreamAsync(Stream source, Stream dest, int originalSize, CancellationToken ct = default);
        Task<int> GetOriginalSizeAsync(Stream source, CancellationToken ct = default);
        void UpdateProgress(int max, int current);
    }
}