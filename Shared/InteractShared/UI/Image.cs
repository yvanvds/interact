using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Image : View
  {
    public abstract void Set(String ImageName);
    public abstract bool Visible { get; set; }
  }
}
