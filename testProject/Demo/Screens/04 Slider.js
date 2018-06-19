var slider;

function Init() 
{
  Root.Init(1, 6, true);
  
  var title = new Title();
  title.Content = "04 Slider";
  
  Root.Add(title, 0, 0);
  
  var content = new Text();
  content.Content = "Moving this slider should also move the one on the server.";
  Root.Add(content, 0, 1);
  
  slider = new Slider();
  slider.Minimum = 0;
  slider.Maximum = 100;
  slider.OnChange("SliderChanged");
  slider.SendOSC(Server.IpAddress, 3000, "/sensor/proximity");
  Root.Add(slider, 0, 2);
  
  var button = new Button();
  button.Content = "Continue";
  button.OnClick("ButtonClicked");
  Root.Add(button, 0, 5);
  
  Server.Invoke("SetText1", "Test 04 is running");
}

function ButtonClicked() {
  Clients.GetLocal().StartScreen("05 Image");
}

function SliderChanged() 
{
  Server.Invoke("SetSlider", slider.Value);
}