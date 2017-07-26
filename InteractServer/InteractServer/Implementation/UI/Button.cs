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
    private Color backgroundColor;
    private Color textColor;

    public Button()
    {
      UIObject.Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigButton.Background);
      UIObject.Foreground = new SolidColorBrush(Global.ProjectManager.Current.ConfigButton.Foreground);
      UIObject.Uid = Guid.NewGuid().ToString();
    }

    public override string Content { get => UIObject.Content as string; set => UIObject.Content = value; }

    public override object InternalObject => UIObject;

    public override Interact.UI.Color TextColor
    {
      set
      {
        UIObject.Foreground = new SolidColorBrush((System.Windows.Media.Color)value.InternalObject);
        textColor = new Color((System.Windows.Media.Color)value.InternalObject);
      }
      get => textColor;
    }

    public override Interact.UI.Color BackgroundColor
    {
      set
      {
        UIObject.Background = new SolidColorBrush((System.Windows.Media.Color)value.InternalObject);
        backgroundColor = new Color((System.Windows.Media.Color)value.InternalObject);
      }
      get => backgroundColor;
    }

    public override void OnClick(string functionName, params object[] arguments)
    {
      JintEngine.Runner.EventHandler.Register(UIObject.Uid, functionName, arguments);
      UIObject.Click += JintEngine.Runner.EventHandler.OnClick;
    }

    public override void OnRelease(string functionName, params object[] arguments)
    {
      throw new NotImplementedException();
    }
  }
}
