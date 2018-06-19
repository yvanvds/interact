---
layout: single
permalink: /documentation/api_gui_image/
last_modified_at: 2018-05-07
toc: true
title: Image class
sidebar:
  nav: "docs"
---

## Description

An `Image` displays it contents on the screen. It must be placed inside a Grid, and has several options to adapt its size.

## Functions
### . Set

```javascript
void Set(string imageName, ImageMode mode) 
```
Assign a project image to the object. The project image can be referenced by name. There are three modes, which influence how the image will be displayed.

* **ImageMode.Fit:** The image will be shown completely, within the assigned bounds. If the bounds aspect is different from the image aspect, space will be added to the width or height of the image.
* **ImageMode.Fill:** The assigned bounds will be filled with the image. If the bounds aspect is different from the image aspect, part of the image will not be displayed.
* **ImageMode.Stretch:** The image will be stretched to fill the bounds exactly. The original aspect will be discarded.

## Properties

### . Visible _[bool]_

Sets or Gets the visibility of the image.

## Examples

### Basic Example
This is a little example with images. Be sure that the `images` folder contains images with the names provided in the script. 

```javascript
let image1 = new Image();
let image2 = new Image();
let image3 = new Image();
let image4 = new Image();
  

function Init() 
{
  Root.Init([1,2], [2,1]);
  
  image1.Set("tree", ImageMode.Stretch);
  Root.Add(image1, 0, 0);
  
  image2.Set("basket", ImageMode.Fill);
  Root.Add(image2, 0, 1);
  
  image3.Set("fish", ImageMode.Fit);
  Root.Add(image3, 1, 0);
  
  image4.Set("grapes", ImageMode.Fill);
  Root.Add(image4, 1, 1);
}
```

### Image inside Button

Images can also be assigned to [Button]({{ "/documentation/api_gui_button/" | relative_url }}). In which case they will be displayed inside the button.

```javascript
let text = new Title();
var counter = 0;

function Init() 
{
  Root.Init([1], [1,1]);
  
  text.Content = "0";
  Root.Add(text, 0, 0);
  
  let image = new Image();
  image.Set("blueButton", ImageMode.Stretch);
  
  let button = new Button();
  button.Image = image;
  button.OnClick("OnClick");
  Root.Add(button, 0, 1);
}

function OnClick() 
{
  text.Content = counter++;
}
```