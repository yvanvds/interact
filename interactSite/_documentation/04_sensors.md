---
layout: single
permalink: /documentation/sensorsintro/
last_modified_at: 2018-10-23
toc: true
sidebar:
  nav: "docs"
title: "Sensors"
---

The great thing about smartphones is that they can contain lots of interesting sensors. What is not so great though, is that there is no standard. Unless you know in advance which devices will participate in your performance, you cannot be sure of anything.

To activate sensors on your Android client, you need to add a new client module of the type `Sensor Configuration`. Open the module and choose the client group `Guests` on top of the page. This is the default group to which all clients belong until you change the group configuration.

## Test your Sensor

All you have to do to use a sensor is activate it and choose a route. If the sensor exists on your device, data will be sent to the route. The easiest way to start is by sending the sensor output to a gui label, which allows you to check on the data it is sending. Go back to your gui screen and add a label. In the properties panel, you can give the label an easy recognizable name. Next, in the sensor configuration, set the route to the `text` endpoint of this label.

When you run the project, the sensor will be activated on your Android device and the output will be visible on the screen.

Once you know the range of values for a particular sensor, you can send them to a more useful endpoint. For instance, a light sensor's output would be a good fit to send to the volume input of the audio patch you've made before. Just be sure to scale down the range of your input. If your proximity sensor works, it should output a 0 or 1. You might want to connect this sensor to the switch input of the audio patch. If set up correctly, the metro object would be activated when your hand is close to your phone.

## Sensor Accuracy

Working with sensors requires a lot of trial and error. Not only are most sensors not supported on every device, the output may also vary. I've seen cheaper phones with a compass that is utterly unstable. The range of a light sensor might be very different on a different phone. And the speed with which some sensors report their data may very from 2 times a second to 20 times a second. 

For these reasons sensor data might best be send to a code module for better interpretation. In most cases it helps if you write some calibration code to your project, but such code would really depend on how you need to use the data. I might add some calibration options for generic use later on, but it is more likely that you'll need to create your own.

