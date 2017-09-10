using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InteractServer.Models
{
  public class Image : ProjectResource
  {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public int Version { get; set; }

    public string SourcePath { get; set; }

    private byte[] imageBytes = null;

    public byte[] Content
    {
      get
      {
        return ImageToByte();
      }

      set
      {
        imageBytes = value;
        ImageObj = ByteToImage(value);
      }
    }

    [JsonIgnore]
    [Ignore]
    public System.Drawing.Image ImageObj { get; set; }


    private byte[] ImageToByte()
    {
      if (imageBytes == null) imageBytes = ImageToByte(ImageObj);
      
      return imageBytes;
    }

    private static byte[] ImageToByte(System.Drawing.Image image)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        image.Save(ms, image.RawFormat);
        byte[] imageBytes = ms.ToArray();
        return imageBytes;
      }
    }

    private static System.Drawing.Image ByteToImage(byte[] imageBytes)
    {
      MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
      ms.Write(imageBytes, 0, imageBytes.Length);
      System.Drawing.Image image = new Bitmap(ms);
      return image;
    }

    public void Reload()
    {
      if (File.Exists(SourcePath))
      {
        ImageObj.Dispose();
        imageBytes = null;
        ImageObj = new System.Drawing.Bitmap(SourcePath);
        Global.Log.AddEntry("Source " + SourcePath + " reloaded.");
      }
      else
      {
        Global.Log.AddEntry("Source " + SourcePath + " cannot be found on this system.");
      }
    }

    public void Replace(string FileName)
    {
      SourcePath = FileName;
      ImageObj.Dispose();
      imageBytes = null;
      ImageObj = new Bitmap(SourcePath);
      Global.Log.AddEntry("Source replaced with " + SourcePath);
    }

    public Image()
    {

    }

    public String Serialize()
    {
      return JsonConvert.SerializeObject(this);
    }

    public void SendToClient(string clientID)
    {
      Global.Sender.SendImage(Global.ProjectManager.Current.ProjectID(), clientID, ID, Serialize());
    }
  }
}
