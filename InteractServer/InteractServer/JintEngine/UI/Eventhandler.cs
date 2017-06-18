using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.JintEngine.UI
{
  public class Eventhandler
  {
    private Dictionary<String, String> Objects = new Dictionary<string, string>();

    public void Register(String ID, String name)
    {
      Objects.Add(ID, name);
    }

    public void Clear()
    {
      Objects.Clear();
    }

    public void OnClick(object sender, EventArgs e)
    {
      System.Windows.Controls.Control control = sender as System.Windows.Controls.Control;
      JintEngine.Runner.Engine.InvokeMethod(Objects[control.Uid]);
    }
  }
}
