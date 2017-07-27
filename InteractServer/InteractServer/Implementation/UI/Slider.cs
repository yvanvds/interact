using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;
using System.Windows.Media;

namespace InteractServer.Implementation.UI
{
  public class Slider : Interact.UI.Slider
  {
    private System.Windows.Controls.Slider UIObject = new System.Windows.Controls.Slider();
    private Color backgroundColor;

    public Slider()
    {
      UIObject.Uid = Guid.NewGuid().ToString();
    }

    public override double Minimum { get => UIObject.Minimum; set => UIObject.Minimum = value; }
    public override double Maximum { get => UIObject.Maximum; set => UIObject.Maximum = value; }
    public override double Value { get => UIObject.Value; set => UIObject.Value = value; }

    public override Interact.UI.Color BackgroundColor
    {
      set
      {
        UIObject.Background = new SolidColorBrush((System.Windows.Media.Color)value.InternalObject);
        backgroundColor = new Color((System.Windows.Media.Color)value.InternalObject);
      }
      get => backgroundColor;
    }

    public override object InternalObject => UIObject;

    public override void OnChange(string functionName, params object[] arguments)
    {
      JintEngine.Runner.EventHandler.RegisterChanged(UIObject.Uid, functionName, arguments);
      UIObject.MouseDoubleClick += JintEngine.Runner.EventHandler.OnValueChanged;
    }
  }
}
