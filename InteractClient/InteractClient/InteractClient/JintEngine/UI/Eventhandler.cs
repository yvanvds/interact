using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.JintEngine.UI
{

  public class Eventhandler
  {
    class Handler
    {
      public string name;
      public object[] arguments;
    }

    private Dictionary<Guid, Handler> OnClickObjects = new Dictionary<Guid, Handler>();
    private Dictionary<Guid, Handler> OnReleaseObjects = new Dictionary<Guid, Handler>();
    private Dictionary<Guid, Handler> OnChangedObjects = new Dictionary<Guid, Handler>();

    public void RegisterClick(Guid ID, String Name, params object[] Arguments)
    {
      OnClickObjects.Add(ID, new Handler() {
        name = Name,
        arguments = Arguments
      });
    }

    public void RegisterRelease(Guid ID, String Name, params object[] Arguments)
    {
      OnReleaseObjects.Add(ID, new Handler()
      {
        name = Name,
        arguments = Arguments
      });
    }

    public void RegisterChanged(Guid ID, string Name, params object[] Arguments)
    {
      OnChangedObjects.Add(ID, new Handler()
      {
        name = Name,
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
      Xamarin.Forms.Element element = sender as Xamarin.Forms.Element;
      
      Engine.Instance.Invoke(OnClickObjects[element.Id].name, (e as Interface.PressedEventArgs).Pressure, OnClickObjects[element.Id].arguments);
    }

    public void OnRelease(object sender, EventArgs e)
    {
      Xamarin.Forms.Element element = sender as Xamarin.Forms.Element;
      Engine.Instance.Invoke(OnReleaseObjects[element.Id].name, OnReleaseObjects[element.Id].arguments);
    }

    public void OnValueChanged(object sender, ValueChangedEventArgs e)
    {
      Xamarin.Forms.Element element = sender as Xamarin.Forms.Element;
      Engine.Instance.Invoke(OnChangedObjects[element.Id].name, OnChangedObjects[element.Id].arguments);
    }
  }
}
