var text1;
var text2;


var slider;
var sliderText;

var video;

function Init() 
{
  Root.Init(4, 10, true);
  
  text1 = new Text();
  Root.Add(text1, 0, 0);
  
  text2 = new Text();
  Root.Add(text2, 0, 1);
  
  var button = new Button();
  button.Content = "Click Test";
  button.OnClick("ButtonClicked");
  button.OnRelease("ButtonReleased");
  button.SendOSC(Server.IpAddress, 3000, "/note/1");
  Root.Add(button, 1, 1);
  
  var imageButton = new Button();
  imageButton.Content = "Switch Image";
  imageButton.OnClick("ImageButtonClicked");
  Root.Add(imageButton, 2, 1);
  
  slider = new Slider();
  slider.Minimum = 0;
  slider.Maximum = 100;
  slider.OnChange("SliderChanged");
  slider.SendOSC(Server.IpAddress, 3000, "/sensor/light");
  Root.Add(slider, 0, 2);
  
  sliderText = new Text();
  Root.Add(sliderText, 1, 2);
 
  video = new GlMixer();
  video.Connect("192.168.0.101", 4000);
}

function SetText1(content) 
{
  text1.Content = content;
}

function SetText2(content) 
{
  text2.Content = content;
}

function ButtonClicked() 
{
  Server.Log("Button Clicked");
  video.Paused = !video.Pause;
}

function ButtonReleased() 
{
  Server.Log("Button Released");
}

function ImageButtonClicked() 
{
  Clients.Invoke("SwitchImage", "");
}

function SetSlider(value) 
{
  slider.Value = value;
}

function SliderChanged() 
{
  sliderText.Content = "Value: " + slider.Value;
  video.RenderAlpha = slider.Value;
}