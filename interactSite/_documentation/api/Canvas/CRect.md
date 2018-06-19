---
layout: single
permalink: /documentation/api_canvas_rect/
last_modified_at: 2018-05-09
title: CRect class
toc: true
sidebar:
  nav: "docs"
---

## Description

CRect can be used to draw a rectangle on a [CCanvas]({{ "/documentaiton/api_canvas_canvas" | relative_url }}) object.

## Constructors
```javascript
CRect()
CRect(Coordinate position, float width, float height)
```

When arguments are used, the position is the bottom left position of the circle.

## Properties
### . Pos _[Coordinate]_

Gets or Sets the position of the rectangle. This is the bottom left corner.

### . Width _[float]_

Gets or Sets the width of the rectangle.

### . Height _[float]_

Gets or Sets the height of the rectangle.

### . Color _[Color]_

The color in which the rectangle will be drawn.

### . Fill _[bool]_

Switch between outline or filled drawing.