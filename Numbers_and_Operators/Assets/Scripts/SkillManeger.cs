using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Constraints;
public class SkillManeger : MonoBehaviour{
   
    private GameManeger gameManeger;

    List<string> OperatorsCardList = new List<string>(Const.OperatorsCardList);
    List<string> NumbersCardList = new List<string>(Const.NumbersCardList);

    // Start is called before the first frame update
    void Start() {
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
    }

    public int DecreaseCardPoint(int currentSkillPoint, int skillCost) {
        return currentSkillPoint - skillCost;
    }
    
    public void ExchangeEachNumbers() {
        //(gameManeger.enemyScore, gameManeger.) = (gameManeger.PlayerNumber.text, gameManeger.EnemyNumber.text);
    }
    public void DrawCards(Transform operatorHandTransform,Transform numbersHandTransform) {

        int currentCardInHandNum = gameManeger.PlayerOperatorsHandTransform.GetComponentsInChildren<CardController>().Length;

        if (currentCardInHandNum < Const.MAX_HAND_NUM) {
           
            string opetatorCardName = gameManeger.DrawCard(OperatorsCardList);
            string numberCardName = gameManeger.DrawCard(NumbersCardList);
            gameManeger.AddCardToHand(operatorHandTransform, opetatorCardName);
            gameManeger.AddCardToHand(numbersHandTransform, numberCardName);

        }
    }


    public int DownScore(int currentScore) {
        int downScore = Mathf.Max(currentScore - Random.Range(10, 31), 1);
        return downScore;
    }

    public int UpScore(int currentScore) {
        int upScore = currentScore + Random.Range(10, 31);
        return upScore;
    }
}
