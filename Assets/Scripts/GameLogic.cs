using UnityEngine;

public class GameLogic : MonoBehaviour
{
    float durationUntilNextBalloon;
    Sprite circleTexture;

    void Start()
    {
        // Link this GameLogic instance to NetworkClientProcessing
        NetworkClientProcessing.SetGameLogic(this);

        // Load the circle texture only once
        circleTexture = Resources.Load<Sprite>("Circle");
        if (circleTexture == null)
        {
            Debug.LogError("Circle texture not found! Ensure it's in a 'Resources' folder.");
        }
    }

    void Update()
    {
        // Countdown for spawning the next balloon
        durationUntilNextBalloon -= Time.deltaTime;

        if (durationUntilNextBalloon < 0)
        {
            durationUntilNextBalloon = 1f; // Reset the timer

            // Generate random screen position percentages
            float screenPositionXPercent = Random.Range(0.0f, 1.0f);
            float screenPositionYPercent = Random.Range(0.0f, 1.0f);
            Vector2 screenPosition = new Vector2(screenPositionXPercent * Screen.width, screenPositionYPercent * Screen.height);

            // Generate a unique balloon ID (example: using hash codes)
            int balloonID = Random.Range(1, 1000000); // Ensure uniqueness per session

            SpawnNewBalloon(screenPosition, balloonID);
        }
    }

    public void SpawnNewBalloon(Vector2 screenPosition, int balloonID)
    {
        // Create a new GameObject with a unique name
        GameObject balloon = new GameObject($"Balloon_{balloonID}");

        // Add components and set the sprite
        SpriteRenderer spriteRenderer = balloon.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = circleTexture;

        balloon.AddComponent<CircleClick>(); // Handles popping
        CircleCollider2D collider = balloon.AddComponent<CircleCollider2D>();
        collider.isTrigger = true; // Optional: Use trigger if needed for events

        // Convert screen position to world position
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));
        pos.z = 0; // Set Z position to 0 for 2D
        balloon.transform.position = pos;
    }

    public void RemoveBalloon(int balloonID)
    {
        GameObject balloon = GameObject.Find($"Balloon_{balloonID}");
        if (balloon != null)
        {
            Destroy(balloon); // Destroying the balloon by ID
        }
    }

}
