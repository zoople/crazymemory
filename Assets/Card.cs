using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {


    private Sprite cardFaceSprite;
    private string cardType;
    private string cardFaceValue;
    public Sprite cardBackSprite;
    public bool faceUp = false;

	public void setCard(Sprite _face, string _cardType, string _cardFace)
    {
        cardFaceSprite = _face;
        cardFaceValue = _cardFace;
        cardType = _cardType;

    }

    public void coverUp()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = cardBackSprite;
        faceUp = false;
    }

    public void revealCard()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = cardFaceSprite;
        faceUp = true;
    }

    public void OnMouseDown()
    {
        if (!faceUp) GameController.instance.cardClicked(this);
    }

    public Sprite getFacesprite() { return cardFaceSprite; }
    public string getFaceValue() { return cardFaceValue; }
    public string getCardType() { return cardType; }



}
