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

    public Title()
    {
      UIObject.Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigTitle.Background);
      UIObject.Foreground = new SolidColorBrush(Global.ProjectManager.Current.ConfigTitle.Foreground);
      UIObject.FontSize = Global.ProjectManager.Current.ConfigTitle.FontSize;

      UIObject.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
      UIObject.VerticalAlignment = System.Windows.VerticalAlignment.Center;
    }

    public override string Content { get => UIObject.Text; set => UIObject.Text = value; }

    public override object InternalObject => UIObject;
  }
}
