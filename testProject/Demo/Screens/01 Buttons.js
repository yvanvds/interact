function Init() 
{
  Root.Init(1, 4, true);
    
  var title = new Title();
  title.Content = "01 Button";
  Root.Add(title, 0, 0);
  
  var content = new Text();
  content.Content = "This test shows a button. Touching it should take you to the next test.";
  Root.Add(content, 0, 1);
  
  var button = new Button();
  button.Content = "Continue";
  button.OnClick("ButtonClicked");
  Root.Add(button, 0, 3);
  
  Server.Invoke("SetText1", "Test 01 is running");
}

function ButtonClicked()
{
  Clients.GetLocal().StartScreen("02 ButtonRelease");
}