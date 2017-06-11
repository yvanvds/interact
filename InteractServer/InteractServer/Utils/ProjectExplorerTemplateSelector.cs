using InteractServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InteractServer.Utils
{
    public class ProjectExplorerTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if(element != null && item != null)
            {
                if(item is ProjectResourceGroup)
                {
                    return element.FindResource("GroupTemplate") as DataTemplate;
                } else if(item is Models.Image)
                {
                    return element.FindResource("ImageTemplate") as DataTemplate;
                } else if (item is SoundFile)
                {
                    return element.FindResource("SoundTemplate") as DataTemplate;
                } else if (item is Screen)
                {
                    return element.FindResource("ScreenTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
