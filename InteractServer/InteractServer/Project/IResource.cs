using Newtonsoft.Json.Linq;
using System;
using tainicom.WpfPropertyGrid;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
    [BrowsableProperty(BrowsableCategoryAttribute.All, false)]
    [BrowsableProperty("Name", true)]
    [BrowsableProperty("ID", true)]
    [BrowsableProperty("Version", true)]
    public interface IResource
	{
		string DisplayName { get; }
		string Name { get; }
		string Location { get; }
		ContentType Type { get; }
		string ID { get; }
		string Icon { get; }
		int Version { get; }

		LayoutDocument Document { get; }

		void MoveTo(string path);

		JObject SaveToJson();
		bool LoadFromJson(JObject obj);
		string SerializeForClient();

		bool SaveContent();
		void DeleteOnDisk();
		bool NeedsSaving();

		void OnShow();
	}
}
