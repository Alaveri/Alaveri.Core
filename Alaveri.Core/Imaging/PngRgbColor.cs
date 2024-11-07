using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Imaging;

public readonly struct PngRgbColor(byte red, byte green, byte blue)
{
    public byte Red { get; } = red;
    public byte Green { get; } = green;
    public byte Blue { get; } = blue;
    public uint AsUint32 => (uint)(Red << 16 | Green << 8 | Blue);

    public PngRgbColor(uint color) : this((byte)(color >> 16), (byte)(color >> 8), (byte)color)
    {
    }
}
