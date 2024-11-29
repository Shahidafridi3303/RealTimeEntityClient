using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkClientProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from pipeline = " + pipeline);

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        // if (signifier == ServerToClientSignifiers.asd)
        // {

        // }
        // else if (signifier == ServerToClientSignifiers.asd)
        // {

        // }

        //gameLogic.DoSomething();

    }

    static public void SendMessageToServer(string msg, TransportPipeline pipeline)
    {
        networkClient.SendMessageToServer(msg, pipeline);
    }

    #endregion

    #region Connection Related Functions and Events
    static public void ConnectionEvent()
    {
        Debug.Log("Network Connection Event!");
    }
    static public void DisconnectionEvent()
    {
        Debug.Log("Network Disconnection Event!");
    }
    static public bool IsConnectedToServer()
    {
        return networkClient.IsConnected();
    }
    static public void ConnectToServer()
    {
        networkClient.Connect();
    }
    static public void DisconnectFromServer()
    {
        networkClient.Disconnect();
    }

    #endregion

    #region Setup
    static NetworkClient networkClient;
    static GameLogic gameLogic;

    static public void SetNetworkedClient(NetworkClient NetworkClient)
    {
        networkClient = NetworkClient;
    }
    static public NetworkClient GetNetworkedClient()
    {
        return networkClient;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion

}

#region Protocol Signifiers
// Client-to-Server Signifiers (sent from client to server)
static public class ClientToServerSignifiers
{
    public const int BalloonPopped = 1; // Client notifies the server when a balloon is popped
    public const int RequestUnpoppedBalloons = 2; // Client requests the list of unpopped balloons
}

// Server-to-Client Signifiers (sent from server to client)
static public class ServerToClientSignifiers
{
    public const int SpawnBalloon = 1; // Server tells client to spawn a balloon
    public const int RemoveBalloon = 2; // Server tells client to remove a balloon
    public const int SendUnpoppedBalloons = 3; // Server sends unpopped balloon data to the client
}


#endregion

