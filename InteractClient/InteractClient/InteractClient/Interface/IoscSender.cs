using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Interface
{
  public interface IoscSender
  {
    string BaseAddress { get; set; }

    void Init(string address, int port);

    void Send(string address, params object[] arguments);
  }


}
