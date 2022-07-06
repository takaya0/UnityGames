using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        skillManeger.ExchangeEachNumbers(gameManeger.PlayerCardPointText, gameManeger.playerCardPoint);
        SkillPanel.SetActive(false);
    }

    public void OnDrawButtonInSkillPanel() {
        skillManeger.DrawCards(gameManeger.PlayerOperatorsHandTransform, gameManeger.PlayerNumbersHandTransform, gameManeger.PlayerCardPointText, gameManeger.playerCardPoint);
        SkillPanel.SetActive(false);
    }

    public void OnDownButtonInSkillPanel() {
        skillManeger.DownCardPoint(gameManeger.EnemyNumber, gameManeger.PlayerCardPointText, gameManeger.playerCardPoint);
        SkillPanel.SetActive(false);
    }

    public void OnUpButtonInSkillPanel() {
        skillManeger.UpCardPoint(gameManeger.EnemyNumber, gameManeger.PlayerCardPointText, gameManeger.playerCardPoint);
        SkillPanel.SetActive(false);
    }

    public void OnBackButtonInSkillPanel() {
       
        SkillPanel.SetActive(false);
    }
}
