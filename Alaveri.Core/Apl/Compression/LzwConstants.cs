namespace Alaveri.Core.Apl.Compression;

public static class LzwConstants
{
    public static readonly byte[] SupportedBitSizes = [12, 13];

    public static readonly IDictionary<AplCompressionLevel, byte> AplLzwCompressionLevels = new Dictionary<AplCompressionLevel, byte>
    {
        { AplCompressionLevel.Low, 12 },
        { AplCompressionLevel.Medium, 12 },
        { AplCompressionLevel.High, 13 }
    };

    public const byte DefaultBitSize = 13;

    public const sbyte StartBitSize = 9;
}
