using SkiaSharp;

namespace Alaveri.Drawing.Skia;

public class SkiaMaskFilter(SKMaskFilter? maskFilter) : IMaskFilter
{
    protected SKMaskFilter? SKMaskFilter { get; } = maskFilter;

    public object? MaskFilter => SKMaskFilter;
}
