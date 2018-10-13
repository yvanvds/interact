﻿using Newtonsoft.Json.Linq;
using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
	public interface IResource
	{
		string Name { get; }
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