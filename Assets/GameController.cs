using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private const int NUMCARDS =  16;
    public Sprite[] cardFaces;
    public static GameController instance;
    public GameObject[] playField = new GameObject[NUMCARDS];
    public GameObject table;
    private string gameState;
    private List<Card> openCards = new List<Card>();
    private List<Card> completedCards = new List<Card>();
    private List<Card> hiddenCards = new List<Card>();

    void setCards()
    {
        int numCards = 16;

        playField[0].GetComponent<Card>().setCard(cardFaces[0], "standard", "BlueCircle");
        playField[1].GetComponent<Card>().setCard(cardFaces[0], "standard", "BlueCircle");

        playField[2].GetComponent<Card>().setCard(cardFaces[1], "standard", "GreenCircle");
        playField[3].GetComponent<Card>().setCard(cardFaces[1], "standard", "GreenCircle");

        playField[4].GetComponent<Card>().setCard(cardFaces[2], "standard", "RedCircle");
        playField[5].GetComponent<Card>().setCard(cardFaces[2], "standard", "RedCircle");

        playField[6].GetComponent<Card>().setCard(cardFaces[13], "hazrd", "HazardSwap");
        playField[7].GetComponent<Card>().setCard(cardFaces[12], "hazrd", "HazardRotate");

        playField[8].GetComponent<Card>().setCard(cardFaces[4], "standard", "BlueSquare");
        playField[9].GetComponent<Card>().setCard(cardFaces[4], "standard", "BlueSquare");

        playField[10].GetComponent<Card>().setCard(cardFaces[5], "standard", "GreenSquare");
        playField[11].GetComponent<Card>().setCard(cardFaces[5], "standard", "GreenSquare");

        playField[12].GetComponent<Card>().setCard(cardFaces[13], "hazrd", "HazardSwap");
        playField[13].GetComponent<Card>().setCard(cardFaces[12], "hazrd", "HazardRotate");

        playField[14].GetComponent<Card>().setCard(cardFaces[6], "standard", "RedSquare");
        playField[15].GetComponent<Card>().setCard(cardFaces[6], "standard", "RedSquare");


        for (int i=0; i< 50; i++) swapTwoNoAnim(Random.Range(0,numCards), Random.Range(0, numCards));

 

        for (int i = 0; i < numCards; i++) hiddenCards.Add(playField[i].GetComponent<Card>());
    }

    IEnumerator flipTable()
    {
        int numSteps = 100;
        for (int i = 0; i < numSteps; i++)
        {
            table.transform.Rotate(new Vector3(0, 0, (float)90/(float)numSteps ));
            yield return null;
        }

        gameState = "playerSelectA";

        yield return null;
    }

    void swapTwoNoAnim(int cardA, int cardB)
    {
        Vector2 cardAOrigin = playField[cardA].gameObject.transform.position;
        Vector2 cardBOrigin = playField[cardB].gameObject.transform.position;

        playField[cardA].gameObject.transform.position = cardBOrigin;
        playField[cardB].gameObject.transform.position = cardAOrigin;

    }

    IEnumerator swapTwo(int cardA, int cardB)
    {
        float deltaSize = 0.005f;

        hiddenCards[cardA].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "HigherCards";
        hiddenCards[cardB].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "HigherCards";

        for (int i = 0; i < 20; i++)
        { 
            hiddenCards[cardA].gameObject.transform.localScale = new Vector2(hiddenCards[cardA].gameObject.transform.localScale.x+deltaSize,
                                                                           hiddenCards[cardA].gameObject.transform.localScale.y+deltaSize);


            hiddenCards[cardB].gameObject.transform.localScale = new Vector2(hiddenCards[cardB].gameObject.transform.localScale.x + deltaSize,
                                                                           hiddenCards[cardB].gameObject.transform.localScale.y + deltaSize);

            yield return null;
        }

        yield return null;

        int steps = 80;
        Vector2 cardAOrigin = hiddenCards[cardA].gameObject.transform.position;
        Vector2 cardBOrigin = hiddenCards[cardB].gameObject.transform.position;
        for (int i = 0; i < steps; i++)
        {
            hiddenCards[cardA].gameObject.transform.position = Vector2.Lerp(cardAOrigin, cardBOrigin, ((float)i / (float)steps));
            hiddenCards[cardB].gameObject.transform.position = Vector2.Lerp(cardBOrigin, cardAOrigin, ((float)i / (float)steps));

            yield return null;
        }

            yield return null;

        for (int i = 0; i < 20; i++)
        {
            hiddenCards[cardA].gameObject.transform.localScale = new Vector2(hiddenCards[cardA].gameObject.transform.localScale.x - deltaSize,
                                                                           hiddenCards[cardA].gameObject.transform.localScale.y - deltaSize);


            hiddenCards[cardB].gameObject.transform.localScale = new Vector2(hiddenCards[cardB].gameObject.transform.localScale.x - deltaSize,
                                                                           hiddenCards[cardB].gameObject.transform.localScale.y - deltaSize);

            yield return null;
        }

        hiddenCards[cardA].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Cards";
        hiddenCards[cardB].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Cards";

        gameState = "playerSelectA";

        yield return null;

    }

    public string getGameState() { return gameState; }


    public void flipOpenCards()
    {
        foreach (Card _card in openCards)
        {
            _card.coverUp();
        }
        openCards.Clear();
    }

    public bool checkOpenCardsMatch()
    {
        bool match = true;
        string cardToMach = null;

        foreach (Card _card in openCards)
        {
            if (cardToMach == null) cardToMach = _card.getFaceValue();

            else if (_card.getFaceValue() != cardToMach) return false;
        }
        return true;


    }

    IEnumerator endOfRound()
    {
        bool isMatch = checkOpenCardsMatch();

        yield return new WaitForSeconds(1);

        if (!isMatch) flipOpenCards();
        else
        {
            foreach (Card _card in openCards)
            {
                completedCards.Add(_card);
                hiddenCards.Remove(_card);
            }

            openCards.Clear();
        }

        if (completedCards.Count == 6) Debug.Log("Game Over");
        gameState = "playerSelectA";



        yield return null;
    }
        

    public void cardClicked(Card clickedCard)
    {
        if (clickedCard.getCardType() == "standard")
        {
            if (gameState == "playerSelectA")
            {
                clickedCard.revealCard();
                openCards.Add(clickedCard);
                gameState = "playerSelectB";
            }
            else if (gameState == "playerSelectB")
            {
                clickedCard.revealCard();
                openCards.Add(clickedCard);
                //bool isMatch = checkOpenCardsMatch();
                //if (!isMatch) flipOpenCards();
                //gameState = "playerSelectA";
                gameState = "checkMatch";
                StartCoroutine(endOfRound());
            }
            else if (gameState == "checkMatch")
            {
                Debug.Log("Locked out");
            }
        }
        else
        {
            if (gameState != "checkMatch")
            {
                clickedCard.revealCard();
                //openCards.Add(clickedCard);
                hiddenCards.Remove(clickedCard);
                flipOpenCards();

                Debug.Log(clickedCard.getFaceValue());
                if (clickedCard.getFaceValue() == "HazardSwap")
                {
                    int cardA = Random.Range(0, hiddenCards.Count);
                    int cardB = Random.Range(0, hiddenCards.Count);
                    while (cardB == cardA) cardB = Random.Range(0, hiddenCards.Count);
                    StartCoroutine(swapTwo(cardA, cardB));
                }
                if (clickedCard.getFaceValue() == "HazardRotate") StartCoroutine(flipTable());

                openCards.Clear();

                gameState = "checkMatch";
            }
        }
    }
    // Use this for initialization
    void Start () {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        setCards();
        gameState = "playerSelectA";

    }

    // Update is called once per frame
    void Update () {
		
	}
}

//TODO

//add timing for cards on 2nd turn
//add animation
//change sorting layer so swapped cards are on top
//tweak timing and feedback for looses
//tweak feedback for wins
//add score / time
