using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  public enum OscPacketInvokeAction
  {
    Invoke,
    DontInvoke,
    HasError,
    Pospone,
  }
}
