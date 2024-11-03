namespace Alaveri.Core.Apl.Compression;

public class AplLzwCodes : LzwCodes
{
    public AplLzwCodes(byte dataBitSize) : base(dataBitSize)
    {
        EndOfStream = (ushort)(1 << dataBitSize);
        IncreaseCodeSize = (ushort)(EndOfStream + 1);
        ClearDict = (ushort)(EndOfStream + 2);
        EmptyCode = (ushort)(EndOfStream + 3);
        FirstCode = (ushort)(EndOfStream + 4);
        DeferredClear = false;
        EncodeDataBitSize = false;
    }
}