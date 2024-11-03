namespace Alaveri.Core.Apl.Compression;

public struct LzwDictionaryEntry
{
    public ushort Code { get; set; }
    public ushort Prefix { get; set; }
    public byte Character { get; set; }
}

