using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="CardEntity", menuName = "Create CardEntity")]
public class CardEntity : ScriptableObject {

    public new string name;
    public string kind;
    public string value;
}
