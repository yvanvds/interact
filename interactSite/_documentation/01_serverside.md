---
layout: single
permalink: /documentation/serverside/
last_modified_at: 2018-10-21
toc: true
sidebar:
  nav: "docs"
title: "The Server"
---

Please start with installing at least the Server application. Preferably you should also install the Android client on your phone. Installation instructions can be found [here](/documentation/installation/).

After running the server application, you will need to create a new project (`Project -> New`). The project explorer will now show your new project, containing two empty folders:

- Server Modules: this folder will contain all server-side modules.
- Client Modules: the contents of this folder will be sent to all clients.

In this guide, we will add some simple interactions to your project. This will teach you the basics concepts and workflow of Interact. You should read the full documentation afterwards to fully understand the possibilities.

_The text below the next video is a transcript of this video._

<iframe src="https://www.youtube.com/embed/DG1ycqNZxtg" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>


## Add a Sound Page

Right-click the server module folder and add a new module. From the drop-down list with module types, select `Sound Page`. Give your module a name. When the module is created, double-click to open it.

Now add a new sound to the page. You can use `.ogg` and `.wav` files, but `.mp3` is not implemented right now. You should test your sound by playing it.

## Add a Gui Screen

Next you should add a Gui. Right-click the server module folder again and add another module. This time, choose `Gui Screen`. After creating the screen, double-click  it to open.

To add Gui Elements to the screen, you must enable edit mode. This is done with the checkbox in the top left of the screen. Next, right-click the upper left placeholder and choose `Button` from the available elements. Next, click on the button to select it and see its properties in the properties window on the right.

In the properties window, with the button selected, enable `Is Toggle` to make this a toggle button. Also, change the `Text` property to "play". Next, click on the ellipsis next to `Targets`. This will open the `Route Inspector`. On the right click on `add`. This will show the `Route Selector`.

## The Route Selector

All interactions between elements in Interact are defined by routes. A route defines to which element an action will be sent. For a toggle button, this means the current on/off state will be sent to the target of the route.

All possible destinations are displayed in a tree format. The two main trees are `server` and `client`. These trees are a darker shade of green, which indicates they are trees. Click the server tree to display its contents. Now, you should select the subtree `sounds`. This tree should contain one more subtree, with the name of your sound page. Once you click that, you will see an object with a lighter shade of green. This color is used for objects, like your sound.

Once you click the sound object, you will see some yellow rectangles. These are `endpoints`. An object can have several endpoints, depending on the object type. Select the endpoint called `play`. A route must always end with and endpoint.

Once the endpoint, the `ok` button in the lower left corner can be selected. Click it. Since we don't need to define other endpoints for this button. The route inspector can now be closed.

If you turn off edit mode for this gui screen, you can start and stop the sound by pressing the play button. Congratulations, you've made your first interaction!

## A Volume Slider

Go back to edit mode on your Gui screen. This time, hold the CTRL button and drag your mouse (while the left button is down) until you select tree placeholders in a row. Next to the `Edit` checkbox, you can see a `Merge` button. Click it to merge the three placeholders into one.

Right-click the new big placeholder and select `Slider`. Next, click on the slider to display its properties. We will use this slider to control the sound's volume, which is defined as a value between 0 and 1. Currently the `Maximum` value of the slider is 100. Use the properties to change this to 1. Next, click on the `Targets` button again to define the route for this slider. Find the route to your sound and select the endpoint called `Volume`.

Exit edit mode and test your new slider while playing the sound.

Another good choice for a volume control would be a `Knob`. This element works best if you merge 4 placeholders in 2 rows and 2 columns. I bet you can figure out how to connect it to your sound by now.
{: .notice--primary}



