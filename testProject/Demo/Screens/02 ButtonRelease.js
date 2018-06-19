var button1;
var button2;

function Init()
{
  Root.Init(1, 6, true);
  
  var title = new Title();
  title.Content = "02 Button Release";
  Root.Add(title, 0, 0);
  
  {
    button1 = new Button();
    button1.Content = "Button 1";
    button1.OnClick("OnClick", 1);
    button1.OnRelease("OnRelease", 1);
    Root.Add(button1, 0, 1);
  }
  
  {
    button2 = new Button();
    button2.Content = "Button 2";
    button2.OnClick("OnClick", 2);
    button2.OnRelease("OnRelease", 2);
    Root.Add(button2, 0, 2);
  }
  
  {
    var text = new Text();
    text.Content = "This test shows button press with velocity and button release. Both send a message to the server";
    Root.Add(text, 0, 3);
  }
  
  {
    var text = new Text();
    text.Content = "Note: velocity and release detection are not available on all platforms.";
    Root.Add(text, 0, 4);
  }
  
  {
    var button = new Button();
    button.Content = "Continue";
    button.OnClick("ContinueClicked");
    Root.Add(button, 0, 5);
  }
  
  Server.Invoke("SetText1", "Test 02 is running");
}

function OnClick(buttonnr) 
{
  var velocity = 0;
  if(buttonnr == 1) 
  {
    velocity = button1.Pressure;
  } else {
    velocity = button2.Pressure;
  }
  
  Server.Invoke("SetText2", "Button " + buttonnr + " is pressed with velocity " + velocity);
}

function OnRelease(buttonnr)
{
  Server.Invoke("SetText2", "Button " + buttonnr + " is released");
}

function ContinueClicked() 
{
  Clients.GetLocal().StartScreen("03 Color");
}