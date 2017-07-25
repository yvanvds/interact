using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc.Values
{
  public class OscFloat : IOscValue<float>
  {
    public float Contents { get; }
    public char TypeTag { get { return 'f'; } }
    public byte[] Bytes { get; }

    public OscFloat(float contents)
    {
      Contents = contents;
      Bytes = GetBytes();
    }

    private byte[] GetBytes()
    {
      byte[] bytes = BitConverter.GetBytes(Contents);

      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(bytes);
      }
      return bytes;
    }

    public static OscFloat Parse(BinaryReader reader)
    {
      byte[] floatBytes = reader.ReadBytes(sizeof(float));

      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(floatBytes);
      }

      float value = BitConverter.ToSingle(floatBytes, 0);
      return new OscFloat(value);
    }

    public int GetByteLength()
    {
      return sizeof(float);
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
