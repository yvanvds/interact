function Init() 
{
  Root.Init(1, 4, true);
  
  var patcher = new Patcher();
  patcher.Load("simplePatcher");
  patcher.EnableAudio();
  
  var button = new Button();
  button.Content = "Continue";
  button.OnClick("ButtonClicked");
  Root.Add(button, 0, 3);
}


function ButtonClicked()
{
  Clients.GetLocal().StartScreen("11 Arduino");
}