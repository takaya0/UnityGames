using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManeger : MonoBehaviour{
   

    [SerializeField] CardController CardPrefab;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] TextMeshProUGUI ResultText;
    [SerializeField] Transform EnemyNumbersHandTransform, EnemyOperatorsHandTransform;
    // 敵側の手札
    [SerializeField] Transform PlayerNumbersHandTransform, PlayerOperatorsHandTransform;
    // プレイヤーの手札
    [SerializeField] Transform EnemySelectedCardsTransform, PlayerSelectedCardsTransform;
    // 敵の選んだカード
    

    [SerializeField] TextMeshProUGUI EnemyNumber, PlayerNumber;
    // それぞれの数字

    [SerializeField] TextMeshProUGUI TargetValueText;
    // 目標の数字


    private List<string> allNumbersCardList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8" };
    private List<string> allOperatorsCardList = new List<string> { "Plus", "Minus", "Product" , "Quotient"};




    private bool IsPlayerTurn = true;

    void Start(){
        ResultPanel.SetActive(false);
        AddinitialCards(3, 4);
        TargetValueText.text = GetTargetValue(10, 50).ToString();
      
    }

    void EnemyTurn() {
        

        CardController [] operatorsCardList = EnemyOperatorsHandTransform.GetComponentsInChildren<CardController>();
        CardController [] numbersCardList = EnemyNumbersHandTransform.GetComponentsInChildren<CardController>();

        // カードを選ぶ
        CardController operatorCard = operatorsCardList[0];
        CardController numberCard = numbersCardList[0];
       

        // カードを配置する
        operatorCard.movement.SetCardTransform(EnemySelectedCardsTransform);
        numberCard.movement.SetCardTransform(EnemySelectedCardsTransform);

        List<CardController> selectedCards = GetSelectedCards(EnemySelectedCardsTransform);
        CaluculateScore(selectedCards);
    }


    void GameTurnFlow() {
        if (IsPlayerTurn) {
            // プレイヤーの行動処理
           
           
        }
        else {
            
            EnemyTurn();
            if (IsGameFinished()) ShowResulttPanel();
            else {
                // カードを引く
                string OpetatorCardName = DrawCard(allOperatorsCardList);
                string NumberCardName = DrawCard(allNumbersCardList);
                AddCardToHand(EnemyOperatorsHandTransform, OpetatorCardName);
                AddCardToHand(EnemyNumbersHandTransform, NumberCardName);
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
                if (IsGameFinished()) ShowResulttPanel();
                else {
                    // カードを引く
                    string OpetatorCardName = DrawCard(allOperatorsCardList);
                    string NumberCardName = DrawCard(allNumbersCardList);
                    AddCardToHand(PlayerOperatorsHandTransform, OpetatorCardName);
                    AddCardToHand(PlayerNumbersHandTransform, NumberCardName);

                    IsPlayerTurn = !IsPlayerTurn;
                    GameTurnFlow();

                }
            }
            else {
                for (int i = 0; i < selectedCards.Count; i++) {
                    string kind = selectedCards[i].card.kind;
                    if (kind == "number") {
                        if (IsPlayerTurn) selectedCards[i].movement.SetCardTransform(PlayerNumbersHandTransform);
                        else selectedCards[i].movement.SetCardTransform(EnemyNumbersHandTransform);
                    }
                    else {
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
            }
            else if(kind == "operator"){
               operatorCard = selectedCards[i];
            }
            else {
                Debug.LogWarning("Invalid card kind");
            }
           

        }
           
        if (IsPlayerTurn) PlayerNumber.text = EvaluateFormula(PlayerNumber.text, operatorCard.card.value, numberCard.card.value).ToString();
        else EnemyNumber.text = EvaluateFormula(EnemyNumber.text, operatorCard.card.value, numberCard.card.value).ToString();

        operatorCard.Vanish();
        numberCard.Vanish();
        
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

    private void ShowResulttPanel() {
        ResultPanel.SetActive(true);
        if (IsPlayerTurn) ResultText.text = "Player WIN";
        else ResultText.text = "Player LOSE";
    }

    private List<CardController> GetSelectedCards(Transform SelectedCardTransform) {
        List<CardController> selectedCards = new List<CardController>(SelectedCardTransform.GetComponentsInChildren<CardController>());
        return selectedCards;
    }

    // Update is called once per frame
    void Update(){


       
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
}
