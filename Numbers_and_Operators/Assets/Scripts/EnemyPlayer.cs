using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemyPlayer {

    public System.Tuple<GameObject, GameObject> SelectEnemyCards(Transform EnemyHandTransform) {

        List<GameObject> EnemyHand = GetEnemyHandFromTramsform(EnemyHandTransform);

        List<GameObject> NumberCardList = new List<GameObject> { };
        List<GameObject> OperatorCardList = new List<GameObject> { };

        for (int i = 0; i < EnemyHand.Count; i++) {
            TextMeshProUGUI CardTMProText = EnemyHand[i].GetComponentInChildren<TextMeshProUGUI>();
      
            if (CardTMProText.text != "+" && CardTMProText.text != "-" && CardTMProText.text != "×") {
                NumberCardList.Add(EnemyHand[i]);
            } else {
                OperatorCardList.Add(EnemyHand[i]);
            }
        }

        GameObject NumberCard = SelectNumberCard(NumberCardList);
        GameObject OperatorCard = SelectOperatorCard(OperatorCardList);

        System.Tuple<GameObject, GameObject> Cards = new System.Tuple<GameObject, GameObject>(NumberCard, OperatorCard);
        return Cards;
        //Debug.Log(EnemyHand[0].GetComponentInChildren<TextMeshProUGUI>().text);
    }

    private List<GameObject> GetEnemyHandFromTramsform(Transform EnemyHandTransform) {
        List<GameObject> EnemyHand = new List<GameObject> { };

        for (int i = 0; i < EnemyHandTransform.childCount; i++) {
            EnemyHand.Add(EnemyHandTransform.GetChild(i).gameObject);
        }

        return EnemyHand;
    }

    private GameObject SelectNumberCard(List<GameObject> NumberCardList) {

        GameObject NumberCard;
        NumberCard = SelectCard(NumberCardList);
        return NumberCard;
    }

    private GameObject SelectOperatorCard(List<GameObject> OperatorCardList) {
        GameObject OperatorCard;
        
        OperatorCard = SelectCard(OperatorCardList);
        return OperatorCard;
    }






    private GameObject SelectCard(List<GameObject> CardList) {

        int index = Random.Range(0, CardList.Count - 1);
        //Debug.Log(index);
        GameObject card = CardList[index];
        return card;


    }
}
