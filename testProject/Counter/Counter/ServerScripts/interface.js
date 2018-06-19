let canvas = new CCanvas();

let red = new Color(255, 0 , 0);
let blue = new Color(0, 0, 255);

let circle = new CCircle(new Coordinate(0, 0), 10);

let rect = new CRect(new Coordinate(-20, -20), 40, 40);

var button1 = new Button();

function Init() 
{
  Root.Init([1], [5, 1, 1]);

  canvas.Init();
  canvas.BackgroundColor = new Color(255,255,255);
  canvas.OnTouchDown("MouseDown");
  canvas.OnTouchUp("MouseUp");
  canvas.OnTouchMove("MouseMove");
  
  Root.Add(canvas, 0, 0);
  
  let horizontal = new CLine(new Coordinate(-1000, 0), new Coordinate(1000, 0));
  let vertical = new CLine(new Coordinate(0, -500), new Coordinate(0, 500));
  horizontal.Color = new Color(100, 100, 100);
  vertical.Color = new Color(100, 100, 100);
  horizontal.Width = 4;
  vertical.Width = 4;
  canvas.Add(horizontal);
  canvas.Add(vertical);
  
  rect.Fill = true;
  rect.Color = new Color(0,255,0);
  canvas.Add(rect);
  
  circle.Fill = false;
  canvas.Add(circle);
  
  
  button1.Content = "Red";
  button1.OnClick("SetColor", red);
  Root.Add(button1, 0, 1);

  var button2 = new Button();
  button2.Content = "Blue";
  button2.OnClick("SetColor", blue);
  Root.Add(button2, 0, 2);
}

function SetColor(arg) 
{
  rect.Color = arg;
}

function MouseDown(button) 
{
  if(button == 0) 
  {
    circle.Fill = true;
  }
}

function MouseUp(button) 
{
  if(button == 0) 
  {
    circle.Fill = false;
  }
}

function MouseMove() 
{
  circle.Pos = canvas.Mouse;
  var pos = canvas.Mouse;
  pos.X -= 20;
  pos.Y -= 20;
  rect.Pos = pos;
}