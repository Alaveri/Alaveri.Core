using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core;
public class AsyncBinaryReader : IDisposable
{
    private readonly Stream _stream;

    private readonly Encoding _encoding;

    private readonly bool _leaveOpen;

    private bool _disposed;

    public virtual Stream BaseStream => _stream;

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    public AsyncBinaryReader(Stream input, Encoding encoding, bool leaveOpen)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(encoding);

        if (!input.CanRead)
            throw new ArgumentException("Stream is not readable.", nameof(input));

        _stream = input;
        _encoding = encoding;
        _leaveOpen = leaveOpen;
    }

    public virtual async Task<bool> ReadBoolean(CancellationToken cancellationToken = default) => await ReadByte(cancellationToken) != 0;

    public virtual async Task<TValue> ReadValue<TValue>(CancellationToken cancellationToken = default)
        where TValue : struct
    {
        ThrowIfDisposed();
        var size = Marshal.SizeOf<TValue>();
        var buffer = new byte[size];
        await _stream.ReadExactlyAsync(buffer, cancellationToken);
        return MemoryMarshal.Read<TValue>(buffer);
    }

    public virtual async Task<byte> ReadByte(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var result = await ReadValue<int>(cancellationToken);
        if (result == -1)
            throw new EndOfStreamException();
        return (byte)result;
    }

    public virtual async Task<int> ReadInt32(CancellationToken cancellationToken = default)
    {
        return await ReadValue<int>(cancellationToken);
    }

    public virtual async Task<short> ReadInt16(CancellationToken cancellationToken = default)
    {
        return await ReadValue<short>(cancellationToken);
    }

    public virtual async Task<ushort> ReadUInt16(CancellationToken cancellationToken = default)
    {
        return await ReadValue<ushort>(cancellationToken);
    }

    public virtual async Task<uint> ReadUInt32(CancellationToken cancellationToken = default)
    {
        return await ReadValue<uint>(cancellationToken);
    }

    public virtual async Task<long> ReadInt64(CancellationToken cancellationToken = default)
    {
        return await ReadValue<long>(cancellationToken);
    }

    public virtual async Task<ulong> ReadUInt64(CancellationToken cancellationToken = default)
    {
        return await ReadValue<ulong>(cancellationToken);
    }

    public virtual async Task<float> ReadSingle(CancellationToken cancellationToken = default)
    {
        return await ReadValue<float>(cancellationToken);
    }

    public virtual async Task<double> ReadDouble(CancellationToken cancellationToken = default)
    {
        return await ReadValue<double>(cancellationToken);
    }

    public virtual async Task<decimal> ReadDecimal(CancellationToken cancellationToken = default)
    {
        return await ReadValue<decimal>(cancellationToken);
    }

    public async Task<int> Read7BitEncodedInt()
    {
        // Unlike writing, we can't delegate to the 64-bit read on
        // 64-bit platforms. The reason for this is that we want to
        // stop consuming bytes if we encounter an integer overflow.

        uint result = 0;
        byte byteReadJustNow;

        // Read the integer 7 bits at a time. The high bit
        // of the byte when on means to continue reading more bytes.
        //
        // There are two failure cases: we've read more than 5 bytes,
        // or the fifth byte is about to cause integer overflow.
        // This means that we can read the first 4 bytes without
        // worrying about integer overflow.

        const int MaxBytesWithoutOverflow = 4;
        for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
        {
            // ReadByte handles end of stream cases for us.
            byteReadJustNow = await ReadByte();
            result |= (byteReadJustNow & 0x7Fu) << shift;

            if (byteReadJustNow <= 0x7Fu)
                return (int)result; // early exit
        }

        // Read the 5th byte. Since we already read 28 bits,
        // the value of this byte must fit within 4 bits (32 - 28),
        // and it must not have the high bit set.

        byteReadJustNow = await ReadByte();
        if (byteReadJustNow > 0b_1111u)
            throw new FormatException("Bad string length");

        result |= (uint)byteReadJustNow << MaxBytesWithoutOverflow * 7;
        return (int)result;
    }

    public async Task<long> Read7BitEncodedInt64()
    {
        ulong result = 0;
        byte byteReadJustNow;

        // Read the integer 7 bits at a time. The high bit
        // of the byte when on means to continue reading more bytes.
        //
        // There are two failure cases: we've read more than 10 bytes,
        // or the tenth byte is about to cause integer overflow.
        // This means that we can read the first 9 bytes without
        // worrying about integer overflow.

        const int MaxBytesWithoutOverflow = 9;
        for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
        {
            // ReadByte handles end of stream cases for us.
            byteReadJustNow = await ReadByte();
            result |= (byteReadJustNow & 0x7Ful) << shift;

            if (byteReadJustNow <= 0x7Fu)
                return (long)result; // early exit
        }

        // Read the 10th byte. Since we already read 63 bits,
        // the value of this byte must fit within 1 bit (64 - 63),
        // and it must not have the high bit set.

        byteReadJustNow = await ReadByte();
        if (byteReadJustNow > 0b_1u)
            throw new FormatException("Invalid string length");

        result |= (ulong)byteReadJustNow << MaxBytesWithoutOverflow * 7;
        return (long)result;
    }

    public async Task<string> ReadString(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var length = await ReadUInt32(cancellationToken);
        var bytes = await ReadBytes(length, cancellationToken);
        return _encoding.GetString(bytes);
    }

    public async Task<byte[]> ReadBytes(uint count, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (count == 0)
            return [];
        var buffer = new byte[count];
        var bytesRead = await _stream.ReadAsync(buffer.AsMemory(0, (int)count), cancellationToken);
        if (bytesRead < count)
            throw new EndOfStreamException();
        return buffer;
    }

    public AsyncBinaryReader(Stream input, Encoding encoding) : this(input, encoding, true)
    {
    }

    public AsyncBinaryReader(Stream input) : this(input, Encoding.UTF8)
    {
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && !_leaveOpen)
                _stream.Close();
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
