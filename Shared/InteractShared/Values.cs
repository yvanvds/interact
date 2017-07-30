using System;
using System.Collections.Generic;
using System.Text;

namespace Interact
{
  public abstract class Values
  {
    public abstract void Store(string key, object data);
    public abstract object Load(string key);

    public abstract void Clear();
  }
}
