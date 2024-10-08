using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Apl.Compression;

public class LzwCompressor(byte bitSize, ushort readBufferSize, ushort writeBufferSize) : Compressor(readBufferSize, writeBufferSize)
{
    private const byte DefaultBitSize = 13;
    private const byte StartBitSize = 9;
    private const ushort EndOfStream = 256;
    private const ushort IncreaseCodeSize = 257;
    private const ushort ClearDictionary = 258;
    private const ushort EmptyCode = 259;
    private const ushort FirstCode = 260;

    private struct DictionaryEntry
    {
        public ushort Code;
        public ushort Prefix;
        public byte Char;
    }

    private DictionaryEntry[] Dictionary { get; set; } = [];

    private int DictionaryEntries { get; set; }
    private byte[] DecodeBuffer { get; set; } = [];
    private byte BitSize { get; set; } = bitSize;
    private byte CurrentBitSize { get; set; } = DefaultBitSize;
    private ushort MaxCode { get; set; }
    private ushort CurrentMaxCode { get; set; }
    private ushort NextCode { get; set; }
    private bool Overflow { get; set; }
    private byte HashShift { get; set; }

    protected override void InitCompression()
    {
        base.InitCompression();
        DictionaryEntries = BitSize switch
        {
            12 => 5021,
            13 => 9029,
            14 => 18041,
            15 => 49063,
            _ => throw new NotSupportedException("Unsupported bit size. Supported bit sizes are 12-15."),
        };
        HashShift = (byte)(BitSize - 8);
        AllocateDictionary();
        InitCoder();
    }

    private void InitCoder()
    {
        CurrentBitSize = StartBitSize;
        MaxCode = (ushort)(1 << BitSize - 1);
        CurrentMaxCode = (ushort)(1 << CurrentBitSize - 1);
        NextCode = FirstCode;
        Overflow = false;
        for (var index = 0; index < DictionaryEntries; index++)
            Dictionary[index].Code = EmptyCode;
    }

    private void AllocateDictionary()
    {
        Dictionary = new DictionaryEntry[DictionaryEntries];
    }

    private void AllocateDecodeBuffer()
    {
        DecodeBuffer = new byte[DictionaryEntries];
    }

    private ushort FindEntry(ushort prefix, byte character)
    {
        var index = (character << HashShift) ^ prefix;
        var offset = 1;
        if (index != 0)
            offset = DictionaryEntries - index;
        do
        {
            var entry = Dictionary[index];
            if (entry.Code == EmptyCode)
                break;
            if (entry.Prefix == prefix && entry.Char == character)
                break;
            index -= offset;
            if (index < 0)
                index += DictionaryEntries;
        } while (true);
        return (ushort)index;
    }

    private ushort DecodeString(ushort count, ushort code)
    {
        var bufferPos = count;
        while (code > byte.MaxValue)
        {
            var entry = Dictionary[code];
            code = entry.Prefix;
            DecodeBuffer[bufferPos] = entry.Char;
            bufferPos++;
            count++;
        }
        DecodeBuffer[bufferPos] = (byte)code;
        count++;
        return count;
    }

    public override uint GetOriginalSize(Stream source)
    {
        var startPos = source.Position;
        source.ReadByte();
        using var reader = new BinaryReader(source, Encoding.Default, true);
        var total = reader.ReadUInt32();
        source.Seek(startPos, SeekOrigin.Begin);
        return total;
    }

