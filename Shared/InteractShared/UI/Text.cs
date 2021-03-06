﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Text : View
  {
    public abstract string Content { get; set; }
    public abstract Color TextColor { get; set; }
    public abstract double FontSize { get; set; }
  }
}
