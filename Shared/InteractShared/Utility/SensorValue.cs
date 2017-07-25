using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Utility
{
  public class SensorValue
  {
    public virtual double? Value { get; set; }

    public override string ToString()
    {
      return string.Format("Value = {0}", Value);
    }
  }
}
