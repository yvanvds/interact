using InteractServer.Osc.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc
{
  public class OscMessage : OscPacket
  {
    public OscString Address { get; set; }
    public List<IOscValue> Arguments { get; set; }
    public override byte[] Bytes { get; }

    public OscMessage(string address, params object[] values)
    {
      Address = new OscString(address);
      Arguments = new List<IOscValue>();
      foreach (object obj in values)
      {
        if (obj is IOscValue)
        {
          Arguments.Add(obj as IOscValue);
        }
        else
        {
          Arguments.Add(OscValue.Wrap(obj));
        }
      }
      Bytes = GetBytes();
    }

    public OscMessage(string address, IEnumerable<IOscValue> values)
    {
      Address = new OscString(address);
      Arguments = new List<IOscValue>(values);
      Bytes = GetBytes();
    }

    private byte[] GetBytes()
    {
      byte[] bytes = new byte[GetByteLength()];
      MemoryStream stream = new MemoryStream(bytes);
      stream.Write(Address.Bytes, 0, Address.Bytes.Length);
      OscString typeTag = GetTypeTagString();
      stream.Write(typeTag.Bytes, 0, typeTag.Bytes.Length);
      foreach (IOscValue value in Arguments)
      {
        stream.Write(value.Bytes, 0, value.Bytes.Length);
      }
      return bytes;
    }

    public int GetByteLength()
    {
      return Address.GetByteLength() + GetTypeTagLength() + GetArgumentsLength();
    }

    private OscString GetTypeTagString()
    {
      char[] chars = new char[Arguments.Count + 1];
      chars[0] = ',';
      int index = 1;
      foreach (IOscValue value in Arguments)
      {
        chars[index] = value.TypeTag;
        index++;
      }
      return new OscString(new string(chars));
    }

    private int GetTypeTagLength()
    {
      return OscString.GetPaddedLength(Arguments.Count + 2);
    }

    private int GetArgumentsLength()
    {
      int length = 0;
      foreach (IOscValue value in Arguments)
      {
        length += value.Bytes.Length;
      }
      return length;
    }

    public static new OscMessage Parse(BinaryReader reader)
    {
      OscString address = OscString.Parse(reader);
      OscString typeTags = OscString.Parse(reader);

      int valueCount = typeTags.Contents.Length - 1;
      List<IOscValue> values = new List<IOscValue>();

      foreach (char current in typeTags.Contents.Substring(1))
      {
        IOscValue value = OscValue.Parse(current, reader);

        values.Add(value);
      }

      return new OscMessage(address.Contents, values);
    }

    public override string ToString()
    {
      StringBuilder builder = new StringBuilder();
      builder.Append(Address.ToString());
      builder.Append(" ");
      foreach (IOscValue value in Arguments)
      {
        builder.Append(value.GetValue());
        builder.Append(" ");
      }
      return builder.ToString();
    }
  }

}
