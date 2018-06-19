var count = 0;
var image;

function Init() 
{
  Root.Init(1, 6, true);
  
  var title = new Title();
  title.Content = "05 Image";
  Root.Add(title, 0, 0);
  
  var content = new Text();
  content.Content = "The image will be changed from the server.";
  Root.Add(content, 0, 1);
  
  image = new Image();
  image.Set("blueButton");  
  Root.Add(image, 0 , 2);
  
  var button = new Button();
  button.Content = "Continue";
  button.OnClick("ButtonClicked");
  Root.Add(button, 0, 5);
  
  Server.Invoke("SetText1", "Test 05 is running");
}  
  
function ButtonClicked()
{
  Clients.GetLocal().StartScreen("06 Patcher");
}

function SwitchImage(arg)
{
  if(count == 0) 
  {
    image.Set("blueButton");
    count = 1;
  } else {
    image.Set("greenButton");
    count = 0;
  }
}