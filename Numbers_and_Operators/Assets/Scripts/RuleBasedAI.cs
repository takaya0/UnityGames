using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constraints;

public class RuleBasedAI : MonoBehaviour
{
   

    private GameManeger gameManeger;
    private SkillManeger skillManeger;

    public Transform operatorsHandTransform;
    public Transform numbersHandTransform;
    public int cardPoint;
    public int score;



   [SerializeField] UIManeger uiManeger;


   
    // Start is called before the first frame update
    void Start(){
        cardPoint = Const.INIT_CARD_POINT;
        score = Const.INIT_SCORE;
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        skillManeger = GameObject.Find("SkillManeger").GetComponent<SkillManeger>();
    }

    public IEnumerator EnemyActions() {
        CardController[] operatorsCardList = operatorsHandTransform.GetComponentsInChildren<CardController>();
        CardController[] numbersCardList = numbersHandTransform.GetComponentsInChildren<CardController>();


        List<CardController> selectedCards = SelectCardsByGreedyPolicy();

        // move cards to selected transform
        foreach (CardController card in selectedCards) {
            yield return new WaitForSeconds(0.5f);
            card.movement.SetCardTransform(gameManeger.EnemySelectedCardsTransform);
            card.view.SetDisActiveMask();
        }
      
        yield return new WaitForSeconds(0.75f);
        gameManeger.enemyAIPlayer.score = gameManeger.CaluculateScore(gameManeger.enemyAIPlayer.score, selectedCards);
       
        uiManeger.SetEnemyScoreText(gameManeger.enemyAIPlayer.score);

        if (gameManeger.IsGameFinished(gameManeger.enemyAIPlayer.score)) gameManeger.ShowResultPanel(false);

        foreach (CardController card in selectedCards) card.Vanish();
    }


    private List<CardController> SelectCardsByGreedyPolicy() {
        
        CardController[] operatorsCardList = operatorsHandTransform.GetComponentsInChildren<CardController>();
        CardController[] numbersCardList = numbersHandTransform.GetComponentsInChildren<CardController>();

        int currentCardNumInHand = operatorsCardList.Length;

        List<CardController> greedyCards = GetCurrentGreedyCards(operatorsCardList, numbersCardList);

        int _score = gameManeger.CaluculateScore(score, greedyCards);
        if (_score == gameManeger.targetScore) return greedyCards;

        if(gameManeger.enemyAIPlayer.cardPoint >= SkillCost.drawSkillCost && currentCardNumInHand < Const.MAX_HAND_NUM) {
            float rnd = Random.Range(0.0f, 1.0f);
            if(rnd <= 0.2) {
                Debug.Log("カードを引いたよ");
                gameManeger.enemyAIPlayer.cardPoint -= SkillCost.drawSkillCost;
                uiManeger.SetEnemyCardPointText(gameManeger.enemyAIPlayer.cardPoint);
                skillManeger.DrawCards(operatorsHandTransform, numbersHandTransform, false);
                operatorsCardList = operatorsHandTransform.GetComponentsInChildren<CardController>();
                numbersCardList = numbersHandTransform.GetComponentsInChildren<CardController>();
                greedyCards = GetCurrentGreedyCards(operatorsCardList, numbersCardList);
            }
        }

        return greedyCards;
    }


    private List<CardController> GetCurrentGreedyCards(CardController[] operatorsCardList, CardController[] numbersCardList) {
        int greedyScore = 50000000;

        List<CardController> greedyCards = new List<CardController>() { };

        int targetScore = gameManeger.targetScore;
        for (int first = 0; first < operatorsCardList.Length; first++) {
            for (int second = 0; second < numbersCardList.Length; second++) {
                List<CardController> cards = new List<CardController>() { operatorsCardList[first], numbersCardList[second] };
                int _score = gameManeger.CaluculateScore(score, cards);

                if (_score == gameManeger.targetScore) {
                    return cards;
                }

                if (Mathf.Abs(_score - targetScore) < Mathf.Abs(greedyScore - targetScore)) {
                    greedyScore = _score;
                    greedyCards = cards;
                }
            }
        }

        if (gameManeger.enemyAIPlayer.cardPoint >= Const.DOUBLE_OPERATE_THRESHOLD) {
            for (int firstOperator = 0; firstOperator < operatorsCardList.Length; firstOperator++) {
                for (int firstNumber = 0; firstNumber < numbersCardList.Length; firstNumber++) {
                    for (int secondOperator = 0; secondOperator < operatorsCardList.Length; secondOperator++) {
                        for (int secondNumber = 0; secondNumber < numbersCardList.Length; secondNumber++) {
                            if ((firstNumber != secondNumber) && (firstOperator != secondOperator)) {
                                List<CardController> cards = new List<CardController>() { operatorsCardList[firstOperator],
                                    numbersCardList[firstNumber], operatorsCardList[secondOperator], numbersCardList[secondNumber]};

                                int _score = gameManeger.CaluculateScore(score, cards);

                                if (_score == gameManeger.targetScore) {
                                    return cards;
                                }

                                if (Mathf.Abs(_score - targetScore) < Mathf.Abs(greedyScore - targetScore)) {
                                    greedyScore = _score;
                                    greedyCards = cards;
                                }
                            }
                        }
                    }
                }
            }
        }

        return greedyCards;
    }
}
