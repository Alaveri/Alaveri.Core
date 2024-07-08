using SkiaSharp;

namespace Alaveri.Drawing.Skia
{
    public interface ISKShader
    {
        SKShader WithColorFilter(SKColorFilter filter);
        SKShader WithLocalMatrix(SKMatrix localMatrix);
    }
}