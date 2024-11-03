using Alaveri.Core.Drawing;
using SkiaSharp;

namespace Alaveri.Core.Drawing.Skia;

public class SkiaLinearGradient(SKShader shader) : IGradient
{
    public SKShader Shader { get; } = shader;
}
