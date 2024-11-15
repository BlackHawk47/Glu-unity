using System.Linq.Expressions;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Game : MonoBehaviour

{
    [SerializeField] private int Rows;

    [SerializeField] private int Columns;

    [SerializeField] private int TotalPairs;

    [SerializeField] private string frontsidesFolder;
    [SerializeField] private string backsidesFolder;

    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private Sprite[] backSprites;

    [SerializeField] private List<Sprite> selectedFrontSprites;
    [SerializeField] private Sprite selectedBackSprite;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Stack<GameObject> stackOfCards;
    [SerializeField] private GameObject[,] placedCards;
    
    private void MakeCards()
    {
        CalculateAmountOfPairs();
        LoadSprites();
        SelectFrontSprites();
        SelectBackSprite();
        ConstructCards();
    }

    private void DistributeCards()
    {

    }

    private void LoadSprites()
    {
        frontSprites = Resources.LoadAll<Sprite>("sprites/Frontsides");
        backSprites = Resources.LoadAll<Sprite>("sprites/Backsides");
    }

    private void CalculateAmountOfPairs()
    {
        if (Rows * Columns % 2 == 0)
        {
            TotalPairs = Rows * Columns / 2;
        } else
        {
            Debug.Log("er is een oneven aantal dus je kan niet spelen");
        }
    }


    private void SelectFrontSprites()
    {
        if(frontSprites.Length < TotalPairs)
        {
            Debug.LogError("Er zijn te weinig plaatjes om "+ TotalPairs + "paren te maken. Break uit code om crash te voorkomen");
            return;
        }
        selectedFrontSprites = new List<Sprite>();
        while (selectedFrontSprites.Count < TotalPairs)
        {
            int rnd = Random.Range(0, frontSprites.Length);
            if (!selectedFrontSprites.Contains(frontSprites[rnd]))
            {
                selectedFrontSprites.Add(frontSprites[rnd]);
            }
        }
    }

    private void SelectBackSprite() 
    {
        if(backSprites.Length > 0)
        {
            int rnd = Random.Range(0, backSprites.Length);
            selectedBackSprite = backSprites[rnd];
        }else
        {
            Debug.LogError("Er zijn geen achterkant plaatjes om te selecteren.");
        }
    }

    private void ConstructCards()
    {
        stackOfCards = new Stack<GameObject>();

        GameObject parent = new GameObject();
        parent.name = "Cards";

        foreach (Sprite sprite in selectedFrontSprites)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject go = Instantiate(cardPrefab);
                Card cardscript = go.GetComponent<Card>();

                cardscript.SetBack(selectedBackSprite);
                cardscript.SetFront(sprite);

                go.name = sprite.name;
                go.transform.parent = parent.transform;

                stackOfCards.Push(go);
            }
        }
    }

    private void Start()
    {
        MakeCards();
        DistributeCards();

    }

    private void Update()
    {
        
    }
}
