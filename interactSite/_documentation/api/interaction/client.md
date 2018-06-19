---
layout: single
permalink: /documentation/api_interaction_client/
last_modified_at: 2018-05-11
toc: true
title: Client class
sidebar:
  nav: "docs"
---

## Description

The `Client` class contains information about a client device in the network. In most cases you won't create objects of this type, but rather retrieve an object from the `Clients` objects.

## Functions
### . Invoke
```javascript
void Invoke(string functionName, object[] args)
```

### . StartScreen
```javascript
void StartScreen(screen name)
```

## Properties
### . IpAddress _[string]_
### . ID _[string]_
### . Name _[string]_
### . LocalName _[string]_

