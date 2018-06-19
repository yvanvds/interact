var title;
var content;
var colorButton;

function Init()
{
  Root.Init(1, 4, true);
  
  title = new Title();
  title.Content = "03 Color";
  Root.Add(title, 0, 0);
  
  content = new Text();
  content.Content = "This test changes several colors every time you press the button below.";
  Root.Add(content, 0, 1);
  
  colorButton = new Button();
  colorButton.Content = "Change Colors";
  colorButton.OnClick("ChangeColor");
  Root.Add(colorButton, 0, 2);
  
  button = new Button();
  button.Content = "Continue";
  button.OnClick("ContinueClicked");
  Root.Add(button, 0, 3);
  
  Server.Invoke("SetText1", "Test 03 (Color) is running");
  Server.Invoke("SetText2", "");
}

function ChangeColor() 
{
  var color = new Color;
  color.SetColor(Math.random() * 255, Math.random() * 255, Math.random() * 255, 255);
  Root.BackgroundColor = color;
  
  color.SetColor(Math.random() * 255, Math.random() * 255, Math.random() * 255, 255);
  title.TextColor = color;
  
  color.SetColor(Math.random() * 255, Math.random() * 255, Math.random() * 255, 255);
  title.BackgroundColor = color;
  
  color.SetColor(Math.random() * 255, Math.random() * 255, Math.random() * 255, 255);
  content.TextColor = color;
  
  color.SetColor(Math.random() * 255, Math.random() * 255, Math.random() * 255, 255);
  content.BackgroundColor = color;
  
  color.SetColor(Math.random() * 255, Math.random() * 255, Math.random() * 255, 255);
  colorButton.TextColor = color;
  
  color.SetColor(Math.random() * 255, Math.random() * 255, Math.random() * 255, 255);
  colorButton.BackgroundColor = color;
}

function ContinueClicked() 
{
  Clients.GetLocal().StartScreen("04 Slider");
}