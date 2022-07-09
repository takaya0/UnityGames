using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleBasedAI : MonoBehaviour
{
   

    private GameManeger gameManeger;
    private SkillManeger skillManeger;

    [SerializeField] UIManeger uiManeger;
    // Start is called before the first frame update
    void Start(){
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        skillManeger = GameObject.Find("SkillManeger").GetComponent<SkillManeger>();
    }

    public IEnumerator EnemyActions() {
        //gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        CardController[] operatorsCardList = gameManeger.EnemyOperatorsHandTransform.GetComponentsInChildren<CardController>();
        CardController[] numbersCardList = gameManeger.EnemyNumbersHandTransform.GetComponentsInChildren<CardController>();

        // カードを選ぶ
        CardController operatorCard = operatorsCardList[Random.Range(0, operatorsCardList.Length)];
        CardController numberCard = numbersCardList[Random.Range(0, numbersCardList.Length)];

        // カードを配置する
        yield return new WaitForSeconds(1);
        operatorCard.movement.SetCardTransform(gameManeger.EnemySelectedCardsTransform);
        yield return new WaitForSeconds(1);
        numberCard.movement.SetCardTransform(gameManeger.EnemySelectedCardsTransform);
        yield return new WaitForSeconds(1);

        List<CardController> selectedCards = gameManeger.GetSelectedCards(gameManeger.EnemySelectedCardsTransform);
        int score = gameManeger.CaluculateScore(gameManeger.enemyScore, selectedCards);
        uiManeger.SetEnemyScoreText(score);

        foreach (CardController card in selectedCards) card.Vanish();
    }
}
