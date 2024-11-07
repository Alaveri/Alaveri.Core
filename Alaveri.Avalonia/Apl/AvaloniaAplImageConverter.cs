using Alaveri.Core.Apl.Images;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Avalonia.Apl;

public class AvaloniaAplImageConverter : AplImageConverter<WriteableBitmap>
{
    public override IAplImage ConvertFromNative(WriteableBitmap bitmap, bool includePalette)
    {
        var bpp = (byte)(bitmap.Format?.BitsPerPixel ?? 32);
        var width = (ushort)bitmap.PixelSize.Width;
        var height = (ushort)bitmap.PixelSize.Height;
        var size = width * height * bpp;
        var data = new byte[size];
        using var buffer = bitmap.Lock();
        Marshal.Copy(buffer.Address, data, 0, size);
        var result = new AplImage(width, height, bpp)
        {
            Buffer = data
        };
        return result;
    }
}
