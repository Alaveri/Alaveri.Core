using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Apl.Compression;
public enum AplCompressionLevel
{
    Low,
    Medium,
    High
}

public enum AplCompression
{
    None,
    Lzw,
    Gif
}