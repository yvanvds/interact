---
layout: single
permalink: /documentation/gui/options/
last_modified_at: 2018-10-31
toc: true
sidebar:
  nav: "docs"
---

# Application Options

The application options can be opened from the top menu `Application -> Options`. For now, this dialog has two tabs: _General_ and _Audio_.

## General

- __Server Name__: This can be anything. This name will be visible on the clients, when listing the available servers on the network.
- __Network Token__: If a Network Token is set, clients with the same token will automatically connect to this server on startup.
- Open Last Project on Startup

## Audio

- __Host__: The audio host to be used, such as Asio, Directsound or MME.
- __Device__: The audio device on the current host.
- __Output__: The channel configuration on the current audio device.

Note: This part of Interact is not very well tested. Swapping audio devices on the fly is not easy to implement and partly dependent on hardware. I've only tested this with my own audio device.
{: .notice--info}