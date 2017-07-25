using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc.Values
{
  public class OscImpulse : IOscValue
  {
    public OscImpulse()
    {
      Bytes = new byte[0];
    }

    public byte[] Bytes { get; }
    public char TypeTag { get { return 'I'; } }

    public object GetValue()
    {
      return null;
    }

  }
}
