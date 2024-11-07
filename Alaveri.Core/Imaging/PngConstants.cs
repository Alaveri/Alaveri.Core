using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public static class PngConstants
{
    public static readonly byte[] Signature = [137, 80, 78, 71, 13, 10, 26, 10];

    public const string EndChunk = "IEND";
    public const string HeaderChunk = "IHDR";
    public const string PaletteChunk = "PLTE";
    public const string DataChunk = "IDAT";
    public const string TransparencyChunk = "tRNS";
    public const string ChromaticityChunk = "cHRM";
    public const string GammaChunk = "gAMA";
    public const string IccProfileChunk = "iCCP";
    public const string SignificantBitsChunk = "sBIT";
    public const string StandardRgbColorSpaceChunk = "sRGB";
    public const string BackgroundChunk = "bKGD";
    public const string HistogramChunk = "hIST";
    public const string TextChunk = "tEXt";
    public const string CompressedTextChunk = "zTXt";
    public const string TimeChunk = "tIME";
    public const string PhysicalPixelDimensionsChunk = "pHYs";
    public const string SuggestedPaletteChunk = "sPLT";
    public const string InternationalTextChunk = "iTXt";

    public static byte[] AllowedBitDepths(PngColorType pngColorType) => pngColorType switch
    {
        PngColorType.Grayscale => [1, 2, 4, 8, 16],
        PngColorType.Rgb => [8, 16],
        PngColorType.Palette => [1, 2, 4, 8],
        PngColorType.GrayscaleAlpha => [8, 16],
        PngColorType.Rgba => [8, 16],
        _ => []
    };

    public static byte BitsPerPixel(PngColorType pngColorType, byte bitDepth) => pngColorType switch
    {
        PngColorType.Grayscale => bitDepth,
        PngColorType.Rgb => (byte)(3 * bitDepth),
        PngColorType.Palette => bitDepth,
        PngColorType.GrayscaleAlpha => (byte)(2 * bitDepth),
        PngColorType.Rgba => (byte)(4 * bitDepth),
        _ => 0
    };
}
