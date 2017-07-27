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

    private Dictionary<String, Handler> OnClickObjects = new Dictionary<string, Handler>();
    private Dictionary<String, Handler> OnReleaseObjects = new Dictionary<string, Handler>();
    private Dictionary<String, Handler> OnChangedObjects = new Dictionary<string, Handler>();

    public void RegisterClick(String ID, String name, params object[] Arguments)
    {
      OnClickObjects.Add(ID, new Handler()
      {
        name = name,
        arguments = Arguments
      });
    }

    public void RegisterRelease(String ID, String name, params object[] Arguments)
    {
      OnReleaseObjects.Add(ID, new Handler()
      {
        name = name,
        arguments = Arguments
      });
    }

    public void RegisterChanged(String ID, String name, params object[] Arguments)
    {
      OnChangedObjects.Add(ID, new Handler()
      {
        name = name,
        arguments = Arguments
      });
    }

    public void Clear()
    {
      OnClickObjects.Clear();
      OnReleaseObjects.Clear();
      OnChangedObjects.Clear();
    }

    public void OnClick(object sender, EventArgs e)
    {
      System.Windows.Controls.Control control = sender as System.Windows.Controls.Control;
      JintEngine.Runner.Engine.InvokeMethod(OnClickObjects[control.Uid].name, OnClickObjects[control.Uid].arguments);
    }

    public void OnRelease(object sender, EventArgs e)
    {
      System.Windows.Controls.Control control = sender as System.Windows.Controls.Control;
      JintEngine.Runner.Engine.InvokeMethod(OnReleaseObjects[control.Uid].name, OnClickObjects[control.Uid].arguments);
    }

    public void OnValueChanged(object sender, EventArgs e)
    {
      System.Windows.Controls.Control control = sender as System.Windows.Controls.Control;
      JintEngine.Runner.Engine.InvokeMethod(OnChangedObjects[control.Uid].name, OnClickObjects[control.Uid].arguments);
    }
  }
}
