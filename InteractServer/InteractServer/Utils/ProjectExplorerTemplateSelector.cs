using InteractServer.Project;
using System.Windows;
using System.Windows.Controls;

namespace InteractServer.Utils
{
	public class ProjectExplorerTemplateSelector : DataTemplateSelector
	{
		public DataTemplate FolderTemplate { get; set; }
		public DataTemplate ScreenTemplate { get; set; }
		public DataTemplate ScriptTemplate { get; set; }
		public DataTemplate GuiTemplate { get; set; }
		public DataTemplate SoundPageTemplate { get; set; }
		public DataTemplate ImageTemplate { get; set; }
		public DataTemplate PatcherTemplate { get; set; }
		public DataTemplate SensorTemplate { get; set; }


		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;

			if (element != null && item != null)
			{
				if (item is IFolder)
				{
					return FolderTemplate;
				}
				else if (item is Gui)
				{
					return GuiTemplate;
				}
				else if (item is Patcher)
				{
					return PatcherTemplate;
				}
				else if (item is Script)
				{
					return ScriptTemplate;
				}
				else if (item is SoundPage)
				{
					return SoundPageTemplate;
				}
				else if (item is SensorConfig)
				{
					return SensorTemplate;
				}
			}

			return null;
		}
	}
}
