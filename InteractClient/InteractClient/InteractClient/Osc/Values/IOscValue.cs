using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc.Values
{
  public interface IOscValue
  {
    char TypeTag { get; }
    byte[] Bytes { get; }
    object GetValue();
  }

  public interface IOscValue<T> : IOscValue
  {
    T Contents { get; }
  }
}
