using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManeger : MonoBehaviour{
   

    [SerializeField] CardController CardPrefab;
    [SerializeField] Transform PlayerHandTransform;
    [SerializeField] Transform EnemyHandTransform;

    [SerializeField] TextMeshProUGUI PlayerNumber;
    [SerializeField] TextMeshProUGUI EnemyNumber;

    [SerializeField] TextMeshProUGUI TargetValueText;


    private List<string> NumbersCardList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8" };
    private List<string> OperatorsCardList = new List<string> { "Plus", "Minus", "Product" };


    private EnemyPlayer enemyplayer = new EnemyPlayer();


    private bool IsPlayerTurn = true;

    void Start(){
        AddinitialCards(4, 3);
        TargetValueText.text = GetTargetValue(10, 100).ToString();
      
    }

    // Update is called once per frame
    void Update(){


        if (IsPlayerTurn) {
            // プレイヤーの行動処理


            IsGameFinished();
            IsPlayerTurn = false;
        } else {
            // 敵プレイヤー(AI)の行動処理
            System.Tuple<GameObject, GameObject> SelectedCards = enemyplayer.SelectEnemyCards(EnemyHandTransform);
            GameObject NumberCard = SelectedCards.Item1;
            GameObject OperatorCard = SelectedCards.Item2;

            // 手札のカードリスト取得
            CardController[] cardList = EnemyHandTransform.GetComponentsInChildren<CardController>();



            IsGameFinished();
            IsPlayerTurn = true;
        }
    }




    private void AddinitialCards(int NumbersCardNum, int OperatorsCardNum) {
        for(int i = 0; i < NumbersCardNum; i++) {
            string cardName = DrawCard(NumbersCardList);
            AddCardToPlayerHand(cardName);
            cardName = DrawCard(NumbersCardList);
            AddCardToEnemyHand(cardName);
        }

        for(int i = 0; i < OperatorsCardNum; i++) {
            string cardName = DrawCard(OperatorsCardList);
            AddCardToPlayerHand(cardName);
            cardName = DrawCard(OperatorsCardList);
            AddCardToEnemyHand(cardName);
        }
    }

    private void AddCardToPlayerHand(string cardName) {
        CardController card = Instantiate(CardPrefab, PlayerHandTransform, false);
        card.Init(cardName);
    }

    private void AddCardToEnemyHand(string cardName) {
        CardController card = Instantiate(CardPrefab, EnemyHandTransform, false);
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
}
