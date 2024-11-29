using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleClick : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        void OnMouseDown()
        {
            int balloonID = GetInstanceID(); // Or another unique identifier
            NetworkClientProcessing.SendMessageToServer($"{ClientToServerSignifiers.BalloonPopped},{balloonID}", TransportPipeline.ReliableAndInOrder);
            Destroy(gameObject);
        }
    }
}
