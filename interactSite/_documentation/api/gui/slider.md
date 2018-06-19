---
layout: single
permalink: /documentation/api_gui_slider/
last_modified_at: 2018-05-09
title: Slider class
toc: true
sidebar:
  nav: "docs"
---

## Description

A `Slider` must be placed inside a Grid.

## Functions
### . OnChange

```javascript
void OnChange(string functionName, object[] args)
```
Assigns a function to be executed on every change to this. `args` can be used to pass one or more arguments to this function when it is called. This is mainly useful for distinguishing between multiple objects calling the same function.

### . SendOSC
```javascript
void SendOSC(string destination, int port, string address)
```
Changes can also be sent with OSC. The destination should be a valid IP address on which the receiver is listening for OSC messages _(Something like 192.168.0.1)_. The port should be the port on which the receiver is listening _(Something like 4000)_. The address is the OSC route to be used _(For example /mobile/slider1)_.

### . SendToPatcher
```javascript
void SendToPatcher(Patcher patcher, string reciever)
```
Send changes directly to a patcher object. This is a bit faster _(and also less typing)_ compared to creating a callback function and using the patcher's `.PassData` functions manually.

This function does not send any data itself, but will relay all future data to the patcher. The reciever string is used to identify the reciever within the patcher.

## Properties

### . Minimum _[float]_

### . Maximum _[float]_

### . Value _[float]_ 