using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Slider : InteractiveView
  {
    public abstract double Minimum { get; set; }
    public abstract double Maximum { get; set; }
    public abstract double Value { get; set; }

    public abstract void OnChange(string functionName, params object[] arguments);
  }
}
