using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Extensions;

public static class RangeExtensions
{
    public static IEnumerable<int> ToEnumerable(this Range range)
    {
        if (range.Start.Value < range.End.Value)
        {
            for (var index = range.Start.Value; index <= range.End.Value; index++)
                yield return index;
        }
        else
        {
            for (var index = range.Start.Value; index >= range.End.Value; index--)
                yield return index;
        }
    }
}
