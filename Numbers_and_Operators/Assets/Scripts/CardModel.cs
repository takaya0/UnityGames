using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel {
    public string name;
    public string kind;
    public string value;

    public CardModel(string cardName) {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardDataList/Card" + cardName);
        name = cardEntity.name;
        kind = cardEntity.kind;
        value = cardEntity.value;
    }
}
