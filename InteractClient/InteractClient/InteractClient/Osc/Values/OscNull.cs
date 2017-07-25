using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc.Values
{
  public class OscNull : IOscValue
  {
    public OscNull()
    {
      Bytes = new byte[0];
    }

    public byte[] Bytes { get; }
    public char TypeTag { get { return 'N'; } }

    public object GetValue()
    {
      return null;
    }

    public override string ToString()
    {
      return "Null";
    }

  }

}
