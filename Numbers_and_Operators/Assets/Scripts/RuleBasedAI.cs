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
        //gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        CardController[] operatorsCardList = operatorsHandTransform.GetComponentsInChildren<CardController>();
        CardController[] numbersCardList = numbersHandTransform.GetComponentsInChildren<CardController>();

        // �J�[�h��I��
        CardController operatorCard = operatorsCardList[Random.Range(0, operatorsCardList.Length)];
        CardController numberCard = numbersCardList[Random.Range(0, numbersCardList.Length)];

        // �J�[�h��z�u����
        yield return new WaitForSeconds(0.5f);
        operatorCard.movement.SetCardTransform(gameManeger.EnemySelectedCardsTransform);
        operatorCard.view.SetDisActiveMask();
        yield return new WaitForSeconds(0.5f);
        numberCard.movement.SetCardTransform(gameManeger.EnemySelectedCardsTransform);
        numberCard.view.SetDisActiveMask();
        yield return new WaitForSeconds(0.7f);

        List<CardController> selectedCards = gameManeger.GetSelectedCards(gameManeger.EnemySelectedCardsTransform);
        score = gameManeger.CaluculateScore(score, selectedCards);
        uiManeger.SetEnemyScoreText(score);

        foreach (CardController card in selectedCards) card.Vanish();
    }
}
