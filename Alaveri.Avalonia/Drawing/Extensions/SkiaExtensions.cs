using Avalonia.Platform;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Avalonia.Drawing.Extensions;

public static class SkiaExtensions
{
    public static SKColorType ToSKColorType(this global::Avalonia.Platform.PixelFormat format)
    {
        if (format == global::Avalonia.Platform.PixelFormat.Rgba8888)
            return SKColorType.Rgba8888;
        if (format == global::Avalonia.Platform.PixelFormat.Bgra8888)
            return SKColorType.Bgra8888;
        if (format == global::Avalonia.Platform.PixelFormat.Rgb565)
            return SKColorType.Rgb565;
        if (format == global::Avalonia.Platform.PixelFormat.Rgb32)
            return SKColorType.RgbaF32;
        return SKColorType.Unknown;
    }
}
