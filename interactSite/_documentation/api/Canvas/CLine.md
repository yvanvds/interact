---
layout: single
permalink: /documentation/api_canvas_line/
last_modified_at: 2018-05-09
title: CLine class
toc: true
sidebar:
  nav: "docs"
---

## Description

CLine can be used to draw a line on a [CLine]({{ "/documentaiton/api_canvas_canvas" | relative_url }}) object.

## Constructors
```javascript
CLine()
CLine(Coordinate start, Coordinate end)
```

## Properties
### . Start _[Coordinate]_

Gets or Sets the start position of the line.

### . End _[Coordinate]_

Gets or Sets the end position of the line.

### . Width _[float]_

Gets or Sets the width of the width.

### . Color _[Color]_

The color in which the line will be drawn.