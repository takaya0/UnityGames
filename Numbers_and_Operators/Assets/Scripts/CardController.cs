using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour{
    // Start is called before the first frame update

    CardView view;
    CardModel card;
    public CardMovement movement; // カードの移動

    private void Awake() {
        view = GetComponent<CardView>();
    }
    public void Init(string cardName) {
        card =  new CardModel(cardName);
        view.Show(card);
        
    }
}
