using Alaveri.Core;
using System.Text;

namespace Alaveri.Core.Apl.Images;

public class AplPalette : IAplPalette
{
    public IList<AplRgbColor> Colors { get; private set; } = [];

    public void SaveToStream(Stream stream)
    {
        using var writer = new BinaryWriter(stream, Encoding.ASCII, true);
        writer.Write((ushort)Colors.Count);
        foreach (var color in Colors)
        {
            writer.Write(color.Red);
            writer.Write(color.Green);
            writer.Write(color.Blue);
        }
    }

    public static async Task<IAplPalette> LoadFromStream(Stream stream, CancellationToken ct = default)
    {
        var palette = new AplPalette();
        using var reader = new AsyncBinaryReader(stream, Encoding.ASCII, true);
        var count = await reader.ReadUInt16(ct);
        for (var index = 0; index < count; index++)
        {
            palette.Colors.Add(new AplRgbColor
            {
                Red = await reader.ReadByte(ct),
                Green = await reader.ReadByte(ct),
                Blue = await reader.ReadByte(ct)
            });
        }
        return palette;
    }
}
