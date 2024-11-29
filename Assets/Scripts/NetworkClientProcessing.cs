using UnityEngine;

static public class NetworkClientProcessing
{
    static NetworkClient networkClient;
    static GameLogic gameLogic;

    public static void ReceivedMessageFromServer(string msg)
    {
        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        if (signifier == ServerToClientSignifiers.SpawnBalloon)
        {
            int balloonID = int.Parse(csv[1]);
            float xPercent = float.Parse(csv[2]);
            float yPercent = float.Parse(csv[3]);
            Vector2 screenPosition = new Vector2(xPercent, yPercent);
            Debug.Log($"Client: Spawning balloon ID {balloonID} at ({xPercent}, {yPercent})");
            gameLogic.SpawnNewBalloon(screenPosition, balloonID);
        }
        else if (signifier == ServerToClientSignifiers.RemoveBalloon)
        {
            int balloonID = int.Parse(csv[1]);
            Debug.Log($"Client: Removing balloon ID {balloonID}");
            gameLogic.RemoveBalloon(balloonID);
        }
    }


    public static void SendMessageToServer(string msg)
    {
        networkClient.SendMessageToServer(msg);
    }

    public static void ConnectionEvent()
    {
        SendMessageToServer($"{ClientToServerSignifiers.RequestUnpoppedBalloons}");
    }

    public static void DisconnectionEvent()
    {
        Debug.Log("Disconnected from the server.");
    }

    public static void SetGameLogic(GameLogic logic) => gameLogic = logic;
    public static void SetNetworkedClient(NetworkClient client) => networkClient = client;
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