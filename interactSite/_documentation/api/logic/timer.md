---
layout: single
permalink: /documentation/api_logic_timer/
last_modified_at: 2018-05-07
title: Timer class
toc: true
sidebar:
  nav: "docs"
---

## Description

A `Timer` object can be used to call a function on regular intervals. A timer can be started and stopped multiple times during its lifespan.

## Functions
### . Start

```javascript
void Start(string callback, int intervalMilliSec)
```
Starts the timer. The function which names is passed as the first argument will be called at every tick. The second argument (`intervalMilliSec`) defines the speed at which the timer runs, defined in milliseconds.

### . Stop
```javascript
void Stop()
```
Stops the timer, if currently running.

## Properties

## Examples

```javascript
let text = new Title();
var counter = 0;
let timer = new Timer();

function Init() 
{
  Root.Init([1], [1, 1, 1]);

  text.Content = counter;
  Root.Add(text, 0, 0);
  
  var button1 = new Button();
  button1.Content = "Start Timer";
  button1.OnClick("StartTimer");
  Root.Add(button1, 0, 1);

  var button2 = new Button();
  button2.Content = "Stop Timer";
  button2.OnClick("StopTimer");
  Root.Add(button2, 0, 2);
}

function StartTimer() 
{
  timer.Start("OnTimer", 1000);
}

function StopTimer() 
{
  timer.Stop();
}

function OnTimer() 
{
  text.Content = counter++;
}
```