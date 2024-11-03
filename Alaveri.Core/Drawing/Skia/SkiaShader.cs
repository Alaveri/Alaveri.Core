using Alaveri.Core.Drawing;
using SkiaSharp;

namespace Alaveri.Core.Drawing.Skia;

public class SkiaShader(SKShader? shader) : IShader
{
    public SKShader? SKShader { get; } = shader;

    public object? Shader => SKShader;
}