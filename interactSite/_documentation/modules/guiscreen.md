---
layout: single
permalink: /documentation/modules/guiscreen/
last_modified_at: 2018-10-31
toc: true
sidebar:
  nav: "docs"
---

# Gui Screen

Interact provides simple gui screens on both client and server side. They are not intended as a fully flexible system, but instead try to keep designing as well as using a gui simple and intuitive. Every screen has a basic grid with placeholder blocks. To see these blocks, enable edit mode.

Once in edit mode, you can right-click a placeholder to replace it with a gui component. It is also possible to replace an existing gui element this way. _(Warning: There is no undo button right now!)_ If an element needs to be bigger, it is possible to merge some placeholders together. Merged placeholders can also be split again, but no further than the original placeholder size.

The placeholders on the client take up only half of the screen. This is intentional, because it accomodates the screen size of a smartphone.
{: .notice--info}

To change the default properties of a gui element, select it by clicking on it in edit mode. This will show its properties in the properties window. Every gui element has its own properties. Below is a description of the currently available elements. Properties starting with a `+` sign can be changed by using them as Osc Route endpoints.

## Shared with all elements

- `Name`: The name of the element. Change this to keep track of what does what. This name is also displayed in the Osc Route selector. Changing it will not affect the route itself because internal unique names are used in the background.
- `+Visible`: If the element is visible or not.
- `Targets`: The targets of this element. Multiple targets are possible. Data is sent on every change. _(Exceptions are labels and leds, which can not have any targets.)_

## Button
- `+Background`: The background color. Dark Grey by default.
- `+Color`: The color used for the button decoration.
- `Is Toggle`: If in toggle mode.
- `+Text`: The text shown inside of the button.
- `+TextScale`:

### Output 
If in toggle mode, the button will output 0 or 1 when the state is changed. If not, it outputs a trigger.

## Knob
A knob is essentially a slider, but it looks very different.

- `+Color`: The indicator color
- `+Show Value`: Wether or not the current value is shown in the knob.
- `Text`: Optionally a tet can be shown in the middle of the control.
- `+Minimum`: The smallest value, on the left of the knob.
- `+Maximum`: The largest value, on the right of the knob.
- `+Value`: The current value.

### Output
A float value, scaled between minimum and maximum, depending on the knob position.


## Label
- `+Background`: The background color. Transparent by default.
- `+Color`: The text color.
- `+FontSize`
- `FontStyle`: Normal or Italic
- `FontWeight`: Several options, like Bold, Thin, etc. 
- `+Text`: The text shown in the label. 
- `Alignment`: Text alignment. _(Left, Right, Center or Stretched)_

### Output
None





## Led
A led can blink for a short time. When a led receives a trigger, it will blink for 100 milliseconds. When it receives a number, it will blink for that number of milliseconds.

- `+Color`: The color of the light
- `Size`: A float between 0 and 1, which scales to the available space.

### Output
This element does not output values.

## Slider
Sliders will change direction automatically, depending on height-width proportions.

- `Accent`: The accent color changes the color of the _handle_ on the slider.
- `+Background`: The color on the non active part of the slider.
- `+Color`: The color indicating the value of the slider.
- `+Minimum`: The smallest value, on the left or bottom of the slider.
- `+Maximum`: The largest value, on the right or top of the slider.
- `+Value`: The current value.

### Output
A float value, scaled between minimum and maximum, depending on the slider position.

## Textbox
A textbox allows you to input text and send it somewhere.

- `+FontSize`
- `+Text`: (OSC input only) set the current text

### Output
A string value, triggered when you leave the textbox.

## XY-Pad
Can be used when you need to output a value on a two-dimensional axis.

- `+Border`: The border color
- `+Color`: The indicator color
- `+Centered`: Determines if the zero position is either in the center or in the left lower corner.
- `ShowValue`: When true, the current value is displayed in the upper right corner.
- `+Coordinate`: (OSC input only) set the current position

### Output
2 floats, representing the current X and Y value of the element's indicator.

## Multi Touch Pad
Works on touch screens only, and tracks multiple touches on the screen. On some screens this element can also fake the amount of presure, by looking at the size of contact.

- `+Background`: The background color
- `+Color`: The color of the active touches, displayed as circles

### Output
an array with the touch ID (integer), the pressure of the touch(float) and X and Y position (floats). If pressure is not recorded, it will be one. When the touch is released, a pressure of 0 will be sent, without the X and Y position.

