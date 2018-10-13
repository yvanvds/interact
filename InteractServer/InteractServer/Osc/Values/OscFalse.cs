using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc.Values
{
  public class OscFalse : IOscValue<bool>
  {
    public OscFalse()
    {
      Bytes = new byte[0];
    }

    public byte[] Bytes { get; }
    public bool Contents { get { return false; } }
    public char TypeTag { get { return 'F'; } }

    public object GetValue()
    {
      return false;
    }

    public override string ToString()
    {
      return "False";
    }
  }
}
