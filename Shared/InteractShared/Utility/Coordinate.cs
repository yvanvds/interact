using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Utility
{
  public struct Coordinate
  {
    public float X;
    public float Y;

    public Coordinate(float x, float y)
    {
      X = x;
      Y = y;
    }

    public string AsText()
    {
      return "X: " + X + " Y: " + Y;
    }
  }
}
