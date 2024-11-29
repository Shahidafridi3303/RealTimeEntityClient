using UnityEngine;

public class CircleClick : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log($"Client: Balloon {gameObject.name} clicked.");
        int balloonID = int.Parse(gameObject.name.Split('_')[1]);
        NetworkClientProcessing.SendMessageToServer($"{ClientToServerSignifiers.BalloonPopped},{balloonID}");
        Destroy(gameObject);
    }
}