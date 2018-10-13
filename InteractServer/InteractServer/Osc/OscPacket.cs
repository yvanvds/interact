using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc
{
  public abstract class OscPacket
  {
    public abstract byte[] Bytes { get; }

    public static OscPacket Parse(byte[] bytes)
    {
      MemoryStream stream = new MemoryStream(bytes);
      BinaryReader reader = new BinaryReader(stream);
      return Parse(reader);
    }

    public static OscPacket Parse(BinaryReader reader)
    {
      if (reader.PeekChar() == '#')
      {
        // OSC Bundle
        return OscBundle.Parse(reader);
      }
      else
      {
        return OscMessage.Parse(reader);
      }
    }
  }

}
