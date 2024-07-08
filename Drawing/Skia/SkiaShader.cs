using SkiaSharp;

namespace Alaveri.Drawing.Skia;

public class SkiaShader(SKShader? shader) : IShader
{
    public SKShader? SKShader { get; } = shader;

    public object? Shader => SKShader;
}