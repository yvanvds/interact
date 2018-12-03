using System;
using System.Collections.Generic;
using System.Text;

namespace Script.Resolume
{
	public interface ISender
	{
		void Send(string route, object[] arguments);
		void Send(string route, object argument);
	}

	public class Resolume: ISender
	{
		//private IOsc osc;

		private Composition composition;
		public Composition Composition => composition;

		private PlaybackController playbackController;
		public PlaybackController PlaybackController => playbackController;

		private Dictionary<string, Layer> layer = new Dictionary<string, Layer>();
		public Dictionary<string, Layer> Layer => layer;

		private Layer activeLayer;
		public Layer ActiveLayer => activeLayer;

		public Resolume()//IOsc Osc)
		{
			//osc = Osc;
			composition = new Composition(this);
			playbackController = new PlaybackController(this);
			activeLayer = new Layer(this, "activelayer");
		}

		public void Send(string route, object[] arguments)
		{
			//osc.ToResolume(route, arguments);
		}

		public void Send(string route, object argument)
		{
			//osc.ToResolume(route, new object[] { argument });
		}

		public void AddLayer(string name)
		{
			Layer.Add(name, new Layer(this, name));
		}
	}
}
