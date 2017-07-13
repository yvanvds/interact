using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Button : View
  {
    public abstract string Content { get; set; }

    public abstract void OnClick(string functionName, params object[] arguments);

    public abstract void OnRelease(string functionName, params object[] arguments);

    public abstract void SetColor(Color color);
  }
}
