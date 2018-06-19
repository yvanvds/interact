---
layout: single
permalink: /documentation/api_canvas_layer/
last_modified_at: 2018-05-09
title: CLayer class
toc: true
sidebar:
  nav: "docs"
---

## Description

CLayer is an object container for a [CCanvas]({{ "/documentaiton/api_canvas_canvas" | relative_url }}) object.

Just as with a CCanvas, objects added last will be drawn on top. The advantage of layers is that by adding layers to a canvas, you can decide in which order to draw objects by putting them in the desired layer.

## Functions

### . Add
```javascript
void Add(int ID, CDrawable object)
```

### . Remove
```javascript
void Remove(int ID, CDrawable object)
```

### . Get
```javascript
CDrawable Get(int ID)
```