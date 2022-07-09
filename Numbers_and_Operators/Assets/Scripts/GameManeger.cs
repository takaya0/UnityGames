using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Constraints;


public class GameManeger : MonoBehaviour{
  

    [SerializeField] CardController CardPrefab;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] TextMeshProUGUI ResultText;

    [SerializeField] RuleBasedAI enemyAIPlayer;
    [SerializeField] UIManeger uiManeger;


    [SerializeField] GameObject SkillPanel;

    // 敵側の手札
    public Transform EnemyNumbersHandTransform, EnemyOperatorsHandTransform;

    // プレイヤーの手札
    public Transform PlayerNumbersHandTransform, PlayerOperatorsHandTransform;

    // 選んだカード
    public Transform EnemySelectedCardsTransform, PlayerSelectedCardsTransform;

    // カードポイント
    public int enemyCardPoint, playerCardPoint;

    List<string> OperatorsCardList = new List<string>(Const.OperatorsCardList);
    List<string> NumbersCardList = new List<string>(Const.NumbersCardList);
   
    public bool IsPlayerTurn = true;

    // ターゲットの数字
    public int targetScore;
    // それぞれの数字
    public int playerScore = 1, enemyScore = 1;


    void Start(){
        ResultPanel.SetActive(false);
        AddInitCards(Const.INIT_OPERATER_CARD_NUM, Const.INIT_NUMBER_CARD_NUM);
        targetScore = GetTargetScore(Const.MIN_TARGET_VALUE, Const.MAX_TARGET_VALUE);
        uiManeger.SetTargetScoreText(targetScore);

        // 初期カードポイントの設定 & UIへの反映
        enemyCardPoint = 2;
        uiManeger.SetEnemyCardPointText(enemyCardPoint);
        playerCardPoint = 2;
        uiManeger.SetPlayerCardPointText(playerCardPoint);
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
            
            if (IsGameFinished()) ShowResultPanel();
            else {
                // カードを引く
                string opetatorCardName = DrawCard(OperatorsCardList);
                string numberCardName = DrawCard(NumbersCardList);
                AddCardToHand(EnemyOperatorsHandTransform, opetatorCardName);
                AddCardToHand(EnemyNumbersHandTransform, numberCardName);
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
                playerScore = CaluculateScore(playerScore, selectedCards);
                uiManeger.SetPlayerScoreText(playerScore);
                foreach (CardController card in selectedCards) card.Vanish();

                // ゲームが終了したら、結果パネルを出す
                if (IsGameFinished()) ShowResultPanel();
                else {
                    // カードを引く
                    string opetatorCardName = DrawCard(OperatorsCardList);
                    string numberCardName = DrawCard(NumbersCardList);
                    AddCardToHand(PlayerOperatorsHandTransform, opetatorCardName);
                    AddCardToHand(PlayerNumbersHandTransform, numberCardName);

                    IsPlayerTurn = !IsPlayerTurn;
                    GameTurnFlow();

                }
            } else {
                for (int i = 0; i < selectedCards.Count; i++) {
                    string kind = selectedCards[i].card.kind;
                    if (kind == "number") {
                        if (IsPlayerTurn) selectedCards[i].movement.SetCardTransform(PlayerNumbersHandTransform);
                        else selectedCards[i].movement.SetCardTransform(EnemyNumbersHandTransform);
                    } else {
                        if (IsPlayerTurn) selectedCards[i].movement.SetCardTransform(PlayerOperatorsHandTransform);
                        else selectedCards[i].movement.SetCardTransform(EnemyOperatorsHandTransform);
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
        // 選んだカードで演算して、UIに反映  

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
            playerCardPoint = Mathf.Min(Const.MAX_CARD_POINT, playerCardPoint + 2);
            uiManeger.SetPlayerCardPointText(playerCardPoint);
        
        }
        else {
           enemyCardPoint = Mathf.Min(Const.MAX_CARD_POINT, enemyCardPoint + 2);
            uiManeger.SetEnemyCardPointText(enemyCardPoint);
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
            AddCardToHand(PlayerNumbersHandTransform, NumberCardName);
            NumberCardName = DrawCard(NumbersCardList);
            AddCardToHand(EnemyNumbersHandTransform ,NumberCardName);
        }

        for(int i = 0; i < OperatorsCardNum; i++) {
            string OpetatorCardName = DrawCard(OperatorsCardList);
            AddCardToHand(PlayerOperatorsHandTransform, OpetatorCardName);
            OpetatorCardName = DrawCard(OperatorsCardList);
            AddCardToHand(EnemyOperatorsHandTransform , OpetatorCardName);
        }
    }
  
    public void AddCardToHand(Transform Hand, string cardName) {
        CardController card = Instantiate(CardPrefab, Hand, false);
        card.Init(cardName);
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

    private bool IsGameFinished() {
        int TargetValue = 0;
        int Score = 1;
        if (Score - TargetValue == 0) return true;
        else return false;

    }
    public void OnSkillButton() {
        if(IsPlayerTurn) SkillPanel.SetActive(true);
    }

}
