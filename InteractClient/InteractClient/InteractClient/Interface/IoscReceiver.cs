using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Interface
{
  public interface IoscReceiver
  {
    void Init(int port);

    bool Start();

    void Stop();

    bool TryReceive(out Rug.Osc.OscMessage message);

    Rug.Osc.OscSocketState State { get; }
  }
}
