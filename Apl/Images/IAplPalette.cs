namespace Alaveri.Apl.Images;

public interface IAplPalette
{
    IList<AplRgbColor> Colors { get; }

    void SaveToStream(Stream stream);
}