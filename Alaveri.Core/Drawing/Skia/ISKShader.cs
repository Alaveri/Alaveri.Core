using SkiaSharp;

namespace Alaveri.Core.Drawing.Skia
{
    public interface ISKShader
    {
        SKShader WithColorFilter(SKColorFilter filter);
        SKShader WithLocalMatrix(SKMatrix localMatrix);
    }
}