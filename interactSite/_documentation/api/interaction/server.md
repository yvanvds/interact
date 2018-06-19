---
layout: single
permalink: /documentation/api_gui_server/
last_modified_at: 2018-05-09
title: Server object
toc: true
sidebar:
  nav: "docs"
---

## Description
`Server` is a global object available from all client and server code.

## Functions
### . Log
```javascript
void Log(string Message)
```


### . Invoke
```javascript
void Invoke(string FunctionName, object[] args)
```
Invoke a method on the server, with `args` as function arguments.

## Properties
### . Name _[string]_

Get the servers' name.

### . IpAddress _[string]_
Get the servers' IP Address.