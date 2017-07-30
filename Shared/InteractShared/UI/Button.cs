using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Button : InteractiveView
  {
    // Appearance
    public abstract string Content { get; set; }
    public abstract Color TextColor { get; set; }

    // Interaction
    public abstract float Pressure { get; }

    public abstract void OnClick(string functionName, params object[] arguments);
    public abstract void OnRelease(string functionName, params object[] arguments);
  }
}
