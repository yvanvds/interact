using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc.Values
{
  public class OscString : IOscValue<string>
  {
    static int PaddingLength = 4;

    public string Contents { get; }
    public char TypeTag { get { return 's'; } }
    public byte[] Bytes { get; }

    public OscString(string contents)
    {
      Contents = contents;
      Bytes = GetBytes();
    }

    public byte[] GetBytes()
    {
      byte[] bytes = new byte[GetByteLength()];
      Encoding.UTF8.GetBytes(Contents, 0, Contents.Length, bytes, 0);
      return bytes;
    }

    public int GetByteLength()
    {
      return GetPaddedLength(Contents.Length);
    }

    public static int GetPaddedLength(int length)
    {
      int terminatedLength = length + 1;
      int paddedLength = (int)(Math.Ceiling(terminatedLength / (float)PaddingLength) * 4);
      return paddedLength;
    }

    public static OscString Parse(BinaryReader reader)
    {
      List<byte> bytes = new List<byte>();
      byte current = reader.ReadByte();
      while (current != 0)
      {
        bytes.Add(current);
        current = reader.ReadByte();
      }
      string str = Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.ToArray().Length);
      OscString oscString = new OscString(str);
      int bytesToBurn = oscString.Bytes.Length - bytes.Count - 1;
      for (int i = 0; i < bytesToBurn; i++)
      {
        reader.ReadByte();
      }
      return oscString;
    }

    public object GetValue()
    {
      return Contents;
    }

    public override string ToString()
    {
      return Contents;
    }
  }

}
