namespace Alaveri.Core.Imaging;

public interface IPngChunk
{
    string Type { get; }
    int Length { get; }
    byte[] Data { get; }
}