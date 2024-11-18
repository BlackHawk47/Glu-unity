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
    //[SerializeField] float percentage;
    private void Start()
    {
        
    }


    private void Update()
    {
        if(status == CardStatus.rotating_to_front || status == CardStatus.rotating_to_back)
        {
            turnTimer += Time.deltaTime;

            float percentage = turnTimer / turnTargetTime;

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

                    game.SelectCard(gameObject);
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
        //Debug.Log("geklikt op de kaart");
        //als hij de achterkant laat zien en er wordt geklikt draai dan om
        if (status == CardStatus.show_back)
        {
            TurnToFront();
        } 
        
        else if (status == CardStatus.show_front)
        { 
            TurnToBack();
        }
    }
    
    public void TurnToFront()
    {
        status = CardStatus.rotating_to_front;
        turnTimer = 0;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 180, 0);
    }

    public void TurnToBack()
    {
        status = CardStatus.rotating_to_back;
        turnTimer = 0;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

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

    public void SetFront(Sprite sprite)
    {
        if (frontRenderer != null)
        {
            frontRenderer.sprite = sprite;
        }
    }

    public void SetBack(Sprite sprite)
    {
        if (backRenderer != null)
        {
            backRenderer.sprite = sprite;
        }
    }

    public Vector2 GetFrontSize()
    {
        if(frontRenderer == null)
        {
            Debug.LogError("er is geen frontrenderer gevonden");
        }
        return frontRenderer.bounds.size;
    }

    public Vector2 GetBackSize()
    {
        if (backRenderer == null)
        {
            Debug.LogError("er is geen backrenderer gevonden");
        }
        return backRenderer.bounds.size;
    }
}
