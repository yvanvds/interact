var ledState = false;
var lastValue = 0;

function Init()
{
  Root.Init(1, 6, true);
  
  var title = new Title();
  title.Content = "11 Arduino";
  Root.Add(title, 0, 0);
  
  var content = new Text();
  content.Content = "This a an Arduino Connection Test. It will only work when the client platform is Windows or Windows Phone. (Which also includes Raspberry Pi)";
  Root.Add(content, 0, 1);
  
  var ledButton = new Button();
  ledButton.Content = "Toggle Led";
  ledButton.OnClick("LedButtonClicked");
  ledButton.SendOSC(Server.IpAddress, 3000, "/ledbutton");
  Root.Add(ledButton, 0, 2);
  
  var button = new Button();
  button.Content = "Continue";
  button.OnClick("ButtonClicked");
  
  Root.Add(button, 0, 5);
  Server.Invoke("SetText1", "Test 11 is running");
  
  Arduino.SetDigitalPinMode(2, PinMode.OUTPUT);
  Arduino.SetDigitalPinMode(4, PinMode.INPUT);
  
  Arduino.OnDigitalPinUpdate("DigitalPinUpdate");
  //Arduino.OnAnalogPinUpdate("AnalogPinUpdate");
  Arduino.SendOSC(Server.IpAddress, 3000, "/arduino");
}

function ButtonClicked() {
  Clients.GetLocal().StartScreen("01_button");
}

function LedButtonClicked() {
  if(ledState == false) 
  {
    Arduino.SetdigitalPinState(2, PinState.HIGH);
    Arduino.SetAnalogPinMode(5, PinMode.ANALOG);
    ledState = true;
  } else {
    Arduino.SetdigitalPinState(2, PinState.LOW);
    Arduino.SetAnalogPinMode(5, PinMode.OUTPUT);
    ledState = false;
  } 
}

function DigitalPinUpdate(pin, value) 
{
  Server.Log("Pin: " + pin + " is " + value);
}

function AnalogPinUpdate(pin, value) 
{  
  var diff = Math.abs(value - lastValue);
  if(pin == "A5" && diff > 3) {
    Server.Log("Pin: " + pin + " is " + value);
    lastValue = value;
  }
}