using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InteractServer.Implementation.UI
{
  public class Title : Interact.UI.Text
  {
    private System.Windows.Controls.TextBlock UIObject = new System.Windows.Controls.TextBlock();
    private Color backgroundColor;
    private Color textColor;

    public Title()
    {
      UIObject.Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigTitle.Background);
      UIObject.Foreground = new SolidColorBrush(Global.ProjectManager.Current.ConfigTitle.Foreground);
      UIObject.FontSize = Global.ProjectManager.Current.ConfigTitle.FontSize;

      UIObject.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
      UIObject.VerticalAlignment = System.Windows.VerticalAlignment.Center;
    }

    public override string Content { get => UIObject.Text; set => UIObject.Text = value; }
    public override double FontSize { get => UIObject.FontSize; set => UIObject.FontSize = value; }

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
