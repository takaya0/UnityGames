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
    
    public void ExchangeEachScores(PlayerManeger player, RuleBasedAI enemyplayer) {
        (player.score, enemyplayer.score) = (enemyplayer.score, player.score);
    
    }
    public void DrawCards(Transform operatorsHandTransform,Transform numbersHandTransform, bool isPlayerCard) {

        int currentCardInHandNum = operatorsHandTransform.GetComponentsInChildren<CardController>().Length;

        if (currentCardInHandNum < Const.MAX_HAND_NUM) {
           
            string opetatorCardName = gameManeger.DrawCard(OperatorsCardList);
            string numberCardName = gameManeger.DrawCard(NumbersCardList);
            gameManeger.AddCardToHand(operatorsHandTransform, opetatorCardName, isPlayerCard);
            gameManeger.AddCardToHand(numbersHandTransform, numberCardName, isPlayerCard);

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
