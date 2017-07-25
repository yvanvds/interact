using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc.Values
{
  public class OscBlob : IOscValue<byte[]>
  {
    public static int PaddingLength = 4;

    public byte[] Contents { get; }

    public char TypeTag => 'b';

    public byte[] Bytes { get; }

    public OscBlob(byte[] contents)
    {
      Contents = contents;
      Bytes = GetBytes();
    }

    private byte[] GetBytes()
    {
      byte[] result = new byte[GetByteLength()];
      Array.Copy(BitConverter.GetBytes(Contents.Length), result, sizeof(int));
      Array.Copy(Contents, 0, result, sizeof(Int32), Contents.Length);
      return result;
    }

    public static OscBlob Parse(BinaryReader reader)
    {
      int size = reader.ReadInt32();
      byte[] blobBytes = reader.ReadBytes(size);
      return new OscBlob(blobBytes);
    }

    public int GetByteLength()
    {
      return GetPaddedLength(Contents.Length);
    }

    public static int GetPaddedLength(int length)
    {
      int terminatedLength = length + 1;
      int paddingRequired = PaddingLength - (terminatedLength % PaddingLength);
      return length + paddingRequired;
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
