using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{

  /// <summary>
  /// Osc Impulse Singleton
  /// </summary>
  public sealed class OscImpulse
  {
    public static readonly OscImpulse Value = new OscImpulse();

    private OscImpulse() { }

    public override string ToString()
    {
      return "bang";
    }

    /// <summary>
    /// Matches the string against "Impulse", "Bang", "Infinitum", "Inf" the comparison is StringComparison.InvariantCultureIgnoreCase
    /// </summary>
    /// <param name="str">string to check</param>
    /// <returns>true if the string matches any of the recognised impulse strings else false</returns>
    public static bool IsImpulse(string str)
    {
      bool isTrue = false;

      isTrue |= "Infinitum".Equals(str, System.StringComparison.CurrentCultureIgnoreCase);

      isTrue |= "Inf".Equals(str, System.StringComparison.CurrentCultureIgnoreCase);

      isTrue |= "Bang".Equals(str, System.StringComparison.CurrentCultureIgnoreCase);

      isTrue |= "Impulse".Equals(str, System.StringComparison.CurrentCultureIgnoreCase);

      return isTrue;
    }

  }
}
