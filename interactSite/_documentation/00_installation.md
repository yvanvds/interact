---
layout: single
permalink: /documentation/installation/
last_modified_at: 2018-10-21
toc: true
sidebar:
  nav: "docs"
---

## installation

The Interact Server application, which is also used for development, can be downloaded from [this site](/download/). The Android Client application can be found on [Google Play](https://play.google.com/store/apps/details?id=com.mute.interact). Instructions for Raspberry Pi will be available soon.

When you run the server, be sure to start it as administrator. Normally this will happen automatically, asking you if it is OK to do so each time you start it. On first start you will also need to give permission to alter firewall settings. If you don't allow this, clients will be unable to find your server on the local network.

In order for the server and the clients to communicate, they must be part of the same (Wifi) network, and the server must be allowed to handle client requests on this network. If clients cannot connect and you are sure they are on the same network, you can also add firewall exceptions manually. The server uses these ports:

- 11012: (UDP)
- 11234: (UDP / OSC)
- 11235: (TCP/IP)
- 33344: (Multicast UDP)




