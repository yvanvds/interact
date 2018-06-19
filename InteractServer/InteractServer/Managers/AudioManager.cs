using InteractServer.Project.Patcher;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Managers
{

  public class AudioManager
  {
    public class AudioObject
    {
      public Item patchModel;
      public IYse.ISound sound;
      public IYse.IPatcher patchAudio;

      public AudioObject(Item patchModel)
      {
        this.patchModel = patchModel;
        sound = Global.Yse.NewSound();
        patchAudio = Global.Yse.NewPatcher();
        patchAudio.Create(1);

        sound.Create(patchAudio);

        if(patchModel.Content != null)
        {
          if(patchModel.Content.Length > 0) {
            patchAudio.ParseJSON(patchModel.Content);
          }
        }
      }

      bool enableAudio = false;
      public bool EnableAudio {
        get => enableAudio;
        set {
          enableAudio = value;
          if(enableAudio && !sound.Playing)
          {
            sound.Play();
          } else if (!enableAudio && sound.Playing)
          {
            sound.Stop();
          }
        }
      }
    }

    Collection<AudioObject> List;

    public AudioManager()
    {
      List = new Collection<AudioObject>();
    }


    public AudioObject GetObject(Item patchModel)
    {
      foreach(var l in List)
      {
        if (l.patchModel == patchModel) return l;
      }
      return null;
    }

    public void AddObject(Item patchModel)
    {
      foreach(var l in List)
      {
        if(l.patchModel == patchModel)
        {
          return;
        }
      }

      List.Add(new AudioObject(patchModel));
    }

    public void DeleteObject(Item patchModel)
    {
      foreach(var l in List)
      {
        if(l.patchModel == patchModel)
        {
          l.sound.Stop();
          l.sound.Dispose();
          l.patchAudio.Dispose();
          List.Remove(l);
          return;
        }
      }
    }

    public void EnableAudio(Item patchModel, bool enable)
    {
      foreach(var l in List)
      {
        if (l.patchModel == patchModel)
        {
          l.EnableAudio = enable;
          return;
        }
      }
    }

    public bool AudioEnabled(Item patchModel)
    {
      foreach(var l in List)
      {
        if(l.patchModel == patchModel)
        {
          return l.EnableAudio;
        }
      }
      return false;
    }
  }
}
