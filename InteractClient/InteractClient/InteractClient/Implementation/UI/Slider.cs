using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;

namespace InteractClient.Implementation.UI
{
  class Slider : Interact.UI.Slider
  {
    private Xamarin.Forms.Slider UIObject = new Xamarin.Forms.Slider();
    private Color backgroundColor;



    public override double Minimum { get => UIObject.Minimum; set => UIObject.Minimum = value; }
    public override double Maximum { get => UIObject.Maximum; set => UIObject.Maximum = value; }
    public override double Value { get => UIObject.Value; set => UIObject.Value = value; }

    public override Interact.UI.Color BackgroundColor
    {
      get => backgroundColor;
      set
      {
        UIObject.BackgroundColor = (Xamarin.Forms.Color)(value.InternalObject);
        backgroundColor = new Color(UIObject.BackgroundColor);
      }
    }

    public override object InternalObject => UIObject;

    public override void OnChange(string functionName, params object[] arguments)
    {
      JintEngine.Engine.EventHandler.RegisterChanged(UIObject.Id, functionName, arguments);
      UIObject.ValueChanged += JintEngine.Engine.EventHandler.OnValueChanged;
    }
  }
}
