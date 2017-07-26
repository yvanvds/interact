using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InteractServer.Implementation.UI
{
  public class Text : Interact.UI.Text
  {
    private System.Windows.Controls.TextBlock UIObject = new System.Windows.Controls.TextBlock();
    private Color backgroundColor;
    private Color textColor;

    public Text()
    {
      UIObject.Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigText.Background);
      UIObject.Foreground = new SolidColorBrush(Global.ProjectManager.Current.ConfigText.Foreground);
      UIObject.FontSize = Global.ProjectManager.Current.ConfigText.FontSize;

      UIObject.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
      UIObject.VerticalAlignment = System.Windows.VerticalAlignment.Center;
    }

    public override string Content { get => UIObject.Text; set => UIObject.Text = value; }

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
  }
}
