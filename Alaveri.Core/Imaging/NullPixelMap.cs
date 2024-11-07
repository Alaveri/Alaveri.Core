using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public class NullPixelMap : PixelMap
{
    public NullPixelMap() : base(0, 0, 0)
    {
    }
}
