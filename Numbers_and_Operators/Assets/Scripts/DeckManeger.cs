using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DeckManeger : MonoBehaviour
{
    // Start is called before the first frame update

    private Deck deck;

    public void LoadDeckFromJSON(string fileName) {
        StreamReader reader = new StreamReader(fileName);
        string dataFile = reader.ReadToEnd();
        deck = JsonUtility.FromJson<Deck>(dataFile);
    }

    public string DrawOperatorCards() {
        List<string> operatorsCardList = new List<string>(deck.oepratorsDeck.Keys);
        string operatorCard = DrawRandom(operatorsCardList);
        deck.oepratorsDeck[operatorCard] -= 1;
        return operatorCard;
    }


    private string DrawRandom(List<string> cardList) {
        string card = cardList[Random.Range(0, cardList.Count - 1)];
        return card;
    }
}
