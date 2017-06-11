﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine.UI
{
    public class Button : Xamarin.Forms.Button
    {
        public Button()
        {
            BackgroundColor = Data.Project.Current.ConfigButton.Background.Get();
            TextColor = Data.Project.Current.ConfigButton.Foreground.Get();
        }

        public void OnClick(String functionName)
        {
            Engine.EventHandler.Register(Id, functionName);
            Clicked += Engine.EventHandler.OnClick;
        }
    }
}
