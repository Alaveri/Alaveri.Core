namespace Alaveri.Core.Apl.Images;

public interface IAplImageConverter<TBitmap>
    where TBitmap : class
{
    IAplImage ConvertFromNative(TBitmap bitmap, bool includePalette);
}