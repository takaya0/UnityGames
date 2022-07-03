using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectedDropPlace : MonoBehaviour, IDropHandler
{
   

    public void OnDrop(PointerEventData eventData) {
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        // プレイヤーの現在のコストポイント
        int currentCostPoint = GameObject.Find("GameManeger").GetComponent<GameManeger>().playerCostPoint;
        int currentHandCardNum = GetCardNumInHand(this.transform);

        int placeableCardNuminHand = GetPlaceableCardNuminHand(currentCostPoint);


        bool isPlaceable = currentHandCardNum < placeableCardNuminHand;

        if (card != null && isPlaceable) {
            card.defaultParent = this.transform;
        }
    }

    private int GetCardNumInHand(Transform transform) {
        return transform.GetComponentsInChildren<CardController>().Length;
    }

    private int GetPlaceableCardNuminHand(int currentCostPoint) {
        // 手札の最大枚数を計算
        /*
         * コストポイントが4以下 : 2枚
         * コストポイントが5以上 : 4枚
         * 
         */
        if (currentCostPoint <= 4) {
            return 2; ;
        }
        else {
            return 4;
        }
    }
}
