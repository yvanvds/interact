using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface.Resolume
{
	public class Video
	{
		ISender parent;
	
		public Video(ISender parent)
		{
			this.parent = parent;
		}

		public void Fadeout(float value)
		{
			parent.Send("/video/fadeout", value);
		}

		public void BlendMode(int value)
		{
			parent.Send("/video/mixeroption4", value);
		}

		public void Opacity(float value)
		{
			parent.Send("/video/opacity", value);
		}

		public void Scale(float value)
		{
			parent.Send("/video/scale", value);
		}

		public void PositionX(float value)
		{
			parent.Send("/video/positionx", value);
		}

		public void PositionY(float value)
		{
			parent.Send("/video/positiony", value);
		}

		public void RotateX(float value)
		{
			parent.Send("/video/rotatex", value);
		}

		public void RotateY(float value)
		{
			parent.Send("/video/rotatey", value);
		}

		public void RotateZ(float value)
		{
			parent.Send("/video/rotatez", value);
		}

		public void AnchorX(float value)
		{
			parent.Send("/video/anchorx", value);
		}

		public void AnchorY(float value)
		{
			parent.Send("/video/anchory", value);
		}

		public void AnchorZ(float value)
		{
			parent.Send("/video/anchorz", value);
		}

		public void TransitionMixerOption(int value)
		{
			parent.Send("/video/transitionmixeroption", value);
		}
	}
}
