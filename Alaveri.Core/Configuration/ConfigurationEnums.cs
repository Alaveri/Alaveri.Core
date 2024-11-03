using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaveri.Core.Configuration;

public enum GenericWindowState
{
    Normal,
    Minimized,
    Maximized
}

/// <summary>
/// The type of configuration.  Determines where the configuration will be stored, either in %programdata% or %appdata%.
/// </summary>
public enum ConfigurationType
{
    /// <summary>
    /// Store the configuration in the %programdata% folder.
    /// </summary>
    Application,

    /// <summary>
    /// Store the configuration in the user's %appdata% folder.
    /// </summary>
    User
}


