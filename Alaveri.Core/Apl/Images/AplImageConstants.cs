using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Apl.Images;

public static class AplImageConstants
{
    public static readonly byte[] AplImageIdentifier = Encoding.ASCII.GetBytes("APLIMG");

    public const byte AplImageMajorVersion = 1;

    public const byte AplImageMinorVersion = 0;
}
