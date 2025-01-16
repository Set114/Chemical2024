using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TipsData", menuName = "Custom/Tips Data", order = 1)]
public class TipsData : ScriptableObject
{
    public TextMapping[] tipsContent;
}
