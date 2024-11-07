using Alaveri.Core;
using Alaveri.Core.Imaging;
using System.Runtime.InteropServices;
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

    public static async Task<IAplPalette> LoadFromStreamAsync(Stream stream, int paletteSize, CancellationToken ct = default)
    {
        var colors = new AplRgbColor[paletteSize];
        var colorBytes = new byte[3 * paletteSize];
        await stream.ReadAsync(colorBytes, ct);
        MemoryMarshal.AsBytes(colors.AsSpan());
        return new AplPalette(colors);
    }

    public AplPalette()
    {
    }

    public AplPalette(IList<AplRgbColor> colors)
    {
        Colors = colors;
    }
}
