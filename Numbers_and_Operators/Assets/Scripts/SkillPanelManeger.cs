using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constraints;

public class SkillPanelManeger : MonoBehaviour
{
    [SerializeField] GameObject SkillPanel;
    private GameManeger gameManeger;
    private SkillManeger skillManeger;
    [SerializeField] UIManeger uiManeger;

    private void Start() {
        skillManeger = GameObject.Find("SkillManeger").GetComponent<SkillManeger>();
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
    }

    public void OnExchangeButtonInSkillPanel() {
        int currentCardPoint = gameManeger.playerCardPoint;
        if(SkillCost.exchangeSkillCost <= currentCardPoint) {
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.exchangeSkillCost);
            gameManeger.playerCardPoint = currentCardPoint;

            uiManeger.SetPlayerCardPointText(currentCardPoint);
            skillManeger.ExchangeEachNumbers();
            SkillPanel.SetActive(false);
        }
       
    }

    public void OnDrawButtonInSkillPanel() {

        int currentCardPoint = gameManeger.playerCardPoint;
        if (SkillCost.drawSkillCost <= currentCardPoint) {
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            gameManeger.playerCardPoint = currentCardPoint;

            uiManeger.SetPlayerCardPointText(currentCardPoint);

            skillManeger.DrawCards(gameManeger.PlayerOperatorsHandTransform, gameManeger.PlayerNumbersHandTransform);
            SkillPanel.SetActive(false);
        }
    }

    public void OnDownButtonInSkillPanel() {
        int currentCardPoint = gameManeger.playerCardPoint;
        if(SkillCost.downSkillCost <= currentCardPoint) {

            // スキルポイントを消費。
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            gameManeger.playerCardPoint = currentCardPoint;
            uiManeger.SetPlayerCardPointText(gameManeger.playerCardPoint);

            // 敵のスコアの減少
            int currentEnemyScore = gameManeger.enemyScore;
            gameManeger.enemyScore = skillManeger.DownScore(currentEnemyScore);
            uiManeger.SetEnemyScoreText(gameManeger.enemyScore);

        }
       
    }

    public void OnUpButtonInSkillPanel() {
        int currentCardPoint = gameManeger.playerCardPoint;
        if( SkillCost.upSkillCost <= currentCardPoint) {

            // スキルポイントを消費。
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            gameManeger.playerCardPoint = currentCardPoint;
            uiManeger.SetPlayerCardPointText(gameManeger.playerCardPoint);

            // 敵のスコアの上昇
            int currentEnemyScore = gameManeger.enemyScore;
            gameManeger.enemyScore = skillManeger.UpScore(currentEnemyScore);
            uiManeger.SetEnemyScoreText(gameManeger.enemyScore);
           
          
            SkillPanel.SetActive(false);
        }
      
    }

    public void OnBackButtonInSkillPanel() {
       
        SkillPanel.SetActive(false);
    }
}
