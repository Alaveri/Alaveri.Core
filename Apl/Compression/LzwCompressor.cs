using Alaveri.Core;

namespace Alaveri.Apl.Compression;

public class LzwCompressor(byte dataBitSize, byte bitSize) : Compressor
{
    public LzwCodes Codes { get; set; } = new AplLzwCodes(dataBitSize);

    public byte BitSize { get; set; } = bitSize;

    public byte DataBitSize { get; set; } = dataBitSize;

    protected sbyte CurrentBitSize { get; set; } = (sbyte)bitSize;

    protected LzwDictionaryEntry[] Dictionary { get; set; } = [];

    protected ushort NextCode { get; set; }

    protected ushort MaxCode { get; set; }

    protected ushort CurrentMaxCode { get; set; }

    protected byte HashShift { get; set; }

    protected byte[] DecodeBuffer { get; set; } = [];

    protected bool Overflow { get; set; }

    public ushort DictEntryCount { get; set; }

    protected ushort DecodeString(ushort count, ushort code)
    {
        int bufferIndex = count;
        while (code > byte.MaxValue)
        {
            var entry = Dictionary[code];
            code = entry.Prefix;
            DecodeBuffer[bufferIndex] = entry.Character;
            count++;
            bufferIndex++;
        }
        DecodeBuffer[bufferIndex] = (byte)code;
        count++;
        return count;
    }

    protected override void InitCompression()
    {
        base.InitCompression();
        DictEntryCount = BitSize switch
        {
            12 => 5021,
            13 => 9029,
            14 => 18041,
            15 => 49063,
            _ => 4096,
        };
        HashShift = (byte)(BitSize - 8);
        Dictionary = new LzwDictionaryEntry[DictEntryCount];
        InitCoder();
    }

    protected void InitCoder()
    {
        CurrentBitSize = LzwConstants.StartBitSize;
        MaxCode = (ushort)((1 << BitSize) - 1);
        CurrentMaxCode = (ushort)((1 << CurrentBitSize) - 1);
        NextCode = Codes.FirstCode;
        Overflow = false;
        for (int index = 0; index < Dictionary.Length - 1; index++)
        {
            Dictionary[index] = new LzwDictionaryEntry
            {
                Code = Codes.EmptyCode,
                Prefix = Codes.EmptyCode,
                Character = (byte)index
            };
        }
    }

    protected ushort FindEntry(ushort prefix, byte character)
    {
        var index = character << HashShift ^ prefix;
        var offset = 1;
        if (index != 0)
            offset = DictEntryCount - index;

        while (true)
        {
            var entry = Dictionary[index];
            if (entry.Code == Codes.EmptyCode)
                break;
            if (entry.Prefix == prefix && entry.Character == character)
                break;
            index -= offset;
            if (index < 0)
                index += DictEntryCount;
        }
        return (ushort)index;
    }

    public override async Task<int> GetOriginalSizeAsync(Stream source, CancellationToken ct = default)
    {
        using var reader = new AsyncBinaryReader(source);
        var startPos = source.Position;
        await reader.ReadByte(ct);
        var total = await reader.ReadInt32(ct);
        source.Seek(startPos, SeekOrigin.Begin);
        return total;
    }

