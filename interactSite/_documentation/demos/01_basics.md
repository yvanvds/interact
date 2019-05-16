---
layout: single
permalink: /documentation/demos/demo01/
last_modified_at: 2019-04-19
toc: true
sidebar:
  nav: "docs"
---

## Demo 1 (Gui Interactions)

[Download](https://firebasestorage.googleapis.com/v0/b/mute-interact.appspot.com/o/Demos%2FDemo01.zip?alt=media&token=3537484d-7810-4ea7-97d0-b915b2bff089)

This demo shows how to control client gui widgets from a server gui. Although hardly enough for any project, these basics are needed for most. A few things to look for when you check out this project:
- The project uses 1 server and 1 client gui.
- All widgets in the client gui that are controlled from the server gui have custom names. This makes it easier to find them when setting up routes.
- Open the server gui in edit mode, click on a widget and look at the properties on the right. Open op the targets property to see each route.
- Note that you are not restricted to one route for a target. For example, the knob and button values are sent to multiple targets at once.
- When you open a target with the edit button, please note that de default path is adjusted to send this value to all clients.

## Demo 2 (Playing Sound)

[Download](https://firebasestorage.googleapis.com/v0/b/mute-interact.appspot.com/o/Demos%2FDemo02.zip?alt=media&token=06309500-5901-4c06-a5e1-e855de9e57d8)

This Demo enables clients to trigger and control sounds on the server. It has a sound page on the server side, and a client gui. Again, the same gui is sent to all clients. 
- The 2 buttons on the left trigger the first 3 sounds. If you open the Targets window, you will see how they are are constructed. Because the targets are on the server side, there is no need to change the path like we did in the previous demo.
- The routes end with the `restart` option. It would have been valid to use the `play` option instead, but by using `restart` we will also start the sound again, from the beginning, when it is still playing.
- The 4th sound can be started and stopped and shows a few other controls. The knob is linked to the speed, the slider to the volume. Panning is easily controlled from an XY-Pad.

## Demo 3 (Switching Screens I)

[Download](https://firebasestorage.googleapis.com/v0/b/mute-interact.appspot.com/o/Demos%2FDemo03.zip?alt=media&token=49b056e2-65b7-4818-809a-1f5e30128966)

This demonstrates how to control which screen is shown on the clients. The project contains one server Gui and 3 client Gui's.
- Each client gui shows only a label to identify the current screen.
- There are three columns with buttons on the server Gui, used to switch between screens on the clients.
- When you want to activate a screen, use the `Activate` endpoint of the screen when selecting a target. Changing the path to all clients will trigger the screen switch on all connected clients.
- The project also has two groups setup. To see how a screen can be sent to only a group of clients, you need to open `Window -> Client Explorer` and move clients to both groups.
- When you look at the target for the group buttons, you will see that the path is set to a specific group. It is important to define groups before setting the target.

## Demo 4 (Switching Screens II)

[Download](https://firebasestorage.googleapis.com/v0/b/mute-interact.appspot.com/o/Demos%2FDemo04.zip?alt=media&token=40d96cc6-7a8d-4650-b10e-57df03604c87)

This demonstrates multiple client screen, without server control. All clients can switch back and forth between screens on their own.
- This project does not have any server side content.
- The target path for all buttons is left at the default value, which propagates changed to the local client only.
- If you need to provide different screens to different groups, start with the previous demo. You should create different starting screens and trigger them from a server gui. On the starting screen for each group, you can provide buttons to move to other screens.