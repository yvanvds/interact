using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface.Resolume
{
	public class Composition : ISender
	{
		ISender parent;

		Audio audio;
		public Audio Audio => audio;

		Video video;
		public Video Video => video;

		Dictionary<int, Deck> deck = new Dictionary<int, Deck>();
		public Dictionary<int, Deck> Deck => deck;

		public Composition(ISender parent)
		{
			this.parent = parent;
			audio = new Audio(this);
			video = new Video(this);
			NumberOfDecks(3);
		}

		public void Send(string route, object[] arguments)
		{
			parent.Send("/composition" + route, arguments);
		}

		public void Send(string route, object argument)
		{
			parent.Send("/composition" + route, argument);
		}

		public void NumberOfDecks(int value)
		{
			deck.Clear();
			for(int i = 1; i <= value; i++)
			{
				deck.Add(i, new Deck(this, i));
			}
		}

		public void DisconnectAll(bool value)
		{
			Send("/disconnectall", value ? 1 : 0);
		}

		public void ByPassed(bool value)
		{
			Send("/bypassed", value ? 1 : 0);
		}

		public void Select()
		{
			Send("/select", 1);
		}

		public void OpacityAndVolume(float value)
		{
			Send("/opacityandvolume", value);
		}

		public void FadeToGroupA()
		{
			Send("/fadetogroupa", 1);
		}

		public void FadeToGroupB()
		{
			Send("/fadetogroupb", 1);
		}

		public void Cross(float value)
		{
			Send("/cross", value);
		}
	}
}
