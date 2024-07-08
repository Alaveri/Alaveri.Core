using SkiaSharp;

namespace Alaveri.Drawing.Skia;

public class SkiaLinearGradient(SKShader shader) : IGradient
{
    public SKShader Shader { get; } = shader;
}
