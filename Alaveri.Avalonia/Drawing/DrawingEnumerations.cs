using Alaveri.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Avalonia.Drawing;

public enum ColorComponent
{
    [EnumDescriptor("Hue")]
    Hue,
    [EnumDescriptor("Saturation")]
    Saturation,
    [EnumDescriptor("Lightness")]
    Lightness,
    [EnumDescriptor("HsvValue")]
    Value,
    [EnumDescriptor("Red")]
    Red,
    [EnumDescriptor("Green")]
    Green,
    [EnumDescriptor("Blue")]
    Blue,
    [EnumDescriptor("Alpha")]
    RgbAlpha,
    [EnumDescriptor("Alpha")]
    HslAlpha,
    [EnumDescriptor("Alpha")]
    HsvAlpha
}

public enum ColorModel
{
    [EnumDescriptor("RGB")]
    Rgb,
    [EnumDescriptor("HSL")]
    Hsl,
    [EnumDescriptor("HSV")]
    Hsv
}