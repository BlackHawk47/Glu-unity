using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;

public enum CardStatus
{
    show_back = 0,
    show_front,
    rotating_to_back,
    rotating_to_front
}

public class Card : MonoBehaviour
{
    [SerializeField] private CardStatus status;

    [SerializeField] public float turnTargetTime;

    [SerializeField] private float turnTimer;

    [SerializeField] Quaternion startRotation;

    [SerializeField] Quaternion targetRotation;

    [SerializeField] SpriteRenderer frontRenderer;

    [SerializeField] SpriteRenderer backRenderer;

    [SerializeField] public Game game;

    
    private void Update()
    {
        if(status == CardStatus.rotating_to_front || status == CardStatus.rotating_to_back)
        {
            turnTimer += Time.deltaTime;

            float percentage = turnTimer / turnTargetTime;
            // Laat de kaart draaien met een Slerp-animatie
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percentage);
            if (percentage > 1)
            {
                if (status == CardStatus.rotating_to_back)
                {
                    status = CardStatus.show_back;
                }

                else if (status == CardStatus.rotating_to_front) 
                { 
                    status = CardStatus.show_front;
                }
            }
        }
    }

    //de huidige statis van de kaart
    private void Awake()
    {
        status = CardStatus.show_back;
        GetFrontAndBackSpriteRenderers();
        game = FindFirstObjectByType<Game>();
    }

    //als je klikt laat het zien dat je hebt geklikt

    private void OnMouseUp()
    {
        //als hij de achterkant laat zien en er wordt geklikt draai dan om
        if (game.AllowedToSelectCard(this) == true)
        {


            if (status == CardStatus.show_back)
            {
                game.SelectCard(gameObject);
                TurnToFront();// Draai de kaart naar de voorkant
            }

            else if (status == CardStatus.show_front)
            {
                TurnToBack(); // Draai de kaart terug naar de achterkant
            }
        }
    }
    // Start het draaien naar de voorkant
    public void TurnToFront()
    {
        status = CardStatus.rotating_to_front;
        turnTimer = 0;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 180, 0);
    }

    // Start het draaien naar de achterkant
    public void TurnToBack()
    {
        status = CardStatus.rotating_to_back;
        turnTimer = 0;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, 0);
    }
    // Zoek en sla de renderers voor de voor- en achterkant van de kaart op
    private void GetFrontAndBackSpriteRenderers()
    {
        foreach(Transform t  in transform)
        {
            if (t.name == "Front")
            {
                    frontRenderer = t.GetComponent<SpriteRenderer>();
            } 
            
            else if (t.name == "Back")
            {
                backRenderer = t.GetComponent<SpriteRenderer>();
            }
        }
    }
    // Stel een nieuwe sprite in voor de voorkant
    public void SetFront(Sprite sprite)
    {
        if (frontRenderer != null)
        {
            frontRenderer.sprite = sprite;
        }
    }
    // Stel een nieuwe sprite in voor de achterkant
    public void SetBack(Sprite sprite)
    {
        if (backRenderer != null)
        {
            backRenderer.sprite = sprite;
        }
    }
    // Verkrijg de grootte van de voorkant
    public Vector2 GetFrontSize()
    {
        if(frontRenderer == null)
        {
            Debug.LogError("er is geen frontrenderer gevonden");
        }
        return frontRenderer.bounds.size;
    }
    // Verkrijg de grootte van de achterkant
    public Vector2 GetBackSize()
    {
        if (backRenderer == null)
        {
            Debug.LogError("er is geen backrenderer gevonden");
        }
        return backRenderer.bounds.size;
    }
}
