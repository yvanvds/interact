﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc.Values
{
  public class OscTimeTag : IOscValue<DateTime>
  {
    public static DateTime BeginningOfTime = new DateTime(1900, 1, 1);
    public static long UnitsPerSecond = (long)Math.Pow(2, 32);
    public static long UnitsPerTick = UnitsPerSecond / TimeSpan.TicksPerSecond;

    public DateTime Contents { get; }
    public char TypeTag { get { return 't'; } }
    public byte[] Bytes { get; }

    public OscTimeTag(DateTime contents)
    {
      Contents = contents;
      Bytes = new byte[8];

      TimeSpan timespan = contents - BeginningOfTime;
      UInt32 seconds = (UInt32)(timespan.Ticks / TimeSpan.TicksPerSecond);
      long remainderTicks = timespan.Ticks % TimeSpan.TicksPerSecond;

      UInt32 remainder = (UInt32)(remainderTicks * UnitsPerTick);

      byte[] secondBytes = BitConverter.GetBytes(seconds);
      byte[] remainderBytes = BitConverter.GetBytes(remainder);
      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(secondBytes);
        Array.Reverse(remainderBytes);
      }
      Array.Copy(secondBytes, 0, Bytes, 0, sizeof(int));
      Array.Copy(remainderBytes, 0, Bytes, sizeof(int), sizeof(int));
    }

    public static OscTimeTag Parse(BinaryReader reader)
    {
      byte[] secondBytes = reader.ReadBytes(sizeof(UInt32));
      byte[] remainderBytes = reader.ReadBytes(sizeof(UInt32));
      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(secondBytes);
        Array.Reverse(remainderBytes);
      }

      UInt32 seconds = BitConverter.ToUInt32(secondBytes, 0);
      UInt32 secondPart = BitConverter.ToUInt32(remainderBytes, 0);

      long remainderTicks = secondPart / UnitsPerTick;

      DateTime time = BeginningOfTime + new TimeSpan(seconds * TimeSpan.TicksPerSecond) + new TimeSpan(remainderTicks);

      return new OscTimeTag(time);
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