    public override uint Compress(Stream source, Stream dest, uint length)
    {
        using var writer = new BinaryWriter(dest, Encoding.Default, true);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(dest);
        if (!source.CanRead)
            throw new ArgumentException("Source stream must be readable.", nameof(source));
        if (!dest.CanWrite)
            throw new ArgumentException("Destination stream must be writable.", nameof(dest));
        if (source.Position + length > source.Length)
            throw new ArgumentOutOfRangeException(nameof(length), "Length exceeds source stream length.");

        var total = 0u;
        var bits = BitSize;
        writer.Write(bits);
        var startPos = dest.Position;
        writer.Write(total);
        ushort code = ReadByte();

        while (ReadTotal < length)
        {
            var character = ReadByte();
            UpdateProgress(length, ReadTotal);
            var index = FindEntry(code, character);
            var entry = Dictionary[index];

            if (entry.Code != EmptyCode)
            {
                code = entry.Code;
                continue;
            }

            if (NextCode < MaxCode)
            {
                entry.Code = NextCode;
                entry.Prefix = code;
                entry.Char = character;
                NextCode++;
            }
            else
                Overflow = true;

            if (code >= CurrentMaxCode && CurrentBitSize < BitSize)
            {
                WriteBits(IncreaseCodeSize, CurrentBitSize);
                CurrentBitSize++;
                CurrentMaxCode = (ushort)(1 << CurrentBitSize - 1);
            }

            WriteBits(code, CurrentBitSize);
            code = character;

            if (Overflow)
            {
                WriteBits(ClearDictionary, CurrentBitSize);
                InitCoder();
            }
        }
        WriteBits(code, CurrentBitSize);
        WriteBits(EndOfStream, CurrentBitSize);
        EndWriteBits();
        total = ReadTotal;
        var endPos = dest.Position;
        dest.Seek(startPos, SeekOrigin.Begin);
        writer.Write(total);
        dest.Seek(endPos, SeekOrigin.Begin);
        UpdateProgress(length, ReadTotal);
        return WriteTotal;

    }

    public override void Decompress(Stream source, Stream dest)
    {
        using var reader = new BinaryReader(source, Encoding.Default, true);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(dest);
        if (!source.CanRead)
            throw new ArgumentException("Source stream must be readable.", nameof(source));
        if (!dest.CanWrite)
            throw new ArgumentException("Destination stream must be writable.", nameof(dest));
        var bits = reader.ReadByte();
        if (bits < 12 || bits > 15)
            throw new NotSupportedException("Unsupported bit size. Supported bit sizes are 12-15.");

        var total = reader.ReadUInt32();
        BitSize = bits;
        InitCoder();
        AllocateDecodeBuffer();

        var oldCode = ReadBits(bits);
        if (oldCode == EndOfStream)
            return;
        var character = (byte)oldCode;
        WriteByte(character);

        while (true)
        {
            var code = ReadBits(bits);
            if (code == EndOfStream)
                break;
            switch (code)
            {
                case IncreaseCodeSize:
                    CurrentBitSize++;
                    continue;
                case ClearDictionary:
                    InitCoder();
                    oldCode = ReadBits(bits);
                    if (oldCode == EndOfStream)
                        return;
                    character = (byte)oldCode;
                    WriteByte(character);
                    continue;
                case EndOfStream:
                    break;
            }
            var count = code >= NextCode ? DecodeString(1, oldCode) : DecodeString(0, code);
            var decodeIndex = count - 1;
            character = DecodeBuffer[decodeIndex];
            while (count > 0)
            {
                WriteByte(DecodeBuffer[decodeIndex]);
                decodeIndex--;
                count--;
            }
            UpdateProgress(total, WriteTotal);

            if (NextCode < MaxCode)
            {
                var entry = Dictionary[NextCode];
                entry.Prefix = oldCode;
                entry.Char = character;
                NextCode++;
            }
            oldCode = code;
        }
        FlushWriteBuffer();
    }

    public override Task<uint> CompressAsync(Stream source, Stream dest, uint length)
    {
        return Task.Run(() =>
        {
            return Compress(source, dest, length);
        });
    }

    public override Task DecompressAsync(Stream source, Stream dest)
    {
        return Task.Run(() =>
        {
            Decompress(source, dest);
        });
    }

    public LzwCompressor() : this(DefaultBitSize, DefaultWriteBufferSize, DefaultReadBufferSize)
    {
    }

    public LzwCompressor(byte bitSize) : this(bitSize, DefaultWriteBufferSize, DefaultReadBufferSize)
    {
    }

    public LzwCompressor(ushort readBufferSize, ushort writeBufferSize) : this(DefaultBitSize, readBufferSize, writeBufferSize)
    {
    }
}
