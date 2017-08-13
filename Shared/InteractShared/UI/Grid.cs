using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Grid : View
  {
    public abstract void Init(int Columns, int Rows);
    public abstract void Init(int Columns, int Rows, bool fillScreen);
    public abstract void Add(View view, int Column, int Row);
    public abstract void AddSpan(View view, int Column, int Row, int Width, int Height);
    public abstract void Remove(View view);
  }
}
