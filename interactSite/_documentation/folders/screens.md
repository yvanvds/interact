---
layout: single
permalink: /documentation/screens/
last_modified_at: 2018-05-03
toc: true
sidebar:
  nav: "docs"
---

## The Screens Folder

The contents of this folder will always be rendered/executed on the client side. It may contain Scripted Screens or Utility scripts. In the next version of interact, visual aids for creating these scripts will be added to this.

## Scripted Screens
The scripted screen item is the most flexible way to create a client side GUI, but you must write the code yourself. At the very least, these scripts should include an `Init()` function with a `Root` initialisation inside.

```javascript
function Init()
{
  Root.Init(1, 4);
}
```
The `Init()` function will be called every time the screen is started.

The `Root` is a predefined Grid object. With its `Init` function you assign the number of columns (1) and rows (4) in the grid. Every other Gui element must be assigned its place in this grid to be visible on the client.

Here's simple example with a `Text` and `Button` object.

```javascript
var button;
var text;
var counter = 0;

function Init() 
{
  Root.Init(1, 2, true);
  
  text = new Text(); // Create a Text object
  text.Content = counter; // Assign the initial text value
  Root.Add(text, 0, 0); // Add to Gui
  
  button = new Button(); // Create a Button object
  button.Content = "Click Me"; // Text in button
  button.OnClick("OnButtonClicked"); // connect a function to click or touch
  Root.Add(button, 0, 1); // Add to Gui
}

// this function will be called every time the user clicks or touches the button
function OnButtonClicked() 
{
  counter++; // increase by one
  text.Content = counter; // assign value to Text object
}
```

## Utility Scripts
Another type of scripts which can be added to the screens folder are Utility Scripts. These scripts can contain any javascript you like. 

When executing your project, the code in these files will be merged with every screen that is started. Which makes them ideal for code you might need in every (or more than one) screen in your project.

## More about coding Screens
If you're not familiar with javascript, I'd recommend you follow a javascript course first. Knowing the language you are working with makes everything much easier. One of the reasons Interact uses javascript is that is a pretty easy language to learn.

Please consult the Javascript part of the documentation to learn more about the different objects that are exposed by Interact.