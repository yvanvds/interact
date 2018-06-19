---
layout: single
permalink: /documentation/api_canvas_circle/
last_modified_at: 2018-05-09
title: CCircle class
toc: true
sidebar:
  nav: "docs"
---

## Description

CCircle can be used to draw a circle on a [CCanvas]({{ "/documentaiton/api_canvas_canvas" | relative_url }}) object.

## Constructors
```javascript
CCircle()
CCircle(Coordinate pos, float size)
```

## Properties
### . Pos _[Coordinate]_

Gets or Sets the position on the canvas.

### . Size _[float]_

Gets or Sets the size of the circle.

### . Color _[Color]_

The color in which the circle will be drawn.

### . Fill _[bool]_

Switch between outline or filled drawing.