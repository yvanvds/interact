using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Utility
{
  public class SensorVector : SensorValue
  {
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public override string ToString()
    {
      return string.Format("X={0}, Y={0}, Z={0}", X, Y, Z);
    }

    public override double? Value { get => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }
  }
}
