using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Interact.UI;

namespace InteractServer.Implementation.UI
{
  public class Button : Interact.UI.Button
  {
    private System.Windows.Controls.Button UIObject = new System.Windows.Controls.Button();

    public Button()
    {
      UIObject.Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigButton.Background);
      UIObject.Foreground = new SolidColorBrush(Global.ProjectManager.Current.ConfigButton.Foreground);
      UIObject.Uid = Guid.NewGuid().ToString();
    }

    public override string Content { get => UIObject.Content as string; set => UIObject.Content = value; }

    public override object InternalObject => UIObject;

    public override void SetColor(Interact.UI.Color color)
    {
      UIObject.Background = new SolidColorBrush((System.Windows.Media.Color)(color.InternalObject));
    }

    public override void OnClick(string functionName, params object[] arguments)
    {
      JintEngine.Runner.EventHandler.Register(UIObject.Uid, functionName, arguments);
      UIObject.Click += JintEngine.Runner.EventHandler.OnClick;
    }
  }
}
