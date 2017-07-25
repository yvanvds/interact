using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  /// <summary>
	/// Osc Null Singleton
	/// </summary>
	public sealed class OscNull
  {
    public static readonly OscNull Value = new OscNull();

    private OscNull() { }

    public override string ToString()
    {
      return "null";
    }

    public static bool IsNull(string str)
    {
      bool isTrue = false;

      isTrue |= "Null".Equals(str, System.StringComparison.CurrentCultureIgnoreCase);

      isTrue |= "Nil".Equals(str, System.StringComparison.CurrentCultureIgnoreCase);

      return isTrue;
    }
  }
}
