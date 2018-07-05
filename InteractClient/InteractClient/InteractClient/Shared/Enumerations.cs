using System;

namespace Shared
{
    public static class Constants
    {
        public const int UdpPort = 11012;
        public const int TcpPort = 11013;
        public const int MulticastPort = 33344;
        public const String MulticastAddress = "239.192.0.1";
    }

    enum NetworkMessage
    {
        EndOfMessage, // sent by client and server to indicate end of message
        Acknowledge, // sent by server to acknowledge presence
        RequestAcknowledge, // sent by server. client has to respond with IamOnline
        IamOnline, // sent by client after server request
        RequestConnection, // sent by client to server when trying to connect
        Disconnect, // sent by client to notify server of disconnect
        SetID, // sent by server to instruct client this is its ID
        SetUserName, // sent by client as a reply to SetID
        SetCurrentProject, // sent by server to client, this determines the project for all project related messages that follow
        StoreScreen, // sent by server to update screen code
        StartScreen, // sent by server to activate screen
        StopScreen, // sent by server to stop the current screen
        JavaException, // sent by client to server if there is a code error
    }
}
