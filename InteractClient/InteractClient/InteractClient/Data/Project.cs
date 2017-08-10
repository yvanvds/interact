using Shared.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Data
{
  public class Project
  {
    public Guid ID { get; set; }
    public int Version { get; set; }
    public List<Screen> Screens = new List<Screen>();
    public List<Image> Images = new List<Image>();
    public List<SoundFile> SoundFiles = new List<SoundFile>();

    // Static fields for project management
    public static Project Current { get => current; set => current = value; }
    public static Dictionary<Guid, Project> List = new Dictionary<Guid, Project>();
    private static Project current = null;

    // public fields for config values
    public ConfigValues ConfigValues = new ConfigValues();
    public Background ConfigPage = new Background();
    public Button ConfigButton = new Button();
    public Text ConfigText = new Text();
    public Title ConfigTitle = new Title();

    public Implementation.Network.Clients Clients = new Implementation.Network.Clients();

    public static void SetCurrent(Guid ID, int Version)
    {
      if (List.ContainsKey(ID))
      {
        Current = List[ID];
        Current.Clients.Clear();

        // request project data if not on the same version
        if (Current.Version != Version)
        {
          Network.Signaler.Get().WriteLog("Project->SetCurrent: project " + ID.ToString() + " found. Requested update.");
          Network.Signaler.Get().GetProjectConfig(ID);
          Current.Version = Version; // might be better to do this with the actual update, but then we have to resend the version, which feels wrong because the update will be done in many parts
        }
        else
        {
          Network.Signaler.Get().WriteLog("Project->SetCurrent: project " + ID.ToString() + " found and up to date.");
          Network.Signaler.Get().GetNextMethod();
        }
        return;
      }

      // create new project
      Current = new Project { ID = ID, Version = Version };
      List.Add(ID, Current);
      Network.Signaler.Get().WriteLog("Project->SetCurrent: new project added with ID " + ID.ToString());
      Network.Signaler.Get().GetProjectConfig(ID);

    }

    public void UpdateScreen(int ID, string content)
    {
      foreach (Screen screen in Screens)
      {
        if (screen.ID == ID)
        {
          screen.Deserialize(content);
          Network.Signaler.Get().WriteLog("Project->UpdateScreen: screen " + ID + " updated.");
          goto next;
        }
      }

      // create a new screen
      Screen s = new Screen();
      s.Deserialize(content);
      Screens.Add(s);
      Network.Signaler.Get().WriteLog("Project->UpdateScreen: new screen " + ID + " added.");

      next:
      Network.Signaler.Get().GetNextMethod();
    }

    public void UpdateImage(int ID, string content)
    {
      foreach (Image image in Images)
      {
        if (image.ID == ID)
        {
          image.Deserialize(content);
          Network.Signaler.Get().WriteLog("Project->UpdateImage: image " + image.Name + " updated.");
          goto next;
        }
      }

      Image newImage = new Image();
      newImage.Deserialize(content);
      Images.Add(newImage);
      Network.Signaler.Get().WriteLog("Project->UpdateImage: new image " + newImage.Name + " added.");

      next:
      Network.Signaler.Get().GetNextMethod();
    }

    public void UpdateSoundFile(int ID, string content)
    {
      foreach (SoundFile sf in SoundFiles)
      {
        if (sf.ID == ID)
        {
          sf.Deserialize(content);
          Network.Signaler.Get().WriteLog("Project->UpdateSoundFile: sound " + sf.Name + " updated.");
          goto next;
        }
      }

      SoundFile newSF = new SoundFile();
      newSF.Deserialize(content);
      SoundFiles.Add(newSF);
      Network.Signaler.Get().WriteLog("Project->UpdateSoundFile: new sound " + newSF.Name + " added.");

      next:
      Network.Signaler.Get().GetNextMethod();
    }

    public void SetScreenVersion(Guid projectID, int screenID, int Version)
    {
      foreach (Screen screen in Screens)
      {
        if (screen.ID == screenID)
        {
          if (screen.Version == Version)
          {
            Network.Signaler.Get().WriteLog("Project->SetScreenVersion: screen " + screenID + " is on the right version.");
            Network.Signaler.Get().GetNextMethod();
          }
          else
          {
            Network.Signaler.Get().WriteLog("Project->SetScreenVersion: Screen " + screenID + " needs an update.");
            Network.Signaler.Get().GetScreen(projectID, screenID);
          }
          return;
        }
      }

      // screen not found, so ask for it anyway
      Network.Signaler.Get().WriteLog("Project->SetScreenVersion: Screen not found. Requesting content for screen " + screenID + ".");
      Network.Signaler.Get().GetScreen(projectID, screenID);

    }

    public void SetImageVersion(Guid projectID, int imageID, int Version)
    {
      foreach (Image image in Images)
      {
        if (image.ID == imageID)
        {
          if (image.Version == Version)
          {
            Network.Signaler.Get().WriteLog("Project->SetImageVersion: image " + image.Name + " is on the right version.");
            Network.Signaler.Get().GetNextMethod();
          }
          else
          {
            Network.Signaler.Get().WriteLog("Project->SetImageVerison: image " + image.Name + " needs an update.");
            Network.Signaler.Get().GetImage(projectID, imageID);
          }
          return;
        }
      }

      // image not found, so ask for it anyway
      Network.Signaler.Get().WriteLog("Project->SetImageVersion: Image not found. Requesting content for image" + imageID + ".");
      Network.Signaler.Get().GetImage(projectID, imageID);
    }

    public void SetSoundFileVersion(Guid projectID, int sfID, int Version)
    {
      foreach (SoundFile soundfile in SoundFiles)
      {
        if (soundfile.ID == sfID)
        {
          if (soundfile.Version == Version)
          {
            Network.Signaler.Get().WriteLog("Project->SetSoundFileVersion: sound " + soundfile.Name + " is on the right version.");
            Network.Signaler.Get().GetNextMethod();
          }
          else
          {
            Network.Signaler.Get().WriteLog("Project->SetSoundFileVersion: sound " + soundfile.Name + " needs an update.");
            Network.Signaler.Get().GetSoundFile(projectID, soundfile.ID);
          }
          return;
        }
      }

      // image not found, so ask for it anyway
      Network.Signaler.Get().WriteLog("Project->SetSoundFileVersion: File not found. Requesting content for file " + sfID + ".");
      Network.Signaler.Get().GetSoundFile(projectID, sfID);
    }

    public Screen GetScreen(int ID)
    {
      foreach (Screen screen in Screens)
      {
        if (screen.ID == ID)
        {
          return screen;
        }
      }

      return null;
    }

    public Image GetImage(int ID)
    {
      foreach (Image image in Images)
      {
        if (image.ID == ID) return image;
      }
      return null;
    }

    public SoundFile GetSoundFile(int ID)
    {
      foreach (SoundFile soundfile in SoundFiles)
      {
        if (soundfile.ID == ID) return soundfile;
      }
      return null;
    }

    public Screen GetScreen(string Name)
    {
      foreach (Screen screen in Screens)
      {
        if (screen.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
        {
          return screen;
        }
      }

      return null;
    }

    public Image GetImage(string Name)
    {
      foreach (Image image in Images)
      {
        if (image.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
        {
          return image;
        }
      }
      return null;
    }

    public SoundFile GetSoundFile(string Name)
    {
      foreach (SoundFile soundfile in SoundFiles)
      {
        if (soundfile.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
        {
          return soundfile;
        }
      }
      return null;
    }

    public void SetConfig(Dictionary<string, string> values)
    {
      if (values.ContainsKey("ConfigValues"))
      {
        ConfigValues.DeSerialize(values["ConfigValues"]);
      }
      if (values.ContainsKey("ConfigButton"))
      {
        ConfigButton.DeSerialize(values["ConfigButton"]);
      }
      if (values.ContainsKey("ConfigPage"))
      {
        ConfigPage.DeSerialize(values["ConfigPage"]);
      }
      if (values.ContainsKey("ConfigTitle"))
      {
        ConfigTitle.DeSerialize(values["ConfigTitle"]);
      }
      if (values.ContainsKey("ConfigText"))
      {
        ConfigText.DeSerialize(values["ConfigText"]);
      }

      Network.Signaler.Get().WriteLog("Project->SetConfig: Project Configuration set.");
      Network.Signaler.Get().GetNextMethod();
    }
  }
}
