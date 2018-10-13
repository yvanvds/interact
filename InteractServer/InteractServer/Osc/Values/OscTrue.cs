using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc.Values
{
  public class OscTrue : IOscValue<bool>
  {
    public OscTrue()
    {
      Bytes = new byte[0];
    }

    public byte[] Bytes { get; }
    public bool Contents { get { return true; } }
    public char TypeTag { get { return 'T'; } }

    public object GetValue()
    {
      return true;
    }

    public override string ToString()
    {
      return "True";
    }

  }

}
