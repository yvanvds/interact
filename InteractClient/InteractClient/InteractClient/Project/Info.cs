using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Project
{
	public class Info
	{
		public string Name { get; set; }
		public string Origin { get; set; }
		public DateTime DownloadDate { get; set; }
		public DateTime LastUseDate { get; set; }
		public string FirstScreen { get; set; }

		public string DownloadDateString { get => DownloadDate.ToShortDateString(); }
		public string LastUseDateString { get => DownloadDate.ToShortDateString(); }

		public string Guid { get; set; } // only used for display list

		public string Serialize()
		{
			return JsonConvert.SerializeObject(this);
		}

		public void Deserialize(string data)
		{
			try
			{
				JsonConvert.PopulateObject(data, this);
			}
			catch (JsonException)
			{

			}
		}
	}
}
