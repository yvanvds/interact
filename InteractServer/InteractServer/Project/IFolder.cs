using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace InteractServer.Project
{
	public interface IFolder
	{
		string Name { get; }
		string Icon { get; }
		bool IsExpanded { get; set; }

		int Count { get; }
		string GuiCount { get; }

		bool FileExists(string name);

		ObservableCollection<IResource> Resources { get; }

		bool SaveToJson(JObject obj);
		void SaveContent();
		bool NeedsSaving();
	}
}
