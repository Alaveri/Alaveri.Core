using Alaveri.Core.Drawing;
using SkiaSharp;

namespace Alaveri.Core.Drawing.Skia;

public class SkiaMaskFilter(SKMaskFilter? maskFilter) : IMaskFilter
{
    protected SKMaskFilter? SKMaskFilter { get; } = maskFilter;

    public object? MaskFilter => SKMaskFilter;
}
