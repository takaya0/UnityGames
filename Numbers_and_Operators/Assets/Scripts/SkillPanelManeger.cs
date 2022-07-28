using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constraints;

public class SkillPanelManeger : MonoBehaviour
{
    [SerializeField] GameObject SkillPanel;
   
    private SkillManeger skillManeger;
    [SerializeField] UIManeger uiManeger;

    private PlayerManeger player;
    private RuleBasedAI enemyAIPlayer;
    private void Start() {
        skillManeger = GameObject.Find("SkillManeger").GetComponent<SkillManeger>();
        player = GameObject.Find("Player").GetComponent<PlayerManeger>();
        enemyAIPlayer = GameObject.Find("EnemyPlayer").GetComponent<RuleBasedAI>();
    }

    public void OnExchangeButtonInSkillPanel() {
        int currentCardPoint = player.cardPoint;
        if(SkillCost.exchangeSkillCost <= currentCardPoint) {
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.exchangeSkillCost);
            player.cardPoint = currentCardPoint;

            uiManeger.SetPlayerCardPointText(currentCardPoint);
            skillManeger.ExchangeEachScores(player, enemyAIPlayer);
            uiManeger.SetPlayerScoreText(player.score);
            uiManeger.SetEnemyScoreText(enemyAIPlayer.score);
            SkillPanel.SetActive(false);
        }
       
    }

    public void OnDrawButtonInSkillPanel() {

        int currentCardPoint = player.cardPoint;
        if (SkillCost.drawSkillCost <= currentCardPoint) {
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            player.cardPoint = currentCardPoint;

            uiManeger.SetPlayerCardPointText(currentCardPoint);

            skillManeger.DrawCards(player.operatorsHandTransform, player.numbersHandTransform, true);
            SkillPanel.SetActive(false);
        }
    }

    public void OnDownButtonInSkillPanel() {
        int currentCardPoint = player.cardPoint;
        if(SkillCost.downSkillCost <= currentCardPoint) {

            // 
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            player.cardPoint = currentCardPoint;
            uiManeger.SetPlayerCardPointText(player.cardPoint);

            // 
            enemyAIPlayer.score = skillManeger.DownScore(enemyAIPlayer.score);
            uiManeger.SetEnemyScoreText(enemyAIPlayer.score);

        }
       
    }

    public void OnUpButtonInSkillPanel() {
        int currentCardPoint = player.cardPoint;
        if( SkillCost.upSkillCost <= currentCardPoint) {

            // 
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            player.cardPoint = currentCardPoint;
            uiManeger.SetPlayerCardPointText(player.cardPoint);

            // 
            enemyAIPlayer.score = skillManeger.UpScore(enemyAIPlayer.score);
            uiManeger.SetEnemyScoreText(enemyAIPlayer.score);
           
          
            SkillPanel.SetActive(false);
        }
      
    }

    public void OnBackButtonInSkillPanel() {
       
        SkillPanel.SetActive(false);
    }
}
