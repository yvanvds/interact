---
layout: single
permalink: /documentation/api_interaction_osc/
last_modified_at: 2018-05-09
title: OSC Send & Recieve
toc: true
sidebar:
  nav: "docs"
---

## Description

`OscSender` and `OscReciever` classes can be used to send Osc messages to the server, to other clients, or to external applications on the network.

## OscSender

### . Init
```javascript
void Init(int port)
void Init(string address, int port)
```

Use the `Init` method with port argument to send messages to the local computer on a specified port. Otherwise, specify the Ip Address first.

### . Send
```javascript
void Send(string address, object[] args)
```

Send the content of args to the specified osc address. (Not the Ip adress!)

### . AllowDouble _[bool]_

Wether or not doubles are converted to floats when sent with OSC. Max/MSP does not support the `double` type and disregards incoming data of that type. The default is `true`.

## OscReceiver

### . Register
```javascript
void Register(string address, string callbackFunction)
```

### . Start
```javascript
void Start(int port)
```

### . Stop
```javascript
void Stop()
```
