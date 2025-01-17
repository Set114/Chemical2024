using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Custom/Dialog Data", order = 1)]
public class DialogData : ScriptableObject
{
    public DialogMapping[] dialogContent;
}
