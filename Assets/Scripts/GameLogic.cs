using UnityEngine;

public class GameLogic : MonoBehaviour
{
    Sprite circleTexture;

    void Start()
    {
        NetworkClientProcessing.SetGameLogic(this);
        circleTexture = Resources.Load<Sprite>("Circle");
    }

    public void SpawnNewBalloon(Vector2 screenPosition, int balloonID)
    {
        Debug.Log($"Client: Creating balloon GameObject for ID {balloonID}");

        GameObject balloon = new GameObject($"Balloon_{balloonID}");
        SpriteRenderer renderer = balloon.AddComponent<SpriteRenderer>();
        renderer.sprite = circleTexture;

        CircleCollider2D collider = balloon.AddComponent<CircleCollider2D>();
        collider.isTrigger = true; // Ensures it reacts to clicks

        balloon.AddComponent<CircleClick>();
        balloon.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x * Screen.width, screenPosition.y * Screen.height, 0));
        balloon.transform.position = new Vector3(balloon.transform.position.x, balloon.transform.position.y, 0);
    }



    public void RemoveBalloon(int balloonID)
    {
        GameObject balloon = GameObject.Find($"Balloon_{balloonID}");
        if (balloon != null)
        {
            Destroy(balloon);
        }
    }
}