using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI symbolText;

    public void Show(CardModel cardmodel)
    {
        symbolText.text = cardmodel.value;
    }
}
