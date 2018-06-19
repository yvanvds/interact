---
layout: single
permalink: /documentation/api_gui_grid/
last_modified_at: 2018-05-06
toc: true
title: Grid class
sidebar:
  nav: "docs"
---

## Description

A `Grid` is used as layout container. Multiple rows and columns can defined, with a relative weight. All grid fields can contains exactly one UI element.

Every screen or server interface contains a premade grid object called `Root`. Use this object as the base container for all UI objects.

## Functions

### . Init
```javascript
void Init(int[] Columns, int[] Rows)
```

Initializes the grid with `Columns` and `Rows`. The number of array values determines the number of rows and colums in the grid. Every number on its own indicates the relative weight of this columns or row.

For example: a row definition of `[3, 1]` will result in 2 rows. The first row will take up 75 percent of the screen height, while the second row will take up only 25 percent.

The `Init` function must be called before doing anything else with the grid.

### . Add
```javascript
void Add(View view, int Column, int Row)
```

Assigns a `view` to the indicated position in the grid. Every UI object is a view. This function can be used to add a Button, Image, Slider or even another Grid to the grid position.

### . Remove
```javascript
void Remove(View view)
```

Removes the view from the grid.

## Properties

### . BackgroundColor _[Color]_

Set or Get the current background color of the grid.

## Examples

### A simple Grid

```javascript
function Init() {
  // remember that Root is the main Grid object!
  Root.Init([1], [3, 1]);
  
  var button1 = new Button();
  button1.Content = "Button 1";
  // add the first button to the main Grid
  Root.Add(button1, 0, 0);
  
  var button2 = new Button();
  button2.Content = "Button 2";
  // add the second button to the main grid
  Root.Add(button2, 0, 1);
}
```
![screenshot](/assets/images/simple_grid.png){: .imgRight}
This example will generate a grid with one column and two rows. The top row takes up 75% of the screen. The bottom row 25%.

### A more complex Grid

```javascript
function Init() 
{
  Root.Init([1], [4,1]);
  
  // define nested grid for top row
  var innerGrid = new Grid();
  innerGrid.Init([1,1],[1,1]);
  Root.Add(innerGrid, 0, 0);
  
  // add 4 buttons to nested grid
  var button1 = new Button();
  button1.Content = "Button 1";
  innerGrid.Add(button1, 0, 0);
  
  var button2 = new Button();
  button2.Content = "Button 2";
  innerGrid.Add(button2, 0, 1);
  
  var button3 = new Button();
  button3.Content = "Button 3";
  innerGrid.Add(button3, 1, 0);
  
  var button4 = new Button();
  button4.Content = "Button 4";
  innerGrid.Add(button4, 1, 1);
 
  // add button to bottom row
  var button5 = new Button();
  button5.Content = "Button 5";
  Root.Add(button5, 0, 1);
}
```

![screenshot](/assets/images/complex_grid.png){: .imgRight}
This example shows how you can nest grids to achieve a more complex layout. Because all weights are relative, the layout will scale to any screen size.