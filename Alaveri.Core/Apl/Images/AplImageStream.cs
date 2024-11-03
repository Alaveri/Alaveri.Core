using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Apl.Images;

public class AplImageStream(byte[] buffer, byte planes) : Stream
{
    private long position = 0;

    public byte Planes { get; set; } = planes;

    protected byte[] Buffer { get; } = buffer;

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => true;

    public override long Length => Buffer.Length;

    public override long Position { get => position; set => Seek(0, SeekOrigin.Begin); }

    public override void Flush()
    {        
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int total = 0;
        while (count > 0)
        {
            if (position >= Buffer.Length)
                break;
            long readPosition = position * Planes;
            buffer[readPosition] = Buffer[position++];
            total++;
            count--;
        }
        return total;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => position + offset,
            SeekOrigin.End => Buffer.Length - offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
        };
        return position;
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        while (count > 0)
        {
            if (position >= Buffer.Length)
                break;
            long writePosition = position % Planes;
            Buffer[writePosition] = buffer[offset];
            position++;
            count--;
        }
    }
}
