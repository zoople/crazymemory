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
    private int numMatches = 0;
    private List<Card> openCards = new List<Card>();
    private List<Card> completedCards = new List<Card>();
    private List<Card> hiddenCards = new List<Card>();
    private bool skipAnimation = false;
    private float timeElapsed = 0.0f;
    private bool isTimerRunning;
    
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


       // for (int i=0; i< 50; i++) swapTwoNoAnim(Random.Range(0,numCards), Random.Range(0, numCards));

 

        for (int i = 0; i < numCards; i++) hiddenCards.Add(playField[i].GetComponent<Card>());
    }

    IEnumerator flipTable()
    {
        int numSteps = 100;
        int deltaStep = 1;
        float percentage = 0;

        float startingAngle = table.transform.eulerAngles.z;

        for (int i = 0; i <= numSteps; i+=deltaStep)
        {
            // if (skipAnimation) deltaStep = 3;
            //table.transform.Rotate(new Vector3(0, 0, (float)90/((float)numSteps/(float)deltaStep)) );
            // table.transform.rotation = new Quaternion(table.transform.rotation.x, table.transform.rotation.y, table.transform.rotation.z, table.transform.rotation.w);
            percentage = (float)i / ((float)numSteps/(float)deltaStep);

          //  Debug.Log("before: " + table.transform.eulerAngles.z);

            //table.transform.rotation = new Quaternion(table.transform.rotation.x, table.transform.rotation.y, , table.transform.rotation.w);
            Vector3 currentAngle = new Vector3(
            0,
            0,
            Mathf.LerpAngle(startingAngle, startingAngle+90, percentage));
            table.transform.eulerAngles = currentAngle;
       


            yield return null;
        }

        if (skipAnimation) skipAnimation = false;
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
        float deltaSize = 0.006f;

        hiddenCards[cardA].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "HigherCards";
        hiddenCards[cardB].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "HigherCards";
        hiddenCards[cardA].gameObject.GetComponent<SpriteRenderer>().sprite = hiddenCards[cardA].cardBackActiveSprite;
        hiddenCards[cardB].gameObject.GetComponent<SpriteRenderer>().sprite = hiddenCards[cardB].cardBackActiveSprite;


        for (int i = 0; i < 15; i++)
        { 
            hiddenCards[cardA].gameObject.transform.localScale = new Vector2(hiddenCards[cardA].gameObject.transform.localScale.x+deltaSize,
                                                                           hiddenCards[cardA].gameObject.transform.localScale.y+deltaSize);


            hiddenCards[cardB].gameObject.transform.localScale = new Vector2(hiddenCards[cardB].gameObject.transform.localScale.x + deltaSize,
                                                                           hiddenCards[cardB].gameObject.transform.localScale.y + deltaSize);

            yield return null;
        }

        yield return null;

        int steps = 50;
        Vector2 cardAOrigin = hiddenCards[cardA].gameObject.transform.position;
        Vector2 cardBOrigin = hiddenCards[cardB].gameObject.transform.position;
        for (int i = 0; i <= steps; i++)
        {
            hiddenCards[cardA].gameObject.transform.position = Vector2.Lerp(cardAOrigin, cardBOrigin, ((float)i / (float)steps));
            hiddenCards[cardB].gameObject.transform.position = Vector2.Lerp(cardBOrigin, cardAOrigin, ((float)i / (float)steps));

            yield return null;
        }

            yield return null;

        for (int i = 0; i < 15; i++)
        {
            hiddenCards[cardA].gameObject.transform.localScale = new Vector2(hiddenCards[cardA].gameObject.transform.localScale.x - deltaSize,
                                                                           hiddenCards[cardA].gameObject.transform.localScale.y - deltaSize);


            hiddenCards[cardB].gameObject.transform.localScale = new Vector2(hiddenCards[cardB].gameObject.transform.localScale.x - deltaSize,
                                                                           hiddenCards[cardB].gameObject.transform.localScale.y - deltaSize);

            yield return null;
        }

        hiddenCards[cardA].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Cards";
        hiddenCards[cardB].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Cards";
        hiddenCards[cardA].gameObject.GetComponent<SpriteRenderer>().sprite = hiddenCards[cardA].cardBackSprite;
        hiddenCards[cardB].gameObject.GetComponent<SpriteRenderer>().sprite = hiddenCards[cardB].cardBackSprite;

        if (skipAnimation) skipAnimation = false;
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

        
        if (skipAnimation) skipAnimation = false;

        if (!isMatch)
        {
            foreach (Card _card in openCards)
            {
                _card.GetComponent<Animator>().SetTrigger("incorrect");
            }
            yield return new WaitForSeconds(1.5f);
            flipOpenCards();
        }
        else
        {

            foreach (Card _card in openCards)
            {
                _card.GetComponent<Animator>().SetTrigger("flipCardCorrect");
                completedCards.Add(_card);
                hiddenCards.Remove(_card);
            }

            openCards.Clear();

        }

        if (completedCards.Count == 12) { Debug.Log("Game Over"); isTimerRunning = false; }
        gameState = "playerSelectA";



        yield return null;
    }
        

    public void cardClicked(Card clickedCard)
    {
        if (isTimerRunning == false && numMatches == 0) isTimerRunning = true;

        
        if (clickedCard.getCardType() == "standard")
        {
            if (gameState == "playerSelectA")
            {
               // clickedCard.GetComponent<Animator>().SetTrigger("flipCard");

                clickedCard.revealCard();
                openCards.Add(clickedCard);
                gameState = "playerSelectB";
            }
            else if (gameState == "playerSelectB")
            {
                //clickedCard.GetComponent<Animator>().SetTrigger("flipCard");

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
                skipAnimation = true;
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
        // timeText.GetComponent<Text>().text = "test";
        if (isTimerRunning)
        {
            timeElapsed += Time.deltaTime;

            string mins = "";
            string secs = "";
            string milis = "";

            int numMins = (int)(timeElapsed / 60);
            int numSecs = (int)timeElapsed;
            int numMilis = (int)((timeElapsed - numSecs) * 100);

            if (numMins < 10) mins = "0";
            if (numSecs < 10) secs = "0";
            if (numMilis < 10) milis = "0";


            mins += numMins.ToString();
            secs += numSecs.ToString();
            milis += numMilis.ToString();

            gameObject.GetComponent<TextMesh>().text = mins + " : " + secs + " : " + milis;
        }
	}
}
