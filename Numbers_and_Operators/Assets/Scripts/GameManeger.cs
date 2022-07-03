using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManeger : MonoBehaviour{


    // コストポイントの最大値
    const int MAX_COST_POINT = 8;
   

    [SerializeField] CardController CardPrefab;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] TextMeshProUGUI ResultText;

    // 敵側の手札
    [SerializeField] Transform EnemyNumbersHandTransform, EnemyOperatorsHandTransform;

    // プレイヤーの手札
    [SerializeField] Transform PlayerNumbersHandTransform, PlayerOperatorsHandTransform;

    // 敵の選んだカード
    [SerializeField] Transform EnemySelectedCardsTransform, PlayerSelectedCardsTransform;

    // お互いのコストポイントテキスト
    [SerializeField] TextMeshProUGUI EnemyCostPointText, PlayerCostPointText;




    // それぞれの数字
    [SerializeField] TextMeshProUGUI EnemyNumber, PlayerNumber;

    // 目標の数字
    [SerializeField] TextMeshProUGUI TargetValueText;
   

    [SerializeField] GameObject ExchangeButton;


    private List<string> allNumbersCardList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8" };
    private List<string> allOperatorsCardList = new List<string> { "Plus", "Minus", "Product" , "Quotient"};


    // カードの重み
    private List<float> OperatorCardWeight = new List<float> { 0.25f, 0.25f, 0.25f, 0.25f };

    // コストポイント
    public int enemyCostPoint, playerCostPoint;




    private bool IsPlayerTurn = true;

    void Start(){
        ResultPanel.SetActive(false);
        AddinitialCards(4, 4);
        TargetValueText.text = GetTargetValue(10, 50).ToString();

        // 初期コストポイントの設定 & UIへの反映
        enemyCostPoint = 2;
        ApplyCostPointToUI(EnemyCostPointText, enemyCostPoint);
        playerCostPoint = 2;
        ApplyCostPointToUI(PlayerCostPointText, playerCostPoint);
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
            CostPointTwoUp();


        } else {
            CostPointTwoUp();
            EnemyTurn();
            if (IsGameFinished()) ShowResultPanel();
            else {
                // カードを引く
                string opetatorCardName = DrawCard(allOperatorsCardList);
                string numberCardName = DrawCard(allNumbersCardList);
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
                    string opetatorCardName = DrawCard(allOperatorsCardList);
                    string numberCardName = DrawCard(allNumbersCardList);
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

    private void CaluculateScore(List<CardController> selectedCards) {

        CardController operatorCard = default;
        CardController numberCard = default;
        for (int i = 0; i < selectedCards.Count; i++) {
            string kind = selectedCards[i].card.kind;
            if (kind == "number") {
                numberCard = selectedCards[i];
            } else if (kind == "operator") {
               operatorCard = selectedCards[i];
            } else {
                Debug.LogWarning("Invalid card kind");
            }
        }
           
        if (IsPlayerTurn) PlayerNumber.text = EvaluateFormula(PlayerNumber.text, operatorCard.card.value, numberCard.card.value).ToString();
        else EnemyNumber.text = EvaluateFormula(EnemyNumber.text, operatorCard.card.value, numberCard.card.value).ToString();

        operatorCard.Vanish();
        numberCard.Vanish();
        
    }


    private void CostPointTwoUp() {
        if (IsPlayerTurn) {
            playerCostPoint = Mathf.Min(MAX_COST_POINT, playerCostPoint + 2);
            ApplyCostPointToUI(PlayerCostPointText, playerCostPoint);
        
        }
        else {
            enemyCostPoint = Mathf.Min(MAX_COST_POINT, enemyCostPoint + 2);
           ApplyCostPointToUI(EnemyCostPointText, enemyCostPoint);
        }
    }

    private void ApplyCostPointToUI(TextMeshProUGUI costPointText, int costPoint) {
        costPointText.text = "CP : " + costPoint.ToString();
    }


    private bool CheckSelectedCards(List<CardController> selectedCards) {

        //　2枚のみ選ばれているか確認
        if(selectedCards.Count != 2) return false;


        // 演算カード、数字のカードがそれぞれ1枚づつ選ばれたか確認
        HashSet<string> kinds = new HashSet<string>();
        for (int i = 0; i < selectedCards.Count; i++) {
            string kind = selectedCards[i].card.kind;
            kinds.Add(kind);
            if(kind != "number" && kind != "operator") return false;
        }

        if(kinds.Count != 2) return false;

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
            string NumberCardName = DrawCard(allNumbersCardList);
            AddCardToHand(PlayerNumbersHandTransform, NumberCardName);
            NumberCardName = DrawCard(allNumbersCardList);
            AddCardToHand(EnemyNumbersHandTransform ,NumberCardName);
        }

        for(int i = 0; i < OperatorsCardNum; i++) {
            string OpetatorCardName = DrawCard(allOperatorsCardList);
            AddCardToHand(PlayerOperatorsHandTransform, OpetatorCardName);
            OpetatorCardName = DrawCard(allOperatorsCardList);
            AddCardToHand(EnemyOperatorsHandTransform , OpetatorCardName);
        }
    }
  
    private void AddCardToHand(Transform Hand, string cardName) {
        CardController card = Instantiate(CardPrefab, Hand, false);
        card.Init(cardName);
    }


    private string DrawCardWithWeights(List<string> CardList, List<float> weights) {

        if(CardList.Count != weights.Count) {
            Debug.LogWarning("CardList.Count " + CardList.Count.ToString() + " do not equal to weights.Count " + weights.Count.ToString());
        }
        int _windex = CardList.Count - 1;
        float rnd = Random.Range(0.0f, 1.0f);
        float weightSum = 0.0f;

        for (int i = 0; i < weights.Count; i++) {
            if (weightSum <= rnd && rnd < weightSum + OperatorCardWeight[i]) _windex = i;
            weightSum += OperatorCardWeight[i];
        }

        return CardList[_windex];
    }
    private string DrawCard(List<string> CardList) {
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


    private int EvaluateFormula(string correntScore, string selectedOperator, string selectedNumber) {
        int result = 0;
        int selectedNum = 0;
        try {
            result = int.Parse(correntScore);
            selectedNum = int.Parse(selectedNumber);
        }
        catch (System.FormatException) {
            Debug.Log("Invalid Value");
        }

        if (selectedOperator == "+") result = result + selectedNum;
        else if (selectedOperator == "-") result = Mathf.Max(result - selectedNum, 1);
        else if (selectedOperator == "*") result = result * selectedNum;
        else if (selectedOperator == "//") result = Mathf.Max(result/selectedNum, 1);
        else Debug.LogWarning("Unsupported Operator");

        return result;
    }


    public void OnRestartButton() {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackToTitleButton() {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnExchangeButton() {
        if (IsPlayerTurn) {
            (EnemyNumber.text, PlayerNumber.text) = (PlayerNumber.text, EnemyNumber.text);
            ExchangeButton.SetActive(false);
        }
    }
        
}
