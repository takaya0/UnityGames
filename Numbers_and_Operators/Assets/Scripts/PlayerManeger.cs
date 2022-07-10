using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constraints;
public class PlayerManeger : MonoBehaviour{
    // Start is called before the first frame update

    public Transform operatorsHandTransform;
    public Transform numbersHandTransform;

    public int cardPoint;
    public int score;
    void Start(){
        score = Const.INIT_SCORE;
        cardPoint = Const.INIT_CARD_POINT;
    }

  
}
