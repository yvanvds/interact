using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
	public abstract class AbstractFolder
	{
		private string name = string.Empty;
		public string Name => name;

		private string icon = string.Empty;
		public string Icon => icon;

		protected string path = string.Empty;

		public bool IsExpanded { get; set; } = false;

		protected ObservableCollection<FileGroup> groups = new ObservableCollection<FileGroup>();
		public ObservableCollection<FileGroup> Groups => groups;

		public AbstractFolder(string name, string path, string icon)
		{
			this.name = name;
			this.path = path;
			this.icon = icon;

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		public bool FileExists(string name, ContentType type)
		{
			string path = string.Empty;
			switch(type)
			{
				case ContentType.ClientArduino:
					{
						path = Path.Combine(this.path, "Arduino", name + ".json");
						break;
					}
				case ContentType.ServerGui:
				case ContentType.ClientGui:
					{
						path = Path.Combine(this.path, "Gui", name + ".json");
						break;
					}
				case ContentType.ServerPatcher:
				case ContentType.ClientPatcher:
					{
						path = Path.Combine(this.path, "Patcher", name + ".yap");
						break;
					}
				case ContentType.ServerScript:
				case ContentType.ClientScript:
					{
						path = Path.Combine(this.path, "Script", name + ".cs");
						break;
					}
				case ContentType.ClientSensors:
					{
						path = Path.Combine(this.path, "Sensor", name + ".json");
						break;
					}
				case ContentType.ServerSounds:
				case ContentType.ClientSounds:
					{
						path = Path.Combine(this.path, "Sound", name + ".json");
						break;
					}
				default: return false;
			}

			return File.Exists(path);
		}

		public IResource Get(string id)
		{
			foreach(var group in groups)
			{
				var resource = group.Get(id);
				if (resource != null) return resource;
			}
			return null;
		}

		public IResource GetByName(string name)
		{
			foreach (var group in groups)
			{
				var resource = group.GetByName(name);
				if (resource != null) return resource;
			}
			return null;
		}

		public string GetName(string ID)
		{
			foreach(var group in groups)
			{
				var result = group.GetName(ID);
				if (result.Length > 0) return result;
			}
			return string.Empty;
		}

		public abstract bool CreateResource(string name, ContentType type);
		public void RemoveResource(IResource resource)
		{
			foreach(var group in groups)
			{
				if(group.Contains(resource))
				{
					group.RemoveResource(resource);
					return;
				}
			}
		}

		public bool Contains(FileGroup group)
		{
			return groups.Contains(group);
		}

		public bool Contains(IResource resource)
		{
			foreach (var group in groups)
			{
				if (group.Contains(resource))
				{
					return true;
				}
			}
			return false;
		}

		public abstract bool Load(JObject obj);
		public abstract bool SaveToJson(JObject obj);


		protected bool needsSaving = false;
		public bool NeedsSaving()
		{
			if (needsSaving) return true;
			foreach(var group in groups)
			{
				if (group.NeedsSaving()) return true;
			}
			return false;
		}

		public void SaveContent()
		{
			foreach (var group in groups)
			{
				if(group.NeedsSaving()) group.SaveContent();
			}
		}
	}
}
