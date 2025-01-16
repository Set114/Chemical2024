using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TextMapping
{
    public string chapter;
    [TextArea(3, 5)] public string content;
}
