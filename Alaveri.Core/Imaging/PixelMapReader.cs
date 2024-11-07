using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public abstract class PixelMapReader(Stream stream) : IPixelMapReader
{
    public Stream Source { get; } = stream;
    
    public abstract Task<IPixelMap> ReadMapAsync(CancellationToken ct = default);
}
