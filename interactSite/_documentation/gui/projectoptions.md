---
layout: single
permalink: /documentation/gui/projectoptions/
last_modified_at: 2019-04-15
toc: true
sidebar:
  nav: "docs"
---

# Project Options

The project options can be accessed from the top menu `Project -> Options`. Currently, there are two tabs.

## General

- __Name__: This is the name of the project. It will be shown on the server as well as the client.
- __Client Gui__: This is the first Gui screen that will be displayed on the client when running the project. This is only the default. The startup screen can be assigned for individual groups of clients as well, which overrides this value.

## Resolume
Interact can send OSC messages to Resolume (currently only from scripts). This tab allows you to configure the destination, being the IP-Address and Port of your Resolume application. If running Resolume on the same computer, the local loopback (127.0.0.1) can be used.

