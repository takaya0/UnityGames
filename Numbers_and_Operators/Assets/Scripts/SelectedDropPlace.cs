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
        int currentHandCardNum = this.transform.GetComponentsInChildren<CardController>().Length;

        bool IsPlaceable = currentHandCardNum < currentCostPoint;

        if (card != null && IsPlaceable) {
            card.defaultParent = this.transform;
        }
    }
}
