using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public enum PngColorType : byte
{
    Grayscale = 0,
    Rgb = 2,
    Palette = 3,
    GrayscaleAlpha = 4,
    Rgba = 6
}

public enum PngCompressionMethod : byte
{
    Deflate = 0
}

public enum PngFilterMethod : byte
{
    Adaptive = 0
}

public enum PngInterlaceMethod : byte
{
    None = 0,
    Adam7 = 1
}