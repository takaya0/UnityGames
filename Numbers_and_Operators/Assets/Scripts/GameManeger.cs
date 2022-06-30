using System.Collections;using System.Collections.Generic;using UnityEngine;using TMPro;public class GameManeger : MonoBehaviour{       [SerializeField] CardController CardPrefab;    [SerializeField] Transform EnemyNumbersHandTransform, EnemyOperatorsHandTransform;
    // 敵側の手札
    [SerializeField] Transform PlayerNumbersHandTransform, PlayerOperatorsHandTransform;
    // プレイヤーの手札
    [SerializeField] Transform EnemySelectedCardsTransform, PlayerSelectedCardsTransform;    // 敵の選んだカード        [SerializeField] TextMeshProUGUI EnemyNumber, PlayerNumber;    // それぞれの数字    [SerializeField] TextMeshProUGUI TargetValueText;    // 目標の数字    private List<string> allNumbersCardList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8" };    private List<string> allOperatorsCardList = new List<string> { "Plus", "Minus", "Product" };    private bool IsPlayerTurn = true;    void Start(){        AddinitialCards(3, 3);        TargetValueText.text = GetTargetValue(10, 30).ToString();          }    void EnemyTurn() {
        

        CardController [] operatorsCardList = EnemyOperatorsHandTransform.GetComponentsInChildren<CardController>();
        CardController [] numbersCardList = EnemyNumbersHandTransform.GetComponentsInChildren<CardController>();

        // カードを選ぶ
        CardController operatorCard = operatorsCardList[0];
        CardController numberCard = numbersCardList[0];
       

        // カードを配置する
        operatorCard.movement.SetCardTransform(EnemySelectedCardsTransform);
        numberCard.movement.SetCardTransform(EnemySelectedCardsTransform);


        CaluculateScore(EnemySelectedCardsTransform);
       
    }    void GameTurnFlow() {
        if (IsPlayerTurn) {
            // プレイヤーの行動処理           
           
        }
        else {
            Debug.Log(IsPlayerTurn);
            EnemyTurn();
            if (IsGameFinished()) {
                Debug.Log("enemyの勝利");
            }        }
    }    public void OnCaluculateButton() {
        if (IsPlayerTurn) {
            CaluculateScore(PlayerSelectedCardsTransform);
            if (IsGameFinished()) {
                Debug.Log("playerの勝利");

            }
        }
    }    private void CaluculateScore(Transform SelectedCardsTransform) {

        List<CardController> selectedCards = GetSelectedCards(SelectedCardsTransform);
        if(selectedCards.Count != 2) {
            Debug.LogWarning("2まいのカードを選択してください");
        }

        CardController operatorCard = selectedCards[0];
        CardController numberCard = selectedCards[1];
        if (IsPlayerTurn) PlayerNumber.text = EvaluateFormula(PlayerNumber.text, operatorCard.card.value, numberCard.card.value).ToString();
        else EnemyNumber.text = EvaluateFormula(EnemyNumber.text, operatorCard.card.value, numberCard.card.value).ToString();

        operatorCard.Vanish();
        numberCard.Vanish();
        // カードを引く
        string OpetatorCardName = DrawCard(allOperatorsCardList);
        string NumberCardName = DrawCard(allNumbersCardList);
        if (IsPlayerTurn) {
            AddCardToHand(PlayerOperatorsHandTransform, OpetatorCardName);
            AddCardToHand(PlayerNumbersHandTransform, NumberCardName);
        }
        else {
            AddCardToHand(EnemyOperatorsHandTransform, OpetatorCardName);
            AddCardToHand(EnemyNumbersHandTransform, NumberCardName);
        }

        IsPlayerTurn = !IsPlayerTurn;
        GameTurnFlow();
    }    private List<CardController> GetSelectedCards(Transform SelectedCardTransform) {
        List<CardController> selectedCards = new List<CardController>(SelectedCardTransform.GetComponentsInChildren<CardController>());
        return selectedCards;
    }    // Update is called once per frame    void Update(){           }    private void AddinitialCards(int NumbersCardNum, int OperatorsCardNum) {        for(int i = 0; i < NumbersCardNum; i++) {            string NumberCardName = DrawCard(allNumbersCardList);            AddCardToHand(PlayerNumbersHandTransform, NumberCardName);            NumberCardName = DrawCard(allNumbersCardList);            AddCardToHand(EnemyNumbersHandTransform ,NumberCardName);        }        for(int i = 0; i < OperatorsCardNum; i++) {            string OpetatorCardName = DrawCard(allOperatorsCardList);            AddCardToHand(PlayerOperatorsHandTransform, OpetatorCardName);            OpetatorCardName = DrawCard(allOperatorsCardList);            AddCardToHand(EnemyOperatorsHandTransform , OpetatorCardName);        }    }      private void AddCardToHand(Transform Hand, string cardName) {        CardController card = Instantiate(CardPrefab, Hand, false);        card.Init(cardName);    }    private string DrawCard(List<string> CardList) {        int _index = Random.Range(0, CardList.Count);        return CardList[_index];    }    private int GetTargetValue(int minValue, int maxValue) {        int TargetValue = Random.Range(minValue, maxValue);        return TargetValue;    }    private bool IsGameFinished() {        int TargetValue = 0;        int Score = 0;        try {            TargetValue = int.Parse(TargetValueText.text);            if (IsPlayerTurn) Score = int.Parse(PlayerNumber.text);            else Score = int.Parse(EnemyNumber.text);        } catch (System.FormatException) {            Debug.Log("Invalid Value");            Debug.Log(TargetValueText.text);        }        if (Score - TargetValue == 0) return true;        else return false;    }    private int EvaluateFormula(string correntScore, string selectedOperator, string selectedNumber) {
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
        else Debug.LogWarning("Unsupported Operator");

        return result;
    }}