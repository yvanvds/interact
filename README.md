# Interact

Interact is a developer platform for interactive art. It is developed by yvan vander sanden [MuteCode.com]((https://mutecode.com)) with funding by [Musica, Impulse Center for Music](https://www.musica.be) and C-Takt. Interact is open source software.

To learn more about Interact, please refer to the project's [website](https://interact.mutecode.com). You will find a binary download and documentation on that site.

Interact is actually a combination of different projects tied together. There is a windows development application and server, and clients exist for Android and Raspberry PI (using Windows Iot). Projects used inside Interact are:
- [YSE](https://github.com/yvanvds/yse-soundengine): an very performant (3D) sound engine with DSP functionality, currently running on the server and the android client;
- [YAP](https://yap.mutecode.com/): a visual patcher system;
- [OSCGui](https://github.com/yvanvds/OSCGui): a C# Gui library with a focus on audio interfaces;
- [OSCTree](https://github.com/yvanvds/OscTree): a C# routing library to facilitate internal message routing in an OSC like manner.

All the libraries above are developed by yvan vander sanden.

The Interact Server also uses the [Actipro SyntaxEditor](https://www.actiprosoftware.com/products/controls/wpf/syntaxeditor) and the .NET Language Add-on. If you are a developer and you do not have a license for these products, please open the server's project properties - Build page and remove the conditional compilation symbol `WithSyntaxEditor`. This will still allow you to work on most of the code, but not on the parts that require the Code Editor.