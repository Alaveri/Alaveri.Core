using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Apl.Images;
public abstract class AplImageConverter<TBitmap> : IAplImageConverter<TBitmap>
     where TBitmap : class
{
    public abstract IAplImage ConvertFromNative(TBitmap bitmap, bool includePalette);
}
