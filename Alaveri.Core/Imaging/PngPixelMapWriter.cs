using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public class PngPixelMapWriter(Stream stream): PixelMapWriter(stream)
{
    public override Task WriteAsync(IPixelMap pixelMap, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}
