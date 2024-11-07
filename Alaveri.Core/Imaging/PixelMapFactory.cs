using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public enum PixelMapFormat
{
    Png
}

public static class PixelMapFactory
{
    public static PixelMapFormat GetFormat(string filename)
    {
        var extension = Path.GetExtension(filename);
        return extension switch
        {
            ".png" => PixelMapFormat.Png,
            _ => throw new NotSupportedException($"Unsupported format: {extension}")
        };
    }

    public static IPixelMapReader CreateReader(Stream stream, PixelMapFormat format)
    {
        return format switch
        {
            PixelMapFormat.Png => new PngPixelMapReader(stream),
            _ => throw new NotSupportedException($"Unsupported format: {format}")
        };
    }

    public static IPixelMapWriter CreateWriter(Stream stream, PixelMapFormat format)
    {
        return format switch
        {
            PixelMapFormat.Png => new PngPixelMapWriter(stream),
            _ => throw new NotSupportedException($"Unsupported format: {format}")
        };
    }
}
