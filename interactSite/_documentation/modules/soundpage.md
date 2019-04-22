---
layout: single
permalink: /documentation/modules/soundpage/
last_modified_at: 2019-04-22
toc: false
sidebar:
  nav: "docs"
---

# Sound Page
This module can be used to add sound files to your project. Right now, it supports `.wav` and `.ogg` files. Due to very complicated reasons it will not work on clients right now, but this will be implemented soon.

Sounds can be added with the green `Add Sound` button. This will copy the selected file to the project directory. Limited functionality is available from this page itself, which is intended for testing purposes. Sounds can be the target of other elements though, allowing the manipulation of a lot more values, like volume, speed and position.

When you need two instances of the same sound, you should use the copy button of the first sound. This will allow Interact to open just a single audio file with multiple pointers to it, thus saving memory.
{: .notice--info}

## Sound Properties
A lot of sound modifiers are available as route targets. Here's a current list:

- `play`: Starts playing the sound. If the sound was paused, it will resume from its current location.
- `restart`: This will start playing the sound from the beginning, regardless of where it was paused.
- `stop`: Stop playing and reset the position to the beginning of the file.
- `pause`: Stop playing the sound without position reset.
- `time`: Skip the playhead to a specific position in the sound file.
- `volume`: The volume at which the sound is played.
- `speed`: The speed at which the sound will be played. To play a sound backwards, use a negative value.
- `loop`: Determines wether or not the sound will loop.
- `pos`: The sound's position on an x/y axis. This implements panning. Sounds normally sound at position (0,0). Use the x-axis to pan from left to right. If a surround system is used, the y-axis can be used to send a sound forward and back.
