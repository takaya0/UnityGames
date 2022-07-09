using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

using Constraints;


public class SelectedDropPlace : MonoBehaviour, IDropHandler{
   
    public void OnDrop(PointerEventData eventData) {
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        
        int currentCostPoint = GameObject.Find("GameManeger").GetComponent<GameManeger>().playerCardPoint;
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

    private int GetPlaceableCardNuminHand(int currentCardPoint) {
        // 置けるカードの枚数を取得
        if (currentCardPoint <= Const.DOUBLE_OPERATE_THRESHOLD) {
            return 2; 
        }else {
            return 4;
        }
    }
}
