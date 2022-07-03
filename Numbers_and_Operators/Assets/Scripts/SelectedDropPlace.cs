using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectedDropPlace : MonoBehaviour, IDropHandler
{
   

    public void OnDrop(PointerEventData eventData) {
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        // �v���C���[�̌��݂̃R�X�g�|�C���g
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
        // ��D�̍ő喇�����v�Z
        /*
         * �R�X�g�|�C���g��4�ȉ� : 2��
         * �R�X�g�|�C���g��5�ȏ� : 4��
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
