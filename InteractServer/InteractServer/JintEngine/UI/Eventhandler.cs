using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.JintEngine.UI
{
  

  public class Eventhandler
  {
    class Handler
    {
      public string name;
      public object[] arguments;
    }

    private Dictionary<String, Handler> Objects = new Dictionary<string, Handler>();

    public void Register(String ID, String name, params object[] Arguments)
    {
      Objects.Add(ID, new Handler()
      {
        name = name,
        arguments = Arguments
      });
    }

    public void Clear()
    {
      Objects.Clear();
    }

    public void OnClick(object sender, EventArgs e)
    {
      System.Windows.Controls.Control control = sender as System.Windows.Controls.Control;
      JintEngine.Runner.Engine.InvokeMethod(Objects[control.Uid].name, Objects[control.Uid].arguments);
    }
  }
}
