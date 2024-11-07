using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public interface IPixelMapWriter
{
    public Task WriteAsync(IPixelMap pixelMap, CancellationToken ct = default);
}
