using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InteractClient.JintEngine;
using PCLStorage;
using Shared.Project;


namespace InteractClient.Data
{
  public class Project
  {
    public Guid ID { get; set; }
    public int Version { get; set; }
    public Dictionary<Guid, Screen> Screens = new Dictionary<Guid, Screen>();
    public Dictionary<Guid, Image> Images = new Dictionary<Guid, Image>();
    public Dictionary<Guid, SoundFile> SoundFiles = new Dictionary<Guid, SoundFile>();
    public Dictionary<Guid, Patcher> Patchers = new Dictionary<Guid, Patcher>();

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

		private ProjectCache projectCache;

    public static async void SetCurrent(Guid ID, int Version)
    {
      if (List.ContainsKey(ID))
      {
        Current = List[ID];

        // request project data if not on the same version
        if (Current.Version != Version)
        {
          Network.Sender.Get().WriteLog("Project->SetCurrent: project " + ID.ToString() + " found. Requested update.");
					await Current.UpdateProject(Version);
					Network.Sender.Get().ProjectUpdateReady(ID);
				}
				else
        {
          Network.Sender.Get().WriteLog("Project->SetCurrent: project " + ID.ToString() + " found and up to date.");
					Network.Sender.Get().ProjectUpdateReady(ID);
				}
				Engine.Instance.SetScreenMessage("Project is Ready", false);
				return;
      }

      // create new project
      try
      {
        Current = new Project(ID, Version);
				await Current.projectCache.Init();
        List.Add(ID, Current);
        Network.Sender.Get().WriteLog("Project->SetCurrent: new project added with ID " + ID.ToString());
				await Current.UpdateProject(Version);
				Network.Sender.Get().ProjectUpdateReady(ID);
				Engine.Instance.SetScreenMessage("Project is Ready", false);
			}
      catch (NullReferenceException)
      {
        Network.Sender.Get().WriteLog("Project->SetCurrent: Unable to add project");
				Engine.Instance.SetScreenMessage("Project failed to load!", false);
      }
    }

		public Project(Guid id, int version)
		{
			this.ID = id;
			this.Version = version;
			this.projectCache = new ProjectCache(ID);
		}

		public async Task UpdateProject(int version)
		{
			await projectCache.UpdateProject();
			SetConfig(projectCache.Config);
			await projectCache.RefreshScreens(Screens);
			await projectCache.RefreshImages(Images);
			await projectCache.RefreshPatchers(Patchers);
			await projectCache.RefreshSounds(SoundFiles);
			Version = version;
		}

    public Screen GetScreen(Guid ID)
    {
			if(Screens.ContainsKey(ID))
			{
				return Screens[ID];
			}			
      return null;
    }

    public Image GetImage(Guid ID)
    {
			if(Images.ContainsKey(ID))
			{
				return Images[ID];
			}
      return null;
    }

    public SoundFile GetSoundFile(Guid ID)
    {
			if(SoundFiles.ContainsKey(ID))
			{
				return SoundFiles[ID];
			}
      return null;
    }

    public Patcher GetPatcher(Guid ID)
    {
			if(Patchers.ContainsKey(ID))
			{
				return Patchers[ID];
			}
      return null;
    }

    public Screen GetScreen(string Name)
    {
      foreach (Screen screen in Screens.Values)
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
      foreach (Image image in Images.Values)
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
      foreach (SoundFile soundfile in SoundFiles.Values)
      {
        if (soundfile.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
        {
          return soundfile;
        }
      }
      return null;
    }

    public Patcher GetPatcher(string Name)
    {
      foreach (Patcher patcher in Patchers.Values)
      {
        if(patcher.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
        {
          return patcher;
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

      Network.Sender.Get().WriteLog("Project->SetConfig: Project Configuration set.");
    }

  }
}
