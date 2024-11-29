using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkClientProcessing
{
    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        if (signifier == ServerToClientSignifiers.SpawnBalloon)
        {
            int balloonID = int.Parse(csv[1]);
            float xPercent = float.Parse(csv[2]);
            float yPercent = float.Parse(csv[3]);
            Vector2 screenPosition = new Vector2(xPercent, yPercent);
            gameLogic.SpawnNewBalloon(screenPosition, balloonID);
        }
        else if (signifier == ServerToClientSignifiers.RemoveBalloon)
        {
            int balloonID = int.Parse(csv[1]);
            gameLogic.RemoveBalloon(balloonID); // Ensure GameLogic has this method
        }
        else if (signifier == ServerToClientSignifiers.SendUnpoppedBalloons)
        {
            // Parse and spawn all unpopped balloons
            for (int i = 1; i < csv.Length; i += 3)
            {
                int balloonID = int.Parse(csv[i]);
                float xPercent = float.Parse(csv[i + 1]);
                float yPercent = float.Parse(csv[i + 2]);
                Vector2 screenPosition = new Vector2(xPercent, yPercent);
                gameLogic.SpawnNewBalloon(screenPosition, balloonID);
            }
        }
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
static public class ClientToServerSignifiers
{
    public const int BalloonPopped = 1;
    public const int RequestUnpoppedBalloons = 2;
}

static public class ServerToClientSignifiers
{
    public const int SpawnBalloon = 1;
    public const int RemoveBalloon = 2;
    public const int SendUnpoppedBalloons = 3;
}
#endregion
