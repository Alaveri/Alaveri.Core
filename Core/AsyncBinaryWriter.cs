using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core;

public class AsyncBinaryWriter : IDisposable, IAsyncDisposable
{
    protected Stream OutStream { get; set; }
    private readonly Encoding _encoding;
    private readonly bool _leaveOpen;


    public virtual async Task Flush(CancellationToken cancellationToken = default)
    {
        await OutStream.FlushAsync(cancellationToken);
    }

    // Returns the stream associated with the writer. It flushes all pending
    // writes before returning. All subclasses should override Flush to
    // ensure that all buffered data is sent to the stream.
    public virtual async Task<Stream> GetBaseStream(CancellationToken cancellationToken)
    {
        await Flush(cancellationToken);
        return OutStream;
    }

    public virtual long Seek(int offset, SeekOrigin origin)
    {
        return OutStream.Seek(offset, origin);
    }

    // Writes a boolean to this stream. A single byte is written to the stream
    // with the value 0 representing false or the value 1 representing true.
    //
    public virtual async Task Write(bool value, CancellationToken cancellationToken = default) => await Write(value ? (byte)1 : (byte)0, cancellationToken);

    // Writes a byte to this stream. The current position of the stream is
    // advanced by one.
    //
    public virtual async Task Write(byte value, CancellationToken cancellationToken = default) => await OutStream.WriteAsync(new byte[] { value }, cancellationToken);

    public virtual async Task Write(sbyte value, CancellationToken cancellationToken = default) => await Write((byte)value, cancellationToken);

    public virtual async Task Write(byte[] buffer, CancellationToken cancellationToken = default) => await OutStream.WriteAsync(buffer, cancellationToken);

    public virtual async Task Write(byte[] buffer, int index, int count, CancellationToken cancellationToken = default) => await OutStream.WriteAsync(buffer.AsMemory(index, count), cancellationToken);

    public virtual async Task Write(short value, CancellationToken cancellationToken = default) => await Write(BitConverter.GetBytes(value), cancellationToken);

    public virtual async Task Write(ushort value, CancellationToken cancellationToken = default) => await Write(BitConverter.GetBytes(value), cancellationToken);

    public virtual async Task Write(long value, CancellationToken cancellationToken = default) => await OutStream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);

    public virtual async Task Write(ulong value, CancellationToken cancellationToken = default) => await OutStream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);

    public virtual async Task Write(int value, CancellationToken cancellationToken = default) => await OutStream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);

    public virtual async Task Write(uint value, CancellationToken cancellationToken = default) => await OutStream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);

    public virtual async Task Write(string value, CancellationToken cancellationToken = default)
    {
        var bytes = _encoding.GetBytes(value);
        await Write((uint)bytes.Length, cancellationToken);
        await Write(bytes, cancellationToken);
    }

    public AsyncBinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(encoding);

        if (!output.CanWrite)
            throw new ArgumentException($"Stream is not writeable {output}");

        _encoding = encoding;
        _leaveOpen = leaveOpen;
        OutStream = output;
    }

    public AsyncBinaryWriter(Stream output) : this(output, Encoding.UTF8, true)
    {
    }

    public AsyncBinaryWriter(Stream output, bool leaveOpen) : this(output, Encoding.UTF8, leaveOpen)
    {
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_leaveOpen)
                OutStream.Flush();
            else
                OutStream.Close();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual ValueTask DisposeAsync()
    {
        try
        {
            if (GetType() == typeof(AsyncBinaryWriter))
            {
                if (_leaveOpen)
                    return new ValueTask(OutStream.FlushAsync());

                OutStream.Close();
            }
            else
            {
                // Since this is a derived BinaryWriter, delegate to whatever logic
                // the derived implementation already has in Dispose.
                Dispose();
            }
            GC.SuppressFinalize(this);

            return default;
        }
        catch (Exception ex)
        {
            return ValueTask.FromException(ex);
        }
    }

}
