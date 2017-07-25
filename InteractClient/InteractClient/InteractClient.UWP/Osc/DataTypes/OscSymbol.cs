using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  /// <summary>
	/// Osc symbol 
	/// </summary>
	public struct OscSymbol
  {
    /// <summary>
    /// The string value of the symbol
    /// </summary>
    public string Value;

    /// <summary>
    /// Create a new symbol
    /// </summary>
    /// <param name="value">literal string value</param>
    public OscSymbol(string value)
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value;
    }

    public override bool Equals(object obj)
    {
      if (obj is OscSymbol)
      {
        return Value.Equals(((OscSymbol)obj).Value);
      }
      else
      {
        return Value.Equals(obj);
      }
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }
  }
}
