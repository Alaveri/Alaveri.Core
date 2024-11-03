using Alaveri.Core.Extensions;

namespace Alaveri.Avalonia.Drawing;

public readonly struct NumberRange(double min, double max)
{
    public double Min { get; } = min;

    public double Max { get; } = max;

    public readonly int IntMin => Math.Round(Min).AsInt32();

    public readonly int IntMax => Math.Round(Max).AsInt32();
}
