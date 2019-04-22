---
layout: single
permalink: /documentation/modules/script/
last_modified_at: 2019-04-22
toc: false
sidebar:
  nav: "docs"
---

# Script

Interact also has an advanced scripting system. Although some basic programming knowledge is needed, it opens up a world of new possibilities. C# is used as a programming language. These documents will not a contain a C# introduction. There is plenty of good course material available to learn how to programming. If you do want to learn C# to be able to create Interact scripts, you don't have to go through a 500 page long manual. Knowing about data types, if-else, for-loops, methods and the basics of classes should be enough to get you started.

## Global Objects
A few global objects are available in Interact Scripts.

### Osc
The `Osc` object enables routing in scripts. Routing data is the core of working with Interact. The `Osc` object has two important methods:
- `SendByID`: can be used to send a message to another endpoint. The route of an endpoint can be found by right clicking on a gui element and and copying it to memory. The method can be used like this.
```csharp
Osc.SendByID("/Root/Client/SQrAsmaEjeXDTk/Led", false);
```
This would turn off a `Led` gui object on all clients with the specific screen active.
- `AddEndpoint`: will define a new endpoint to this script. A function pointer or inline function must be provided. It will be called every time data is passed to this endpoint. An example:
```csharp
Osc.AddEndpoint("calculate", (args)=> {
    Log.AddEndpoint("got message " + args);
});
```

### Client
The `Client` object can be used to obtain information about clients.

- `count`: retrieve the number of active clients.
- `getID`: retrieve the ID of a client with the provided name.
- `getName`: retrieve the Name of a client with the provided ID.
- `getIP`: retrieve the IP address of a client with the provided ID.
- `IDExists`: to check if a given ID exists.
- `NameExists`: to check if a given Name exists.

This object will be useful in situations where you know the clients in the project. When all clients are guests, you will not know the ID or name of these clients.

### Log
The `Log` object is primarily intended for development. It allows you to print messages to the event log on the bottom of the screen.

- `AddEntry` can be used to send a string to the event log.


