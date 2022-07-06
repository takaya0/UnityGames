using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using Constraints;


public class GameManeger : MonoBehaviour{
  

    [SerializeField] CardController CardPrefab;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] TextMeshProUGUI ResultText;

    [SerializeField] GameObject SkillPanel;

    // 敵側の手札
    public Transform EnemyNumbersHandTransform, EnemyOperatorsHandTransform;

    // プレイヤーの手札
    public Transform PlayerNumbersHandTransform, PlayerOperatorsHandTransform;

    // 敵の選んだカード
    [SerializeField] Transform EnemySelectedCardsTransform, PlayerSelectedCardsTransform;

    // お互いのカードポイントテキスト
    public TextMeshProUGUI EnemyCardPointText, PlayerCardPointText;




    // それぞれの数字
    public TextMeshProUGUI EnemyNumber, PlayerNumber;

    // 目標の数字
    [SerializeField] TextMeshProUGUI TargetValueText;
   


    // カードポイント
    public int enemyCardPoint, playerCardPoint;

    List<string> OperatorsCardList = new List<string>(Const.OperatorsCardList);
    List<string> NumbersCardList = new List<string>(Const.NumbersCardList);
   


   




    public bool IsPlayerTurn = true;

    void Start(){
        ResultPanel.SetActive(false);
        AddinitialCards(3, 3);
        TargetValueText.text = GetTargetValue(50, 100).ToString();

        // 初期カードポイントの設定 & UIへの反映
        enemyCardPoint = 2;
        ApplyCardPointToUI(EnemyCardPointText, enemyCardPoint);
        playerCardPoint = 2;
        ApplyCardPointToUI(PlayerCardPointText, playerCardPoint);
    }

    private void EnemyTurn() {
        CardController [] operatorsCardList = EnemyOperatorsHandTransform.GetComponentsInChildren<CardController>();
        CardController [] numbersCardList = EnemyNumbersHandTransform.GetComponentsInChildren<CardController>();

        // カードを選ぶ
        CardController operatorCard = operatorsCardList[Random.Range(0, operatorsCardList.Length)];
        CardController numberCard = numbersCardList[Random.Range(0, numbersCardList.Length)];
       

        // カードを配置する
        operatorCard.movement.SetCardTransform(EnemySelectedCardsTransform);
        numberCard.movement.SetCardTransform(EnemySelectedCardsTransform);

        List<CardController> selectedCards = GetSelectedCards(EnemySelectedCardsTransform);
        CaluculateScore(selectedCards);
    }

    private void GameTurnFlow() {
        if (IsPlayerTurn) {
            // プレイヤーの行動処理
            CardPointTwoUp();


        } else {
            CardPointTwoUp();
            EnemyTurn();
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
                CaluculateScore(selectedCards);

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

    private void CaluculateScore(List<CardController> selectedCards) {


        // 選んだカードで演算して、UIに反映  
        if (IsPlayerTurn) {
            string formula = GetFormulaFromSelectedCards(PlayerNumber.text, selectedCards);

            // 文字数式を評価してresultに格納
            ExpressionEvaluator.Evaluate(formula, out int result);

            // スコアが0以下なら1にする
            result = Mathf.Max(1, result);
            // UIに反映
            PlayerNumber.text = result.ToString();
        } else {
            string formula = GetFormulaFromSelectedCards(EnemyNumber.text, selectedCards);

            // 文字数式を評価してresultに格納
            ExpressionEvaluator.Evaluate(formula, out int result);

            // スコアが0以下なら1にする
            result = Mathf.Max(1, result);

            // UIに反映
            EnemyNumber.text = result.ToString();
        }

        // カードを消去
        foreach (CardController card in selectedCards) card.Vanish();
        
    }

    

    private void CardPointTwoUp() {
        if (IsPlayerTurn) {
            playerCardPoint = Mathf.Min(Const.MAX_CARD_POINT, playerCardPoint + 2);
            ApplyCardPointToUI(PlayerCardPointText, playerCardPoint);
        
        }
        else {
            enemyCardPoint = Mathf.Min(Const.MAX_CARD_POINT, enemyCardPoint + 2);
           ApplyCardPointToUI(EnemyCardPointText, enemyCardPoint);
        }
    }

    public void ApplyCardPointToUI(TextMeshProUGUI cardPointText, int cardPoint) {
        cardPointText.text = "CP : " + cardPoint.ToString() + "/" + Const.MAX_CARD_POINT.ToString();
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

    private List<CardController> GetSelectedCards(Transform SelectedCardTransform) {
        List<CardController> selectedCards = new List<CardController>(SelectedCardTransform.GetComponentsInChildren<CardController>());
        return selectedCards;
    }

  

    private void AddinitialCards(int NumbersCardNum, int OperatorsCardNum) {
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

    private int GetTargetValue(int minValue, int maxValue) {
        int TargetValue = Random.Range(minValue, maxValue);
        return TargetValue;
    }

    private bool IsGameFinished() {
        int TargetValue = 0;
        int Score = 0;
        try {
            TargetValue = int.Parse(TargetValueText.text);
            if (IsPlayerTurn) Score = int.Parse(PlayerNumber.text);
            else Score = int.Parse(EnemyNumber.text);
        } catch (System.FormatException) {
            Debug.Log("Invalid Value");
            Debug.Log(TargetValueText.text);
        }


        if (Score - TargetValue == 0) return true;
        else return false;

    }

  

    public void OnRestartButton() {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackToTitleButton() {
        SceneManager.LoadScene("TitleScene");
    }

    

    public void OnSkillButton() {
        if(IsPlayerTurn) SkillPanel.SetActive(true);
    }
    public int IntParser(string numStr) {
        int numInt = 0;
        try {
            numInt = int.Parse(numStr);
        }
        catch (System.FormatException) {
            Debug.Log("Invalid Value : " + numStr);
        }

        return numInt;
    }

}
