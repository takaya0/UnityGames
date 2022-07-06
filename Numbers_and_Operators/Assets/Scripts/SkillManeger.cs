using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Constraints;
public class SkillManeger : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManeger gameManeger;

    List<string> OperatorsCardList = new List<string>(Const.OperatorsCardList);
    List<string> NumbersCardList = new List<string>(Const.NumbersCardList);

    void Start() {
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
    }

    private int DecreaseCardPoint(int currentSkillPoint, int skillCost) {
        return currentSkillPoint - skillCost;
    }

    public void ExchangeEachNumbers(TextMeshProUGUI cardPointText, int currentcardPoint) {

        const int exchangeSkillPoint = 8;

        if (currentcardPoint >= exchangeSkillPoint) {
            // コストポイントを消費する
            currentcardPoint = DecreaseCardPoint(currentcardPoint, exchangeSkillPoint);
            gameManeger.ApplyCardPointToUI(cardPointText, currentcardPoint);

            // お互いの数字を入れ替える
            (gameManeger.EnemyNumber.text, gameManeger.PlayerNumber.text) = (gameManeger.PlayerNumber.text, gameManeger.EnemyNumber.text);


      
        }



    }
    public void DrawCards(Transform operatorHandTransform,Transform numbersHandTransform,TextMeshProUGUI cardPointText, int currentCardPoint) {

        const int drawSkillPoint = 4;

        int currentCardInHandNum = gameManeger.PlayerOperatorsHandTransform.GetComponentsInChildren<CardController>().Length;

        if (currentCardPoint >= drawSkillPoint && currentCardInHandNum < Const.MAX_HAND_NUM) {
            // コストポイントを消費する
            currentCardPoint = DecreaseCardPoint(currentCardPoint, drawSkillPoint);
            gameManeger.ApplyCardPointToUI(cardPointText, currentCardPoint);

            // カードを引く
            string opetatorCardName = gameManeger.DrawCard(OperatorsCardList);
            string numberCardName = gameManeger.DrawCard(NumbersCardList);
            gameManeger.AddCardToHand(operatorHandTransform, opetatorCardName);
            gameManeger.AddCardToHand(numbersHandTransform, numberCardName);


          
        }


    }


    public void DownCardPoint(TextMeshProUGUI number ,TextMeshProUGUI cardPointText, int currentCardPoint) {

        const int downSkillPoint = 6;



        if (currentCardPoint >= downSkillPoint) {
            // コストポイントを消費する
            currentCardPoint = DecreaseCardPoint(currentCardPoint, downSkillPoint);
            gameManeger.ApplyCardPointToUI(cardPointText, currentCardPoint);

            // 敵の数字をランダムに減少させる(from 10 to 30)
            number.text = (Mathf.Max(gameManeger.IntParser(number.text) - Random.Range(10, 31), 1)).ToString();


           
        }


    }

    public void UpCardPoint(TextMeshProUGUI number, TextMeshProUGUI cardPointText, int currentCardPoint) {

        const int downSkillPoint = 6;



        if (currentCardPoint >= downSkillPoint) {
            // コストポイントを消費する
            currentCardPoint = DecreaseCardPoint(currentCardPoint, downSkillPoint);
            gameManeger.ApplyCardPointToUI(cardPointText, currentCardPoint);

            // 敵の数字をランダムに減少させる(from 10 to 30)
            number.text = (Mathf.Max(gameManeger.IntParser(number.text) + Random.Range(10, 31), 1)).ToString();

        }


    }




}
