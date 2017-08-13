using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Interact.UI;

namespace InteractServer.Implementation.UI
{
  public class Grid : Interact.UI.Grid
  {
    private System.Windows.Controls.Grid UIObject = new System.Windows.Controls.Grid();
    private Color backgroundColor;

    public override object InternalObject { get { return UIObject; } }

    public override Interact.UI.Color BackgroundColor
    {
      set
      {
        UIObject.Background = new SolidColorBrush((System.Windows.Media.Color)value.InternalObject);
        backgroundColor = new Color((System.Windows.Media.Color)value.InternalObject);
      }
      get
      {
        return backgroundColor;
      }
    }

    

    public Grid()
    {
      UIObject.Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigPage.Color);
      backgroundColor = new Color(Global.ProjectManager.Current.ConfigPage.Color);
    }

    public override void Init(int Columns, int Rows)
    {
      Init(Columns, Rows, true);
    }

    public override void Init(int Columns, int Rows, bool fillScreen)
    {
      for(int i = 0; i < Columns; i++)
      {
        UIObject.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
      }

      for(int i = 0; i < Rows; i++)
      {
        UIObject.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
      }
    }

    public override void Add(Interact.UI.View view, int Column, int Row)
    {
      System.Windows.Controls.Grid.SetColumn(view.InternalObject as System.Windows.UIElement, Column);
      System.Windows.Controls.Grid.SetRow(view.InternalObject as System.Windows.UIElement, Row);
      UIObject.Children.Add(view.InternalObject as System.Windows.UIElement);
    }

    public override void Remove(Interact.UI.View view)
    {
      UIObject.Children.Remove(view.InternalObject as System.Windows.UIElement);
    }

    public override void AddSpan(View view, int Column, int Row, int Width, int Height)
    {
      System.Windows.Controls.Grid.SetColumn(view.InternalObject as System.Windows.UIElement, Column);
      System.Windows.Controls.Grid.SetRow(view.InternalObject as System.Windows.UIElement, Row);
      System.Windows.Controls.Grid.SetColumnSpan(view.InternalObject as System.Windows.UIElement, Width);
      System.Windows.Controls.Grid.SetRowSpan(view.InternalObject as System.Windows.UIElement, Height);

      UIObject.Children.Add(view.InternalObject as System.Windows.UIElement);
    }
  }
}
