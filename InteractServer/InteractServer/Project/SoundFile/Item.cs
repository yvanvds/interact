using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project.SoundFile
{
  public class Item : IDiskResource
  {
    public enum Format
    {
      WAV,
      OGG,
      MP3,
      UNKNOWN,
    }

    private IDiskResource resource;

    // implements IContentModel
    public Guid ID { get => resource.ID; set => resource.ID = value; }
    public string Name => resource.Name;

    [JsonIgnore]
    public string Path { get => resource.Path; set => resource.Path = value; }

    [JsonIgnore]
    public string Data { get => resource.Data; set => resource.Data = value; }
    [JsonIgnore]
    public bool Tainted { get; set; }
    [JsonIgnore]
    public ContentType Type { get => resource.Type; set => resource.Type = value; }
    public void MoveTo(string path) { resource.MoveTo(path); }

    public int Version { get; set; }

    public string SourcePath { get; set; }

    public byte[] Content { get; set; }

    IYse.ISound sound = null;

    public Item(IDiskResource resource)
    {
      this.resource = resource;
      string file = System.IO.Path.Combine(Global.ProjectPath, resource.Path);
      Tainted = false;
    }

    public static Format GetFormat(string FileName)
    {
      string ext = System.IO.Path.GetExtension(FileName);

      Format format = Format.UNKNOWN;
      if (ext.Equals(".wav", StringComparison.CurrentCultureIgnoreCase)) format = Format.WAV;
      else if (ext.Equals(".ogg", StringComparison.CurrentCultureIgnoreCase)) format = Format.OGG;
      else if (ext.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase)) format = Format.MP3;

      return format;
    }

    public void Replace(string path)
    {
      Path = path;
      Global.Log.AddEntry("Soundfile replaced with " + path);
    }

    public void Play()
    {
      if(sound == null)
      {
        sound = Global.Yse.NewSound();
        sound.Create(System.IO.Path.Combine(Global.ProjectPath, resource.Path));
        sound.Play();
      } else if (!sound.Playing)
      {
        sound.Play();
      }
    }

    public void Stop()
    {
      if(sound != null)
      {
        sound.Stop();
      }
    }

    public bool IsPlaying()
    {
      if (sound == null) return false;
      return sound.Playing;
    }

    public String Serialize()
    {
      return JsonConvert.SerializeObject(this);
    }

    public void SendToClient(Guid clientID)
    {
      Global.Clients.Get(clientID).Send.SoundfileSet(Global.ProjectManager.Current.ProjectID(), ID, Serialize());
    }
  }
}
