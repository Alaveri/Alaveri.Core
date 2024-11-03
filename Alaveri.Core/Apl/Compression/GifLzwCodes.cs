namespace Alaveri.Core.Apl.Compression;

public class GifLzwCodes : LzwCodes
{
    public GifLzwCodes(byte dataBitSize) : base(dataBitSize)
    {
        ClearDict = (ushort)(1 << dataBitSize);
        EndOfStream = (ushort)(ClearDict + 1);
        FirstCode = (ushort)(ClearDict + 2);
        IncreaseCodeSize = (ushort)(ClearDict + 3);
        EmptyCode = 0xFFFF;
        DeferredClear = true;
        EncodeDataBitSize = true;
    }
}
