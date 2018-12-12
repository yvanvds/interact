using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace InteractServer.Project
{
	public interface IFolder
	{
		string Name { get; }
		string Icon { get; }
		bool IsExpanded { get; set; }

		bool FileExists(string name, ContentType type);

		ObservableCollection<FileGroup> Groups { get; }

		bool SaveToJson(JObject obj);
		void SaveContent();
		bool NeedsSaving();
	}
}
