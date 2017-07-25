using InteractClient.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Implementation
{
  public class OscReceiverImplementation : IoscReceiver
  {
    public global::Rug.Osc.OscSocketState State => throw new NotImplementedException();

    public void Init(int port)
    {
      throw new NotImplementedException();
    }

    public bool Start()
    {
      throw new NotImplementedException();
    }

    public void Stop()
    {
      throw new NotImplementedException();
    }

    public bool TryReceive(out global::Rug.Osc.OscMessage message)
    {
      throw new NotImplementedException();
    }
  }
}
