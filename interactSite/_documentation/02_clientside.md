---
layout: single
permalink: /documentation/clientside/
last_modified_at: 2018-10-22
toc: true
sidebar:
  nav: "docs"
title: "The Client"
---

Now that you've setup a simple interaction on the server side, you will find it very easy to do something similar for a client. Gui Screens are very important for servers, but even more important on clients. While you could create a project without a server Gui, a client really needs a Gui interface.

Right-click on the `client modules` folder and add a new module. Choose the `Gui Screen` module type and enter a name. The first client gui you create will be set as the default gui for all clients. This can be seen when you double click the project name in the project explorer. The properties explorer will now show the project's properties. Among this is the `Gui on Startup` property, which contains the name of your new gui screen.

Now double-click the gui screen to open it. Client Gui Screens work the same as Server Gui Screens, but they are only half as wide because of smartphone proportions. Use the checkbox to enter edit mode. To keep this tutorial simple, we will just duplicate the server gui we've made before. Just add a button and a slider.

When picking targets, you should select the server audio object again, using the button to control playing and the slider to control volume.

When you're done, disable edit mode and save your project. Now test your new Gui on the server itself. You will find that most client interactions can be tested from within the Gui designer in the server application.

## Connecting a Client

On your Android device, make sure to connect to the same local Wifi network as your server. _(The server might be physically connected to this network if your Wifi router allows this.)_ When you fire up the Interact Client, touch the `Connect` button in the middle of the screen. If no server is shown, try the refresh button or check your firewall settings. Once the server is visible, touch it to connect.

Once connected, the client will show some technical information which might be later on. On the server side you will see some logging about a client connecting to your server. 

If you are on a corporate or school network this might not work. Industrial Wifi allows administrators to disable traffic directly between clients. This is of course very good for network security, but you cannot use Interact on such a network. If you plan to do a performance on such a network, be sure to verify this with the IT people. They might even want to add an exception for your server.
{: .notice--danger}

As soon as your client connects to the server, the server will start sending your project's resources to the client. In this case this is only the Gui Screen you've just created, but a real project can contain much more resources.


## Running the project

To start the project on the client, you must click the play button on the server's toolbar. The client will verify that all resources are at the latest version, requesting new versions if needed. After that (this should take only a second), the main gui screen will show up on the client.

Test the client gui to verify that your sound can now be controlled from the client side.

Now go back to edit mode on the client gui and change a few colors. Next, leave edit mode again. For most changes, leaving edit mode will be sufficient to notify the client of a new version of this gui. Stop and restart your project. The client will re-download your gui screen and display the new version.