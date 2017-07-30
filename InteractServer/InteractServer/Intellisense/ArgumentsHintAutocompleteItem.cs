using AutocompleteMenuNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Intellisense
{
  class ArgumentsHintAutocompleteItem : AutocompleteItem
  {
    List<string> parameters = new List<string>();
    int currentArg;

    public ArgumentsHintAutocompleteItem(ParameterInfo[] info, int currentArg)
      : base("")
    {
      
      foreach(var parm in info)
      {
        string entry = parm.ParameterType.Name + " " + parm.Name;
        if (parm.Position < info.Length - 1) entry += ", ";
        parameters.Add(entry);
      }

      this.currentArg = currentArg;
    }

    public override CompareResult Compare(string fragmentText)
    {
      return CompareResult.Visible;
    }

    public override void OnSelected(SelectedEventArgs e)
    {
      return;
    }

    public override void OnPaint(PaintItemEventArgs e)
    {
      var brush = new SolidBrush(e.IsSelected ? e.Colors.SelectedForeColor : e.Colors.ForeColor);

      PointF point = new PointF(0, 0);
      point = e.TextRect.Location;
      int count = 0;

      foreach (string par in parameters)
      {
        SizeF stringSize = new SizeF();
        stringSize = e.Graphics.MeasureString(par, e.Font);
        if(count == currentArg)
        {
          e.Graphics.DrawString(par, e.Font, Brushes.White, point, e.StringFormat);
        } else
        {
          e.Graphics.DrawString(par, e.Font, brush, point, e.StringFormat);
        }
        point.X += stringSize.Width;
        count++;
      }
    }
  }
}
