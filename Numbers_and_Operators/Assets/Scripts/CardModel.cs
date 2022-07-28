using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel {
    public string name;
    public string kind;
    public string value;
    public bool isPlayerCard;

    public CardModel(string cardName, bool isPlayerCard) {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardDataList/Card" + cardName);
        name = cardEntity.name;
        kind = cardEntity.kind;
        value = cardEntity.value;
        this.isPlayerCard = isPlayerCard;
    }
}
