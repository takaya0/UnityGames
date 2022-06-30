using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI symbolText;
    [SerializeField] GameObject selectableFrame;

    public void Show(CardModel cardmodel)
    {
        symbolText.text = cardmodel.value;
    }

    public void SetActiveSelectableFrame() {
        selectableFrame.SetActive(true);
    }

    public void SetDisActiveSelectedFrame() {
        selectableFrame.SetActive(false);
    }

}
