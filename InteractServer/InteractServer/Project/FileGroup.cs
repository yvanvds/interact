using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InteractServer.Project
{
	public class FileGroup
	{
		private string name = string.Empty;
		public string Name => name;

		private string icon = string.Empty;
		public string Icon => icon;

		protected string path = string.Empty;

		public bool IsExpanded { get; set; } = false;

		public int Count => Resources.Count;

		public string GuiCount => " [" + Count.ToString() + "]";

		public ContentType ContentType { get; set; }

		protected ObservableCollection<IResource> resources = new ObservableCollection<IResource>();
		public ObservableCollection<IResource> Resources => resources;

		private Type targetType;
		private string extension;

		public FileGroup(string name, string path, string icon, string extension, Type targetType, ContentType contentType)
		{
			this.name = name;
			this.path = path;
			this.icon = icon;
			this.extension = extension;
			this.targetType = targetType;
			this.ContentType = contentType;

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		public bool FileExists(string name)
		{
			return File.Exists(Path.Combine(path, name + extension));
		}

		public IResource Get(string id)
		{
			foreach (var resource in resources)
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

		public string GetName(string ID)
		{
			foreach (var resource in resources)
			{
				if (resource.ID.Equals(ID)) return resource.Name;
			}
			return string.Empty;
		}

		public bool Contains(IResource resource)
		{
			return resources.Contains(resource);
		}

		public IResource CreateResource(string name, bool serverSide)
		{
			object resource = Activator.CreateInstance(targetType, new object[] { name + extension, serverSide, this.path });
			resources.Add(resource as IResource);
			return resources.Last();
		}

		public IResource CreateResource(JObject content, bool serverSide)
		{
			object resource = Activator.CreateInstance(targetType, new object[] { content, serverSide, this.path });
			resources.Add(resource as IResource);
			return resources.Last();
		}

		public void RemoveResource(IResource resource)
		{
			resource.DeleteOnDisk();
			resources.Remove(resource);
		}

		private bool needsSaving = false;
		public bool NeedsSaving()
		{
			if (needsSaving) return true;
			foreach (var resource in resources)
			{
				if (resource.NeedsSaving()) return true;
			}
			return false;
		}

		public void SaveContent()
		{
			foreach (var resource in resources)
			{
				if (resource.NeedsSaving()) resource.SaveContent();
			}
		}

		public void SaveToJson(JObject obj)
		{
			foreach(var resource in resources)
			{
				obj[resource.ID] = resource.SaveToJson();
			}
			needsSaving = false;
		}

		public void SaveForClient(JObject obj)
		{
			foreach (var resource in resources)
			{
				obj[resource.ID] = resource.Version;
			}
		}

        public List<string> GetFileNames()
        {
            List<string> result = new List<string>();
            foreach (var resource in resources)
            {
                result.Add(resource.Name);
            }
            return result;
        }
	}
}
