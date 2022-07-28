using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Constraints;


public class GameManeger : MonoBehaviour{
  

    [SerializeField] CardController CardPrefab;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] TextMeshProUGUI ResultText;

    [SerializeField] PlayerManeger player;
    [SerializeField] RuleBasedAI enemyAIPlayer;
    [SerializeField] UIManeger uiManeger;


    [SerializeField] GameObject SkillPanel;

  
    // 選んだカード
    public Transform EnemySelectedCardsTransform, PlayerSelectedCardsTransform;

    List<string> OperatorsCardList = new List<string>(Const.OperatorsCardList);
    List<string> NumbersCardList = new List<string>(Const.NumbersCardList);
   
    public bool IsPlayerTurn = true;

    // ターゲットの数字
    public int targetScore;

 
  


    void Start(){
        ResultPanel.SetActive(false);
        AddInitCards(Const.INIT_OPERATER_CARD_NUM, Const.INIT_NUMBER_CARD_NUM);
        targetScore = GetTargetScore(Const.MIN_TARGET_VALUE, Const.MAX_TARGET_VALUE);
        uiManeger.SetTargetScoreText(targetScore);

        // 初期カードポイントのUIへの反映
        uiManeger.SetEnemyCardPointText(enemyAIPlayer.cardPoint);
        uiManeger.SetPlayerCardPointText(player.cardPoint);
    }

    private void GameTurnFlow() {
        if (IsPlayerTurn) {
            // プレイヤーの行動処理
            CardPointTwoUp();


        } else {
            CardPointTwoUp();
            //EnemyTurn();
            StopAllCoroutines();
            StartCoroutine(enemyAIPlayer.EnemyActions());
            
            if (IsGameFinished(enemyAIPlayer.score)) ShowResultPanel();
            else {
                // カードを引く
                string opetatorCardName = DrawCard(OperatorsCardList);
                string numberCardName = DrawCard(NumbersCardList);
                AddCardToHand(enemyAIPlayer.operatorsHandTransform, opetatorCardName, false);
                AddCardToHand(enemyAIPlayer.numbersHandTransform, numberCardName, false);
                IsPlayerTurn = !IsPlayerTurn;
                GameTurnFlow();

            }

        }
    }

    public void OnCaluculateButton() {
        if (IsPlayerTurn) {

            // 選ばれたカードを取得
            List<CardController> selectedCards = GetSelectedCards(PlayerSelectedCardsTransform);

            if (CheckSelectedCards(selectedCards)) {
                player.score = CaluculateScore(player.score, selectedCards);
                uiManeger.SetPlayerScoreText(player.score);
                foreach (CardController card in selectedCards) card.Vanish();

                // ゲームが終了したら、結果パネルを出す
                if (IsGameFinished(player.score)) ShowResultPanel();
                else {
                    // カードを引く
                    string opetatorCardName = DrawCard(OperatorsCardList);
                    string numberCardName = DrawCard(NumbersCardList);
                    AddCardToHand(player.operatorsHandTransform, opetatorCardName, true);
                    AddCardToHand(player.numbersHandTransform, numberCardName, true);

                    IsPlayerTurn = !IsPlayerTurn;
                    GameTurnFlow();
                }
            } else {
                for (int i = 0; i < selectedCards.Count; i++) {
                    string kind = selectedCards[i].card.kind;
                    if (kind == "number") {
                        if (IsPlayerTurn) selectedCards[i].movement.SetCardTransform(player.numbersHandTransform);
                        else {
                            selectedCards[i].movement.SetCardTransform(enemyAIPlayer.numbersHandTransform);
                            selectedCards[i].view.SetDisActiveMask();
                        } 
                    } else {
                        if (IsPlayerTurn) selectedCards[i].movement.SetCardTransform(player.operatorsHandTransform);
                        else {
                            selectedCards[i].movement.SetCardTransform(enemyAIPlayer.operatorsHandTransform);
                            selectedCards[i].view.SetDisActiveMask();
                        } 
                    }
                }
            }
           
        }
    }


    private string GetFormulaFromSelectedCards(string currentNumber,List<CardController> selectedCards) {
        string formula = currentNumber;

        foreach(CardController card in selectedCards) {
            string value = card.card.value;
            formula += value;
        }

        return formula;
    }

    public int CaluculateScore(int currentScore, List<CardController> selectedCards) {
      
        // 式を作成
        string formula = GetFormulaFromSelectedCards(currentScore.ToString(), selectedCards);
        Debug.Log(formula);
        // 文字数式を評価してscoreに格納
        ExpressionEvaluator.Evaluate(formula, out int score);
        // スコアが0以下なら1にする
        score = Mathf.Max(1, score);
        return score;
    }

    private void CardPointTwoUp() {
        if (IsPlayerTurn) {
            player.cardPoint = Mathf.Min(Const.MAX_CARD_POINT, player.cardPoint + 2);
            uiManeger.SetPlayerCardPointText(player.cardPoint);  
        } else {
           enemyAIPlayer.cardPoint = Mathf.Min(Const.MAX_CARD_POINT, enemyAIPlayer.cardPoint + 2);
            uiManeger.SetEnemyCardPointText(enemyAIPlayer.cardPoint);
        }
    }

    private bool CheckSelectedCards(List<CardController> selectedCards) {

        //　2枚 or 4枚選ばれているか確認
        if(selectedCards.Count != 2 && selectedCards.Count != 4) return false;


        // 正しい順番で配置されているかどうか確認

   
        string[] rightCardOrder = { "operator", "number" };
        for(int i = 0; i < selectedCards.Count; i++) {
            string kind = selectedCards[i].card.kind;
            if(kind != rightCardOrder[i % 2]) return false;
        }

        return true;

    }

    private void ShowResultPanel() {
        ResultPanel.SetActive(true);
        if (IsPlayerTurn) ResultText.text = "Player WIN";
        else ResultText.text = "Player LOSE";
    }

    public List<CardController> GetSelectedCards(Transform SelectedCardTransform) {
        List<CardController> selectedCards = new List<CardController>(SelectedCardTransform.GetComponentsInChildren<CardController>());
        return selectedCards;
    }

    private void AddInitCards(int NumbersCardNum, int OperatorsCardNum) {
        for(int i = 0; i < NumbersCardNum; i++) {
            string NumberCardName = DrawCard(NumbersCardList);
            AddCardToHand(player.numbersHandTransform, NumberCardName, true);
            NumberCardName = DrawCard(NumbersCardList);
            AddCardToHand(enemyAIPlayer.numbersHandTransform ,NumberCardName, false);
        }

        for(int i = 0; i < OperatorsCardNum; i++) {
            string OpetatorCardName = DrawCard(OperatorsCardList);
            AddCardToHand(player.operatorsHandTransform, OpetatorCardName, true);
            OpetatorCardName = DrawCard(OperatorsCardList);
            AddCardToHand(enemyAIPlayer.operatorsHandTransform , OpetatorCardName, false);
        }
    }
  
    public void AddCardToHand(Transform Hand, string cardName, bool isPlayerCard) {
        CardController card = Instantiate(CardPrefab, Hand, false);
        card.Init(cardName, isPlayerCard);
    }

    public string DrawCard(List<string> CardList) {
        int _index = Random.Range(0, CardList.Count);
        return CardList[_index];
    }

    private int GetTargetScore(int minValue, int maxValue) {
        // 目標値をランダムに生成(両端を含む)
        int _targetScore = Random.Range(minValue, maxValue + 1);
        return _targetScore;
    }

    private bool IsGameFinished(int currentScore) {
        return currentScore == targetScore;

    }
    public void OnSkillButton() {
        if(IsPlayerTurn) SkillPanel.SetActive(true);
    }

}
