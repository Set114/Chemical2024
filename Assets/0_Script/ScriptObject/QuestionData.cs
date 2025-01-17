using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData", menuName = "Custom/Question Data", order = 1)]
public class QuestionData : ScriptableObject
{
    public QuestionMapping[] questionContent;
}