    public override async Task<int> CompressStreamAsync(Stream source, Stream dest, int length, CancellationToken ct = default)
    {
        await base.CompressStreamAsync(source, dest, length, ct);
        var destStart = dest.Position;
        if (source.Position + length > source.Length)
            throw new EndOfStreamException("Read past stream end");

        using var writer = new BinaryWriter(dest);
        using var reader = new BinaryReader(source);

        writer.Write(BitSize);
        ushort code = await ReadByteAsync(ct);
        if (AddTotalToStream)
            writer.Write(length);

        while (ReadTotal < length)
        {
            if (ct.IsCancellationRequested)
                return 0;
            var character = await ReadByteAsync(ct);
            UpdateProgress(length, ReadTotal);
            var index = FindEntry(code, character);
            var entry = Dictionary[index];
            if (entry.Code != Codes.EmptyCode)
            {
                code = entry.Code;
                continue;
            }
            if (NextCode < MaxCode)
            {
                Dictionary[index].Code = NextCode;
                Dictionary[index].Prefix = code;
                Dictionary[index].Character = character;
                NextCode++;
            }
            else
                Overflow = true;


            if (code >= CurrentMaxCode && CurrentBitSize < BitSize)
            {
                await WriteBitsAsync(Codes.IncreaseCodeSize, CurrentBitSize, ct);
                CurrentBitSize++;
                CurrentMaxCode = (ushort)((1 << CurrentBitSize) - 1);
            }
            await WriteBitsAsync(code, CurrentBitSize, ct);
            code = character;
            if (Overflow)
            {
                await WriteBitsAsync(Codes.ClearDict, CurrentBitSize, ct);
                InitCoder();
            }
        }
        await WriteBitsAsync(code, CurrentBitSize, ct);
        await WriteBitsAsync(Codes.EndOfStream, CurrentBitSize, ct);
        await EndWriteBitsAsync(ct);
        UpdateProgress(length, ReadTotal);
        return (int)(dest.Position - destStart);
    }

    public override async Task DecompressStreamAsync(Stream source, Stream dest, int originalSize, CancellationToken ct = default)
    {
        await base.DecompressStreamAsync(source, dest, originalSize, ct);

        using var reader = new BinaryReader(source);
        using var writer = new BinaryWriter(dest);

        BitSize = reader.ReadByte();
        if (!LzwConstants.SupportedBitSizes.Contains(BitSize))
            throw new NotSupportedException("Lzw bit size not supported");

        if (AddTotalToStream)
            originalSize = reader.ReadInt32();

        InitCoder();
        DecodeBuffer = new byte[DictEntryCount];
        var oldCode = await ReadBitsAsync(CurrentBitSize, ct);
        if (oldCode == Codes.EndOfStream)
            return;
        var character = oldCode;
        await WriteByteAsync((byte)oldCode, ct);

        while (true)
        {
            var code = await ReadBitsAsync(CurrentBitSize, ct);
            if (code == Codes.IncreaseCodeSize)
            {
                CurrentBitSize++;
                continue;
            }
            if (code == Codes.ClearDict)
            {
                InitCoder();
                oldCode = await ReadBitsAsync(CurrentBitSize, ct);
                if (oldCode == Codes.EndOfStream)
                    break;
                await WriteByteAsync((byte)oldCode, ct);
                continue;
            }
            if (code == Codes.EndOfStream)
                break;
            ushort count;
            if (code >= NextCode)
            {
                DecodeBuffer[0] = (byte)character;
                count = DecodeString(1, oldCode);
            }
            else
                count = DecodeString(0, code);

            var decodeIndex = count - 1;
            character = DecodeBuffer[decodeIndex];

            while (count > 0)
            {
                await WriteByteAsync(DecodeBuffer[decodeIndex], ct);
                decodeIndex--;
                count--;
            }
            UpdateProgress(originalSize, WriteTotal);
            if (NextCode <= MaxCode)
            {
                var entry = Dictionary[NextCode];
                entry.Prefix = oldCode;
                entry.Character = (byte)character;
                NextCode++;
                if (Codes.DeferredClear && NextCode >= MaxCode && CurrentBitSize < BitSize)
                {
                    CurrentBitSize = (sbyte)BitSize;
                    MaxCode = (ushort)(MaxCode << 1);
                }
            }
            oldCode = code;
        }
        await FlushWriteBufferAsync(ct);
    }

    public LzwCompressor(byte dataBitSize, AplCompressionLevel compressionLevel)
        : this(dataBitSize, LzwConstants.AplLzwCompressionLevels[compressionLevel])
    {
    }

    public LzwCompressor(AplCompressionLevel compressionLevel)
        : this(8, compressionLevel)
    {
    }

    public LzwCompressor() : this(8, LzwConstants.DefaultBitSize)
    {
    }
}
