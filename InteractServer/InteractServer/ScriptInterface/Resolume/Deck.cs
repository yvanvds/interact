using System;
using System.Collections.Generic;
using System.Text;

namespace Script.Resolume
{
	public class Deck
	{
		ISender parent;
		int ID;

		public Deck(ISender parent, int ID)
		{
			this.parent = parent;
			this.ID = ID;
		}

		public void Select()
		{
			parent.Send("/deck" + ID + "/select", 1);
		}
	}
}
