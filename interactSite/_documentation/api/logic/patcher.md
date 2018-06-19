---
layout: single
permalink: /documentation/api_logic_patcher/
last_modified_at: 2018-05-07
title: Patcher class
toc: true
sidebar:
  nav: "docs"
---

## Description

A `Patcher` object creates an instance of a Patcher. The patcher is opened in the background, but if executed on the server, the Gui can still be opened manually. _(Which comes in handy during debugging.)_

## Functions
### . Load

```javascript
void Load(string patcherName)
```
Loads the patcher by name. The patcher must exist in the project for this function to succeed. 

If multiple patcher objects open the same named patcher, both objects will point to the same patcher.
{: .notice--danger}

### . EnableAudio
```javascript
void EnableAudio()
```
Enables audio generation for this patcher.

### . DisableAudio
```javascript
void DisableAudio()
```
Disables audio generation for this patcher.

### . PassBang
```javascript
void PassBang(string to)
```
Pass a _bang_ to this patcher. Patchers can have `.r name` _(receive)_ objects. The argument `to` should be set to the name used with the receiver. 

Also note that multiple recieves can have the same name. If so, the _bang_ will be sent to all of them.
{: .notice--info}

### . PassInt
```javascript
void PassInt(int value, string to)
```
Similar to `PassBang`, but sends an integer value.

### . PassFloat
```javascript
void PassFloat(int value, string to)
```
Similar to `PassBang`, but sends an float value.

### . PassString
```javascript
void PassString(int value, string to)
```
Similar to `PassBang`, but sends a string value.

## Properties

This class does not have any properties.

## Examples

```javascript
let slider = new Slider();

function Init() 
{
  Root.Init([1], [1]);
  
  let patcher = new Patcher();
  patcher.Load("test");
  patcher.EnableAudio();
  
  slider.SendToPatcher(patcher, "volume");
  Root.Add(slider, 0, 0);
}
```
The patcher which goes with this example looks like this. Note that the float object on the right is just used to double check on the incoming slider value.

![patcher example](/assets/images/patcher_example.png)
