using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc.Values
{
  public class Color
  {
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
    public byte Alpha { get; }

    public Color(byte red, byte green, byte blue, byte alpha)
    {
      Red = red;
      Green = green;
      Blue = blue;
      Alpha = alpha;
    }
  }


  public class OscColor : IOscValue<Color>
  {
    public OscColor(byte red, byte green, byte blue, byte alpha) : this(new Color(red, green, blue, alpha)) {}

    public OscColor(Color color)
    {
      Contents = color;
      Bytes = new byte[4] { Contents.Red, Contents.Green, Contents.Blue, Contents.Alpha };
      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(Bytes);
      }
    }

    public byte[] Bytes { get; }

    public char TypeTag { get { return 'c'; } }

    public Color Contents { get; }

    public object GetValue()
    {
      return Contents;
    }

    public static OscColor Parse(BinaryReader reader)
    {
      byte red = reader.ReadByte();
      byte green = reader.ReadByte();
      byte blue = reader.ReadByte();
      byte alpha = reader.ReadByte();
      return new OscColor(red, green, blue, alpha);
    }

  }
}
