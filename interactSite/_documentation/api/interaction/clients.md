---
layout: single
permalink: /documentation/api_interaction_clients/
last_modified_at: 2018-05-04
toc: true
title: Clients object
sidebar:
  nav: "docs"
---

## Description

The `Clients` object is a global object available to server and client code. It contains a reference to all current clients in the network.

## Functions
### . GetLocal()
```javascript
Client GetLocal()
```
Gets a reference to the local client. This method doens't exist within server side code.

###  . Invoke
```javascript
void Invoke(string FunctionName, object[] args)
```
Invokes a function on all available clients.

### . StartScreen
```javascript
void StartScreen(string name)
```
Start a screen on all available clients.

## Properties
### . Count _[int]_
Gets the number of available clients.

### . [i] _[Client]_ 
Gets the i-th client.

