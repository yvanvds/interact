using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InteractServer.JintEngine.UI
{
  public class Button : System.Windows.Controls.Button
  {
    public Button()
    {
      Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigButton.Background);
      Foreground = new SolidColorBrush(Global.ProjectManager.Current.ConfigButton.Foreground);
      Uid = Guid.NewGuid().ToString();
    }

    public void OnClick(String functionName, params object[] arguments)
    {
      Runner.EventHandler.Register(Uid, functionName, arguments);
      Click += Runner.EventHandler.OnClick;
    }
  }
}
