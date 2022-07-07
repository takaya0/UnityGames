using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constraints;

public class SkillPanelManeger : MonoBehaviour
{
    [SerializeField] GameObject SkillPanel;
    private GameManeger gameManeger;
    private SkillManeger skillManeger;

    private void Start() {
        skillManeger = GameObject.Find("SkillManeger").GetComponent<SkillManeger>();
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
    }

    public void OnExchangeButtonInSkillPanel() {
        int currentCardPoint = gameManeger.playerCardPoint;
        if(SkillCost.exchangeSkillCost <= currentCardPoint) {
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.exchangeSkillCost);
            gameManeger.playerCardPoint = currentCardPoint;

            gameManeger.ApplyCardPointToUI(gameManeger.PlayerCardPointText, currentCardPoint);
            skillManeger.ExchangeEachNumbers();
            SkillPanel.SetActive(false);
        }
       
    }

    public void OnDrawButtonInSkillPanel() {

        int currentCardPoint = gameManeger.playerCardPoint;
        if (SkillCost.drawSkillCost <= currentCardPoint) {
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            gameManeger.playerCardPoint = currentCardPoint;

            gameManeger.ApplyCardPointToUI(gameManeger.PlayerCardPointText, currentCardPoint);

            skillManeger.DrawCards(gameManeger.PlayerOperatorsHandTransform, gameManeger.PlayerNumbersHandTransform);
            SkillPanel.SetActive(false);
        }
    }

    public void OnDownButtonInSkillPanel() {
        int currentCardPoint = gameManeger.playerCardPoint;
        if(SkillCost.downSkillCost <= currentCardPoint) {

            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            gameManeger.playerCardPoint = currentCardPoint;

            gameManeger.ApplyCardPointToUI(gameManeger.PlayerCardPointText, currentCardPoint);

            skillManeger.DownCardPoint(gameManeger.EnemyNumber);
            SkillPanel.SetActive(false);

        }
       
    }

    public void OnUpButtonInSkillPanel() {
        int currentCardPoint = gameManeger.playerCardPoint;
        if( SkillCost.upSkillCost <= currentCardPoint) {
            currentCardPoint = skillManeger.DecreaseCardPoint(currentCardPoint, SkillCost.drawSkillCost);
            gameManeger.playerCardPoint = currentCardPoint;

            gameManeger.ApplyCardPointToUI(gameManeger.PlayerCardPointText, currentCardPoint);
            skillManeger.UpCardPoint(gameManeger.EnemyNumber);
            SkillPanel.SetActive(false);
        }
      
    }

    public void OnBackButtonInSkillPanel() {
       
        SkillPanel.SetActive(false);
    }
}
