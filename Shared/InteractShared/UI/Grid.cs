using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Grid : View
  {
    public abstract void Init(int Columns, int Rows, bool fillScreen = false);
    public abstract void Add(View view, int Column, int Row);
    public abstract void Remove(View view);

    public abstract Color BackgroundColor { get;  set; }
  }
}
