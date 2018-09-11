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

		public int Count => Resources.Count;

		public string GuiCount => " [" + Count.ToString() + "]";

		protected ObservableCollection<IResource> resources = new ObservableCollection<IResource>();
		public ObservableCollection<IResource> Resources => resources;

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

		public bool FileExists(string name)
		{
			foreach (var resource in resources)
			{
				if (resource.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public IResource Get(string id)
		{
			foreach(var resource in resources)
			{
				if (resource.ID.Equals(id)) return resource;
			}
			return null;
		}

		public IResource GetByName(string name)
		{
			foreach (var resource in resources)
			{
				if (resource.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) return resource;
			}
			return null;
		}

		public abstract bool CreateResource(string name, ContentType type);
		public bool RemoveResource(IResource resource)
		{
			resource.DeleteOnDisk();
			resources.Remove(resource);
			return true;
		}

		public abstract bool Load(JObject obj);
		public abstract bool SaveToJson(JObject obj);


		protected bool needsSaving = false;
		public bool NeedsSaving()
		{
			if (needsSaving) return true;
			foreach(var resource in resources)
			{
				if (resource.NeedsSaving()) return true;
			}
			return false;
		}

		public void SaveContent()
		{
			foreach (var resource in resources)
			{
				if(resource.NeedsSaving()) resource.SaveContent();
			}
		}
	}
}
