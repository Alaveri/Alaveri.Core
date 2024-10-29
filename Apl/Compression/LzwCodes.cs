namespace Alaveri.Apl.Compression;

public abstract class LzwCodes(byte dataBitSize)
{
    public byte BitSize { get; set; } = dataBitSize;
    public ushort EndOfStream { get; set; }
    public ushort IncreaseCodeSize { get; set; }
    public ushort ClearDict { get; set; }
    public ushort EmptyCode { get; set; }
    public ushort FirstCode { get; set; }
    public bool DeferredClear { get; set; }
    public bool EncodeDataBitSize { get; set; }
}