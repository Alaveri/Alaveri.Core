using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public abstract class PixelMapWriter(Stream stream) : IPixelMapWriter
{
    public Stream Stream { get; } = stream;

    public abstract Task WriteAsync(IPixelMap pixelMap, CancellationToken ct = default);
}
