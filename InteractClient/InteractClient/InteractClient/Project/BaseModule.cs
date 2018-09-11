using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Project
{
	public abstract class BaseModule
	{
		public string Name { get; set; }
		public string ID { get; set; }
		public int Version { get; set; }
		public string Type { get; set; }
		public string Content { get; set; }

		public void Deserialize(JObject data)
		{
			if (data.ContainsKey("Name")) Name = (string)data["Name"];
			if (data.ContainsKey("ID")) ID = (string)data["ID"];
			if (data.ContainsKey("Version")) Version = (int)data["Version"];
			if (data.ContainsKey("Type")) Type = (string)data["Type"];
			if (data.ContainsKey("Content")) Content = (string)data["Content"];
		}

		public abstract void LoadContent();
		public abstract void Activate();
	}
}
