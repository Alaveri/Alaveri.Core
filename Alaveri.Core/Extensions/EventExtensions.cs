using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Extensions;

public static class EventExtensions
{
    public static void AsyncInvoke<TEventArgs>(this MulticastDelegate? multicastDelegate, object sender, TEventArgs e)
      where TEventArgs : EventArgs
    {
        multicastDelegate?.GetInvocationList()?.ToList()?.ForEach(del =>
        {
            if (del.Target is ISynchronizeInvoke synchronizeInvoke && synchronizeInvoke.InvokeRequired)
                synchronizeInvoke.EndInvoke(synchronizeInvoke.BeginInvoke(del, [sender, e]));
            else
                del.DynamicInvoke([sender, e]);
        });

        return;
    }
}
