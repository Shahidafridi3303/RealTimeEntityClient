using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using System.Text;

public class NetworkClient : MonoBehaviour
{
    public static NetworkClient Instance { get; private set; } // Singleton instance

    NetworkDriver networkDriver;
    NetworkConnection networkConnection;
    NetworkPipeline reliablePipeline;
    const ushort NetworkPort = 9001;
    const string IPAddress = "192.168.4.102";

    void Start()
    {
        // Enforce Singleton
        if (Instance != null && Instance != this)
        {
            Debug.Log("Singleton-ish architecture violation detected. Investigate duplicate instances.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        // Initialize the client
        NetworkClientProcessing.SetNetworkedClient(this);
        Connect();
    }

    public void OnDestroy()
    {
        if (networkConnection.IsCreated)
            networkConnection.Disconnect(networkDriver);
        networkConnection = default(NetworkConnection);
        networkDriver.Dispose();
    }

    void Update()
    {
        networkDriver.ScheduleUpdate().Complete();

        if (!networkConnection.IsCreated)
        {
            Debug.Log("Client is unable to connect to the server.");
            return;
        }

        DataStreamReader streamReader;
        NetworkEvent.Type networkEventType;

        while (PopNetworkEventAndCheckForData(out networkEventType, out streamReader))
        {
            switch (networkEventType)
            {
                case NetworkEvent.Type.Connect:
                    NetworkClientProcessing.ConnectionEvent();
                    break;

                case NetworkEvent.Type.Data:
                    int sizeOfDataBuffer = streamReader.ReadInt();
                    NativeArray<byte> buffer = new NativeArray<byte>(sizeOfDataBuffer, Allocator.Persistent);
                    streamReader.ReadBytes(buffer);
                    string msg = Encoding.Unicode.GetString(buffer.ToArray());
                    NetworkClientProcessing.ReceivedMessageFromServer(msg);
                    buffer.Dispose();
                    break;

                case NetworkEvent.Type.Disconnect:
                    NetworkClientProcessing.DisconnectionEvent();
                    networkConnection = default(NetworkConnection);
                    break;
            }
        }
    }

    private bool PopNetworkEventAndCheckForData(out NetworkEvent.Type networkEventType, out DataStreamReader streamReader)
    {
        networkEventType = networkConnection.PopEvent(networkDriver, out streamReader);
        return networkEventType != NetworkEvent.Type.Empty;
    }

    public void SendMessageToServer(string msg)
    {
        byte[] msgAsByteArray = Encoding.Unicode.GetBytes(msg);
        NativeArray<byte> buffer = new NativeArray<byte>(msgAsByteArray, Allocator.Persistent);

        DataStreamWriter streamWriter;
        networkDriver.BeginSend(reliablePipeline, networkConnection, out streamWriter);
        streamWriter.WriteInt(buffer.Length);
        streamWriter.WriteBytes(buffer);
        networkDriver.EndSend(streamWriter);

        buffer.Dispose();
    }

    public void Connect()
    {
        networkDriver = NetworkDriver.Create();
        reliablePipeline = networkDriver.CreatePipeline(typeof(FragmentationPipelineStage), typeof(ReliableSequencedPipelineStage));
        networkConnection = default(NetworkConnection);
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(IPAddress, NetworkPort, NetworkFamily.Ipv4);
        networkConnection = networkDriver.Connect(endpoint);
    }

    public bool IsConnected()
    {
        return networkConnection.IsCreated;
    }

    public void Disconnect()
    {
        if (networkConnection.IsCreated)
            networkConnection.Disconnect(networkDriver);
        networkConnection = default(NetworkConnection);
    }
}
