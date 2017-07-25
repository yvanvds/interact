using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  /// <summary>
	/// Represents a 32bit ARGB color 
	/// </summary>
	/// <remarks>
	/// This is a poor replacement for System.Drawing.Color but unfortunatly many platforms do not support 
	/// the System.Drawing namespace. 
	/// </remarks>
	public struct OscColor
  {
    private const int AlphaMask = 0x18;
    private const int RedMask = 0x10;
    private const int GreenMask = 0x08;
    private const int BlueMask = 0;

    private int m_Value;

    /// <summary>
    /// Alpha, red, green and blue components packed into a single 32bit int
    /// </summary>
    public int ARGB { get { return m_Value; } }

    /// <summary>
    /// Red component
    /// </summary>
    public byte R
    {
      get
      {
        return (byte)((this.m_Value >> RedMask) & 0xff);
      }
    }

    /// <summary>
    /// Green component
    /// </summary>
    public byte G
    {
      get
      {
        return (byte)((this.m_Value >> GreenMask) & 0xff);
      }
    }

    /// <summary>
    /// Blue component
    /// </summary>
    public byte B
    {
      get
      {
        return (byte)(this.m_Value & 0xff);
      }
    }

    /// <summary>
    /// Alpha component
    /// </summary>
    public byte A
    {
      get
      {
        return (byte)((this.m_Value >> AlphaMask) & 0xff);
      }
    }

    /// <summary>
    /// Initate a new Osc-Color from an ARGB color value
    /// </summary>
    /// <param name="value">An 32bit ARGB integer</param>
    public OscColor(int value)
    {
      m_Value = value;
    }

    public override bool Equals(object obj)
    {
      if (obj is OscColor)
      {
        return ((OscColor)obj).ARGB == ARGB;
      }
      else if (obj is int)
      {
        return ((int)obj) == ARGB;
      }
      else if (obj is uint)
      {
        return unchecked((int)(uint)obj) == ARGB;
      }
      else
      {
        return base.Equals(obj);
      }
    }

    public override string ToString()
    {
      return String.Format("{0}, {1}, {2}, {3}", A, R, G, B);
    }

    public override int GetHashCode()
    {
      return ARGB;
    }

    /// <summary>
    /// Create a Osc-Color from an 32bit ARGB integer
    /// </summary>
    /// <param name="argb">An ARGB integer</param>
    /// <returns>An Osc Color</returns>
    public static OscColor FromArgb(int argb)
    {
      return new OscColor(unchecked(argb & ((int)0xffffffff)));
    }

    /// <summary>
    /// Create a Osc-Color from 4 channels 
    /// </summary>
    /// <param name="alpha">Alpha channel component</param>
    /// <param name="red">Red channel component</param>
    /// <param name="green">Green channel component</param>
    /// <param name="blue">Blue channel component</param>
    /// <returns>An Osc Color</returns>
    public static OscColor FromArgb(int alpha, int red, int green, int blue)
    {
      CheckByte(alpha, "alpha");
      CheckByte(red, "red");
      CheckByte(green, "green");
      CheckByte(blue, "blue");

      return new OscColor(MakeArgb((byte)alpha, (byte)red, (byte)green, (byte)blue));
    }

    private static int MakeArgb(byte alpha, byte red, byte green, byte blue)
    {
      return unchecked((int)(((uint)((((red << RedMask) | (green << GreenMask)) | blue) | (alpha << AlphaMask))) & 0xffffffff));
    }

    private static void CheckByte(int value, string name)
    {
      if ((value < 0) || (value > 0xff))
      {
        throw new ArgumentException(String.Format(Strings.OscColor_ChannelInvalidValue, new object[] { name, value, 0, 0xff }), name);
      }
    }
  }
}
