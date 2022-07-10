using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Constraints;


public class SelectedDropPlace : MonoBehaviour, IDropHandler{

    PlayerManeger player;
    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerManeger>();
    }
   
    public void OnDrop(PointerEventData eventData) {
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        
        int currentCardPoint = player.cardPoint;
       
        int currentHandCardNum = GetCardNumInHand(this.transform);

        int placeableCardNuminHand = GetPlaceableCardNuminHand(currentCardPoint);
        
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
        if (currentCardPoint < Const.DOUBLE_OPERATE_THRESHOLD) {
            return 2; 
        }else {
            return 4;
        }
    }
}
