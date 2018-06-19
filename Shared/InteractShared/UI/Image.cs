using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Image : View
  {
    public enum Mode
    {
      Fill,
      Fit,
      Stretch
    }

    public abstract void Set(String ImageName, Mode mode);
    public abstract bool Visible { get; set; }
  }
}
