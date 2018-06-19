---
layout: single
permalink: /documentation/api_canvas_canvas/
last_modified_at: 2018-05-09
title: CCanvas class
toc: true
sidebar:
  nav: "docs"
---

## Description

For scenarios where the basic UI with buttons and such does not provide enough control, or when you need a multitouch interface, Interact provides a vector based drawing system.

A `CCanvas` is the base for this system. It is a View element just like any other, which means that it can take up the whole screen or only part of it.

Basic shapes can be drawn upon the canvas. On a desktop device the mouse actions can be intercepted. On mobile devices, multiple touches can be tracked within the canvas.

## Functions
### . Init

```javascript
void Init()
```

Initializes the canvas.

### . Add

```javascript
void Add(CDrawable object)
```
Adds a drawable object to the canvas. These are objects like [CCircle]({{ "/documentaiton/api_canvas_circle" | relative_url }}), [CLayer]({{ "/documentaiton/api_canvas_layer" | relative_url }}), [CLine]({{ "/documentaiton/api_canvas_line" | relative_url }}) and [CRect]({{ "/documentaiton/api_canvas_rect" | relative_url }})

The hierarchy between canvas objects is such that the first added object is drawn behind later objects. The last added object is drawn on top.

For a more flexible ordering of elements, a [CLayer]({{ "/documentaiton/api_canvas_layer" | relative_url }}) can be used.

### . Remove

```javascript
void Remove(CDrawable object)
```
Removes a drawable object from the canvas.

### . OnTouchDown

```javascript
void OnTouchDown(string functionName)
```
Registers the callback function to be called when a mouse button is pressed or a new touch is detected.

The callback function should expect one or more arguments.

- on the server: one integer argument, indicating which mouse button was pressed. The left button sends a 0, the right button a 1.

- on the clients: one integer argument, providing the touch ID. This ID can be used to track the movement of the touch. Also a second argument of the type Coordinate is passed to the callback function, indicating the current position of this touch.

When working with touches, it is important to familiarize yourself with the way touch ID's work. ID's are reused and normally start from zero. This means that the first touch will get ID 0. When a second touch is added, it will get ID 1. When the first touch is now released, ID 0 will no longer exist but ID 1 still does. There will be no touch with ID 0 at that point.

### . OnTouchMove

```javascript
void OnTouchMove(string functionName)
```

Registers the callback function which is to be called when a touch or mouse is moved. The server version does not expect any arguments, because the mouse position can be requested with the Mouse property.

The client version passes the touch ID and Position as arguments, just like in the `OnTouchDown` method.

### . OnTouchUp

```javascript
void OnTouchUp(string functionName)
```

Registers the callback function which is to be called when a touch ends, or when a mouse button is released.

Arguments work in the same way as they do with the other callback functions.

## Properties

### . BackgroundColor _[Color]_

Sets or Gets the current background color.

### . Mouse _[Coordinate]_

Gets the current mouse position. Only works on the server platform.

### . Width _[float]_

Gets the current width of the canvas, in pixels. This only works when the canvas has been drawn at least once and cannot be called during screen initialisation.

### . Height _[float]_

Gets the current height of the canvas, in pixels. This only works when the canvas has been drawn at least once and cannot be called during screen initialisation.

## Canvas Coordinates

When drawing objects on a canvas, coordinates must be given. The coordinate system scales according to the height of the canvas. 

The center of a canvas has the coordinate `(0,0)`. The vertical range will be between -500 and 500. -500 being the bottom of the screen, 500 being on top.

The horizontal range is dependent on the width of the screen, with the vertical scaling applied. A negative number will be on the left, a positive number on the right.