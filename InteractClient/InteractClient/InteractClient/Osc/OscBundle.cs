using InteractClient.Osc.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc
{
  public class OscBundle : OscPacket
  {
    private const int BUNDLE_STRING_SIZE = 8;
    private const int TIME_TAG_SIZE = 8;
    private const int MESSAGE_SIZE_SIZE = 4;

    public OscTimeTag TimeTag { get; }
    public List<OscPacket> Contents { get; }
    public override byte[] Bytes { get; }

    public OscBundle(DateTime time, IEnumerable<OscPacket> contents)
    {
      Contents = new List<OscPacket>(contents);

      Bytes = new byte[GetByteLength()];
      MemoryStream stream = new MemoryStream(Bytes);
      OscString bundleString = new OscString("#bundle");
      stream.Write(bundleString.Bytes, 0, bundleString.Bytes.Length);
      TimeTag = new OscTimeTag(time);
      stream.Write(TimeTag.Bytes, 0, TimeTag.Bytes.Length);

      foreach (OscPacket message in contents)
      {
        stream.Write(OscInt.GetBigEndianIntBytes(message.Bytes.Length), 0, sizeof(int));
        stream.Write(message.Bytes, 0, message.Bytes.Length);
      }
    }

    public OscBundle(DateTime time, params OscPacket[] contents) : this(time, contents as IEnumerable<OscPacket>)
    { }

    public OscBundle(params OscPacket[] contents) : this(DateTime.Now, contents as IEnumerable<OscPacket>)
    { }

    private int GetByteLength()
    {
      return BUNDLE_STRING_SIZE + TIME_TAG_SIZE + (Contents.Count() * MESSAGE_SIZE_SIZE) + GetMessagesBytesLength();
    }

    /// <summary>
    /// This is only the content length and DOES NOT INCLUDE the size component
    /// </summary>
    /// <returns></returns>
    private int GetMessagesBytesLength()
    {
      int length = 0;
      foreach (OscPacket message in Contents)
      {
        length += message.Bytes.Length;
      }
      return length;
    }

    public static new OscBundle Parse(BinaryReader reader)
    {
      OscString bundleString = OscString.Parse(reader);
      OscTimeTag timeTag = OscTimeTag.Parse(reader);

      List<OscPacket> contents = new List<OscPacket>();
      while (reader.BaseStream.Position < reader.BaseStream.Length)
      {
        OscInt size = OscInt.Parse(reader);
        OscPacket packet = OscPacket.Parse(reader);
        contents.Add(packet);
      }

      return new OscBundle(timeTag.Contents, contents);
    }
  }

}
