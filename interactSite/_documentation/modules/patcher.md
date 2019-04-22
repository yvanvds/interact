---
layout: single
permalink: /documentation/modules/patcher/
last_modified_at: 2019-04-20
toc: true
sidebar:
  nav: "docs"
---

# Audio Patcher

The Patcher system can be used for DSP audio, but also for controlling routes in a visual manner. If you have worked with Max/MSP or Pure Data before, you will feel right at home. 

## Edit Mode
To add blocks to a patcher, you should enable edit mode. The can be done with the switch on top of the screen, or by pressing `CTRL+E`. The same shortcut can be used to leave edit mode.

In edit mode, you can add and remove blocks at will. Adding blocks can be done with the shortcut `CTRL+A`. Remove them by selecting a block _(or a connection)_ and pressing `BACKSPACE`.

Connections between blocks can be made by clicking on a block outlet (the dot at the bottom) and dragging it to the inlet of another block. In most cases, the leftmost inlet is active. This means that when a message arrives at this inlet, it will trigger calculations and output a value at the outlet.

DSP blocks are audio blocks and will output values continuously. All blocks starting with a `~` are DSP blocks. Blocks starting with a dot are normal data blocks.

## Available Blocks
The Interact sound engine and its patcher system are independent projects. A list of available blocks can be found in the documentation section of the [YAP website](https://yap.mutecode.com/docs/).

## Routing
Unique to Interact are the routing blocks. You can receive data from other parts of Interact by using a `.r name` block. The name should be a meaningful value, indicating what this route is for. This is all you need to setup a route endpoint to your patch. Once done, you can open the router on a gui element end select this endpoint. You can also use this route from a script.

Sending data to another route can be done with the `.s /name/of/route` block. _If you need to know the name of route to a gui element, you can right click on the element (when not in edit mode) and select the desired route to copy it to memory._ 

