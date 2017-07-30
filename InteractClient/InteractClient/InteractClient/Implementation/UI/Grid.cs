using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;
using Xamarin.Forms;

namespace InteractClient.Implementation.UI
{
  public class Grid : Interact.UI.Grid
  {
    private Xamarin.Forms.Grid UIObject = new Xamarin.Forms.Grid();
    private Color backgroundColor;

    public Grid()
    {
      UIObject.BackgroundColor = Data.Project.Current.ConfigPage.Color.Get();
      backgroundColor = new Color(UIObject.BackgroundColor);
    }

    public override object InternalObject => UIObject;

    public override Interact.UI.Color BackgroundColor {
      set
      {
        UIObject.BackgroundColor = (Xamarin.Forms.Color)(value.InternalObject);
        backgroundColor = new Color(UIObject.BackgroundColor);
      }
      get
      {
        return backgroundColor;
      }
    }

    public override void Init(int Columns, int Rows, bool fillScreen = false)
    {
      UIObject.ColumnDefinitions.Clear();
      for (int i = 0; i < Columns; i++)
      {
        UIObject.ColumnDefinitions.Add(new ColumnDefinition());
      }

      UIObject.RowDefinitions.Clear();
      for (int i = 0; i < Rows; i++)
      {
        UIObject.RowDefinitions.Add(new RowDefinition());
      }
    }

    public override void Add(Interact.UI.View view, int Column, int Row)
    {
      Xamarin.Forms.Grid.SetColumn(view.InternalObject as Xamarin.Forms.View, Column);
      Xamarin.Forms.Grid.SetRow(view.InternalObject as Xamarin.Forms.View, Row);
      UIObject.Children.Add(view.InternalObject as Xamarin.Forms.View);
    }

    public override void Remove(Interact.UI.View view)
    {
      UIObject.Children.Remove(view.InternalObject as Xamarin.Forms.View);
    }
  
  }
}
