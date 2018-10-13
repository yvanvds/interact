using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc.Values
{
  public class OscInt : IOscValue<int>
  {
    public int Contents { get; }
    public char TypeTag { get { return 'i'; } }
    public byte[] Bytes { get; }

    public OscInt(int contents)
    {
      Contents = contents;
      Bytes = GetBytes();
    }

    private byte[] GetBytes()
    {
      return GetBigEndianIntBytes(Contents);
    }

    public static byte[] GetBigEndianIntBytes(int integer)
    {
      byte[] bytes = BitConverter.GetBytes(integer);
      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(bytes);
      }
      return bytes;
    }

    public static OscInt Parse(BinaryReader reader)
    {
      byte[] intBytes = reader.ReadBytes(sizeof(Int32));
      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(intBytes);
      }
      int value = BitConverter.ToInt32(intBytes, 0);
      return new OscInt(value);
    }

    public object GetValue()
    {
      return Contents;
    }

    public override string ToString()
    {
      return Contents.ToString();
    }
  }

}
