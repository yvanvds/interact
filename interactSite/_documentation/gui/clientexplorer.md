---
layout: single
permalink: /documentation/gui/clientexplorer/
last_modified_at: 2018-10-31
toc: true
sidebar:
  nav: "docs"
---

The client explorer (`Window -> Client Explorer`) can be used to view the active clients on the server.

New clients will appear as Guests. New groups can be added if needed. 

## Groups

Groups provide you with a flexible system to define message routes and more. Here are a few features:

- Each group can be assigned its own startup gui screen. This is done by double clicking the name of the group and changing this value in the properties window on the right.
- When assigning a client side data route on a (gui) element, there is an option to send the data to a specific group instead of sending it to all clients.
- Each group can have its own sensor configuration.
- Scripts can access group information and make decisions based on this.

## Managing Groups

New clients always appear in the `Guests` group. The can be dragged to other groups. The assignment is instant, but not all parts of the project will instantly act on it. For instance, the startup gui will only be changed on the next project start.

Clients in custom groups will be stored with the project. This means you will see them in a disconnected state when the project is opened. If they have a server token configured, they will automatically connect to the project and put back in their group. Clients within the `Guests` group will not be stored within the project.

