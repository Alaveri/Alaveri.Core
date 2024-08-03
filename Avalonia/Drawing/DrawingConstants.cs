using Alaveri.Core.Enumerations;

namespace Alaveri.Avalonia.Drawing;

public static class DrawingConstants
{
    public readonly static NumberRange HueRange = new(0, 360);

    public readonly static NumberRange SaturationRange = new(0, 1);

    public readonly static NumberRange LightnessRange = new(0, 1);

    public readonly static NumberRange ValueRange = new(0, 1);

    public readonly static NumberRange RedRange = new(0, 255);

    public readonly static NumberRange GreenRange = new(0, 255);

    public readonly static NumberRange BlueRange = new(0, 255);

    public readonly static NumberRange AlphaRange = new(0, 1);

    public readonly static NumberRange RgbAlphaRange = new(0, 255);

    public readonly static NumberRange SkHueRange = new(0, 100);

    public readonly static NumberRange SkSaturationRange = new(0, 100);

    public readonly static NumberRange SkLightnessRange = new(0, 100);

    public readonly static NumberRange SkValueRange = new(0, 100);

    public readonly static IDictionary<ColorComponent, NumberRange> ColorComponentRange = new Dictionary<ColorComponent, NumberRange>
    {
        { ColorComponent.Hue, HueRange },
        { ColorComponent.Saturation, SaturationRange },
        { ColorComponent.Lightness, LightnessRange },
        { ColorComponent.Value, ValueRange },
        { ColorComponent.Red, RedRange },
        { ColorComponent.Green, GreenRange },
        { ColorComponent.Blue, BlueRange },
        { ColorComponent.RgbAlpha, RgbAlphaRange },
        { ColorComponent.HslAlpha, AlphaRange },
        { ColorComponent.HsvAlpha, AlphaRange }
    };
}
