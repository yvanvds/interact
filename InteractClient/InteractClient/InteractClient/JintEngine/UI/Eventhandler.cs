using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine.UI
{

  public class Eventhandler
  {
    class Handler
    {
      public string name;
      public object[] arguments;
    }

    private Dictionary<Guid, Handler> Objects = new Dictionary<Guid, Handler>();

    public void Register(Guid ID, String Name, params object[] Arguments)
    {
      Objects.Add(ID, new Handler() {
        name = Name,
        arguments = Arguments
      });
    }

    public void Clear()
    {
      Objects.Clear();
    }


    public void OnClick(object sender, EventArgs e)
    {
      Xamarin.Forms.Element element = sender as Xamarin.Forms.Element;
      Engine.Instance.Invoke(Objects[element.Id].name, Objects[element.Id].arguments);
    }
  }
}
