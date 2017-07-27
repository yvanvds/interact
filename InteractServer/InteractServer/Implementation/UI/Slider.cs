﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;
using System.Windows.Media;
using System.Windows;

namespace InteractServer.Implementation.UI
{
  public class Slider : Interact.UI.Slider
  {
    private System.Windows.Controls.Slider UIObject = new System.Windows.Controls.Slider();
    private Color backgroundColor;

    class Handler
    {
      public string name;
      public object[] arguments;
    }
    private Handler OnChangeHandler;

    public Slider()
    {
      UIObject.Uid = Guid.NewGuid().ToString();
    }

    public override double Minimum { get => UIObject.Minimum; set => UIObject.Minimum = value; }
    public override double Maximum { get => UIObject.Maximum; set => UIObject.Maximum = value; }
    public override double Value { get => UIObject.Value; set => UIObject.Value = value; }

    public override Interact.UI.Color BackgroundColor
    {
      set
      {
        UIObject.Background = new SolidColorBrush((System.Windows.Media.Color)value.InternalObject);
        backgroundColor = new Color((System.Windows.Media.Color)value.InternalObject);
      }
      get => backgroundColor;
    }

    public override object InternalObject => UIObject;

    public override void OnChange(string functionName, params object[] arguments)
    {
      OnChangeHandler = new Handler()
      {
        name = functionName,
        arguments = arguments
      };
      UIObject.ValueChanged += OnChangeEvent;
      JintEngine.Runner.EventHandler.RegisterChanged(UIObject.Uid, functionName, arguments);
      UIObject.MouseDoubleClick += JintEngine.Runner.EventHandler.OnValueChanged;
    }

    private void OnChangeEvent(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      JintEngine.Runner.Engine.InvokeMethod(OnChangeHandler.name, OnChangeHandler.arguments);
    }
  }
}
