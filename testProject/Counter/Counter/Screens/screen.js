let canvas = new CCanvas();
let red = new Color(255, 0 , 0);
let blue = new Color(0, 0, 255);
let touches = new CLayer();

let circle = new CCircle(new Coordinate(0, 0), 100);
let rect = new CRect(new Coordinate(-100, -100), 200, 200);

function Init() 
{
  Root.Init([1], [1]);

  canvas.Init();
  canvas.BackgroundColor = new Color(255,255,255);
  canvas.OnTouchDown("TouchDown");
  canvas.OnTouchUp("TouchUp");
  canvas.OnTouchMove("TouchMove");
  Root.Add(canvas, 0, 0);
  
  let horizontal = new CLine(new Coordinate(-1000, 0), new Coordinate(1000, 0));
  let vertical = new CLine(new Coordinate(0, -450), new Coordinate(0, 450));
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
  
  canvas.Add(touches);
}

function TouchDown(ID, Position) 
{
  let circle = new CCircle(Position, 50);
  circle.Color = new Color(0,0,255);
  touches.Add(ID, circle);
}

function TouchMove(ID, Position) 
{
  let circle = touches.Get(ID);
  if(circle != null) 
  {
    circle.Pos = Position;
  }
}

function TouchUp(ID) 
{
  touches.Remove(ID);
}