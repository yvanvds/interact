using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  public class UnknownAddressEventArgs : EventArgs
  {
    public bool Retry { get; set; }

    public readonly object Sender;

    public readonly string Address;

    public UnknownAddressEventArgs(object sender, string address)
    {
      Retry = false;

      Sender = sender;

      Address = address;
    }
  }
}
