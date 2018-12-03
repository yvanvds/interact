---
layout: single
permalink: /documentation/varia/pi/
last_modified_at: 2018-11-27
toc: true
sidebar:
  nav: "docs"
---

# Raspberry PI Client

## Installation

The Interact Client also runs on Raspberry PI 3 (Tested with Model B+). Installation is not that hard, but it does take a few steps:

1. Install the latest windows IoT system on the PI. You need the Windows 10 IoT Core Dashboard for this. Please follow instructions as provided by [Microsoft](https://docs.microsoft.com/en-us/windows/iot-core/downloads).
2. When the device is up and running verify its state by connecting a screen. Now start the device portal from the IoT Dashboard. (Right click the device entry, and pick `Open in device Portal`).
3. Unpack the zipfile with the Interact Client for UWP.
4. In the device Portal, go to `Apps -> Apps Manager`.
5. Click on `Install Certificate`.
6. Install the certificate provided with the Client.
7. Click on `Local Storage`.
8. Upload the `.appxbundle` file, and select `allow me to select framework packages`.
9. Add the `.appx` files in `Dependencies\arm`.
10. Install everything. Try running the application. If everything works, you might want to select the InteractClient as the Startup application.

_(Developer note: when you create your own version of the interact client, be sure __not__ to set the build option `Compile with .NET native toolchain`. This setting is not compatible with Windows IoT and will cause the app to crash on launch.)_

## Arduino Shield

While in some instances a Raspberry PI might cover your needs, it does not contain any sensors by default. A special Arduino Shield for the Raspberry PI can be mounted on top. This allows you to connect all sorts of sensors (and also outputs) to the system. I've tested this with the [Gravity Arduino Shield by DFRobot](https://www.dfrobot.com/product-1211.html).

Before connecting your shield to the Raspberry PI, you should install the [Firmata](https://github.com/firmata/arduino
) library on the Arduino. This is a preset option in the Arduino IDE, so I'm not going to cover that here. Once that is done, the shield should be mounted on the the Raspberry PI and connected with USB.

Next time you start the Interact Client, open the options page and configure your Arduino. Once you've connected a few sensors, you can setup routing from the Interact Server.