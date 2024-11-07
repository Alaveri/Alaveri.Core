using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public interface IPixelMapReader
{
    Task<IPixelMap> ReadMapAsync(CancellationToken ct = default);    
}

