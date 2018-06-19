using InteractServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InteractServer.Project.Image
{
  public class Item : IDiskResource
  {
    private IDiskResource resource;

    // implements IContentModel
    public Guid ID { get => resource.ID; set => resource.ID = value; }
    public string Name => resource.Name;
    [JsonIgnore]
    public string Path { get => resource.Path; set => resource.Path = value; }
    [JsonIgnore]
    public bool Tainted { get; set; }
    [JsonIgnore]
    public string Data { get => resource.Data; set => resource.Data = value; }

    public int Version { get => resource.Version; set => resource.Version = value; }

    [JsonIgnore]
    public ContentType Type { get => resource.Type; set => resource.Type = value; }
    public void MoveTo(string path) { resource.MoveTo(path); }

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
      if (File.Exists(Path))
      {
        ImageObj.Dispose();
        imageBytes = null;
        ImageObj = new System.Drawing.Bitmap(Path);
        Global.Log.AddEntry("Source " + Path + " reloaded.");
      }
      else
      {
        Global.Log.AddEntry("Source " + Path + " cannot be found on this system.");
      }
    }

    public void Replace(string path)
    {
      Path = path;
      ImageObj.Dispose();
      imageBytes = null;
      ImageObj = new Bitmap(Path);
      Global.Log.AddEntry("Source replaced with " + Path);
    }

    public Item(IDiskResource resource)
    {
      this.resource = resource;
      imageBytes = null;
      try
      {
        ImageObj = new Bitmap(System.IO.Path.Combine(Global.ProjectPath, resource.Path));
      } catch
      {
        Global.Log.AddEntry("Invalid Image Path: " +  resource.Path);
      }
      
      Tainted = false;
    }

    public void Release()
    {
      imageBytes = null;
      if(ImageObj != null)
      {
        ImageObj.Dispose();
        ImageObj = null;
      }
      
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
