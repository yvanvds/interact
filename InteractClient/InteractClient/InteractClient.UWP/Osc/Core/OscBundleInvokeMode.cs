using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  /// <summary>
	/// Flags to define when bundles are to be invoked 
	/// </summary>
	[Flags]
  public enum OscBundleInvokeMode : int
  {
    /// <summary>
    /// Bundles should never be invoked
    /// </summary>
    NeverInvoke = 0,

    /// <summary>
    /// Invoke bundles that arrived within the current frame 
    /// </summary>
    InvokeOnTimeBundles = 1,

    /// <summary>
    /// Invoke bundles that arrive late immediately
    /// </summary>
    InvokeLateBundlesImmediately = 2,

    /// <summary>
    /// Pospone the ivokation of bundles that arrive early 
    /// </summary>
    PosponeEarlyBundles = 4,

    /// <summary>
    /// Invoke bundles that arrive early immediately
    /// </summary>
    InvokeEarlyBundlesImmediately = 12,

    /// <summary>
    /// Invoke all bundles immediately. Equivilent of InvokeOnTimeBundles | InvokeLateBundlesImmediately | InvokeEarlyBundlesImmediately
    /// </summary>
    InvokeAllBundlesImmediately = InvokeOnTimeBundles | InvokeLateBundlesImmediately | InvokeEarlyBundlesImmediately,
  }
}
