---
layout: single
permalink: /documentation/api_gui_button/
last_modified_at: 2018-05-06
title: Button class
toc: true
sidebar:
  nav: "docs"
---

## Description

A `Button` can react on clicks and touches. As with all UI elements it must be placed inside a Grid, and its size will expand to the available space, depending on the Grid definition.

## Functions
### . OnClick

```javascript
void OnClick(string functionName, object[] args)
```
Assigns a function to be executed on every click or touch down on this button. `args` can be used to pass one or more arguments to this function when it is called. This is mainly useful for distinguishing between multiple buttons calling the same function.

### . OnRelease
```javascript
void OnRelease(string functionName, object[] args)
```
Assigns a function to be executed when a button is released. It works with clicks (desktop) as well as touches (mobile).

### . SendOSC
```javascript
void SendOSC(string destination, int port, string address)
```
Changes can also be sent with OSC. The destination should be a valid IP address on which the receiver is listening for OSC messages _(Something like 192.168.0.1)_. The port should be the port on which the receiver is listening _(Something like 4000)_. The address is the OSC route to be used _(For example /mobile/button1)_.

On Android devices, a message will be sent on every pressure change. A non zero value means that the touch is active, while zero is sent when the button is released. 

On other platforms only 0 and 1 are sent, indicating wether or not the button is pressed.

### . SendToPatcher
```javascript
void SendToPatcher(Patcher patcher, string reciever)
```
Send changes directly to a patcher object. This is a bit faster _(and also less typing)_ compared to creating a callback function and using the patcher's `.PassData` functions manually.

This function does not send any data itself, but will relay all future data to the patcher. The reciever string is used to identify the reciever within the patcher.

## Properties

### . Content _[string]_

Sets or Gets the current text, displayed on the button.

### . Image _[Image]_

Assigns an [Image]({{ "/documentation/api_gui_image/#image-inside-button" | relative_url }}) to the button. The image will be displayed inside the bounds of the button object.

### . TextColor _[Color]_

Sets or Gets the current text color. To keep things simple and still have reasonably good looking gui, the border color and text color are always the same.

### . BackgroundColor _[Color]_

Sets or Gets the current background color.

### . Pressure _[Float]_

Gets the current touch pressure. 
Touch screens do not measure the pressure of a touch, but we can get a useable value by measuring the size of the touch. When the user presses harder on the device, the area of contact will be bigger.

This function is not available on iOS!
{: .notice--danger}

## Examples

This script runs as a client screen.

```javascript
let text = new Text();
let button1 = new Button();
let button2 = new Button();
let button3 = new Button();
let button4 = new Button();

function Init() 
{
  Root.Init([1], [1,1,1,1,1]);
  Root.Add(text, 0, 0);

  // add 4 buttons
  button1.Content = "Button 1";
  button1.OnClick("OnButtonClicked", button1);
  button1.OnRelease("OnButtonReleased", button1);
  Root.Add(button1, 0, 1);
  
  button2.Content = "Button 2";
  button2.OnClick("OnButtonClicked", button2);
  button2.OnRelease("OnButtonReleased", button2);
  Root.Add(button2, 0, 2);
  
  button3.Content = "Button 3";
  button3.TextColor = new Color(127, 255, 127);
  button3.BackgroundColor = new Color(0, 0, 0);
  button3.OnClick("OnButton3Clicked");
  button3.OnRelease("OnButton3Released");
  Root.Add(button3, 0, 3);
  
  button4.Content = "Button 4";
  button4.BackgroundColor = new Color(255, 0 ,0);
  button4.SendOSC(Server.IpAddress, 4567, "/demo/button4");
  Server.Log(Server.IpAddress);
  Root.Add(button4, 0, 4);
}

function OnButtonClicked(button) 
{
  text.Content = button.Content + " is on.";
  button.BackgroundColor = new Color(100, 200, 100);
}

function OnButtonReleased(button) 
{
  text.Content = button.Content + " is off.";
  button.BackgroundColor = new Color(50, 100, 50);
}

function OnButton3Clicked() 
{
  text.Content = "Pressure: " + button3.Pressure;
}

function OnButton3Released() 
{
  text.Content = "Pressure: " + button3.Pressure;
}
```
* Button 1 and 2 demonstrate how the same callbacks can be used for different buttons. 

Javascript just passes objects to functions. If you use an argument as a button inside the callback function, be sure to pass a button as an argument. Since Javascript doesn't any type checks, there will be no warnings if you pass a different argument type, but your screen just won't work.
{: .notice--danger}

* Button 3 demonstrates how callback functions without arguments can be used. In this case the button pressure is displayed.

* Button 4 is an example of how to use the OSC functionality of a button. With the server address as an argument, this part of the code will work as long as an OSC receiver is properly defined in de server code.

A basic serverscript implementation could be like this:

```javascript
let text = new Title();

function Init() 
{
  Root.Init([1], [1]);
  Root.Add(text, 0,0);
  
  text.Content = "Waiting for value";
  
  var OSC = new OscReceiver();
  OSC.Register("/demo/button4", "OnOSC");
  OSC.Start(4567);
}

function OnOSC(value) 
{
  text.Content = value;
}
```