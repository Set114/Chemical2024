using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElementDisplay : MonoBehaviour
{
    public TMP_Text cText;
    public TMP_Text nText;
    public TMP_Text oText;
    public TMP_Text hText;
    public TMP_Text feText;

    private ElementalBallCounter elementalBallCounter;
    //private DetectBall detectBall; // 引用 DetectBall 脚本

    void Start()
    {
        elementalBallCounter = FindObjectOfType<ElementalBallCounter>();
    }

    public void UpdateDisplay()
    {
            // 显示 ElementalBallCounter 中的值

            cText.text = elementalBallCounter.GetBallCount("c").ToString();
            nText.text = elementalBallCounter.GetBallCount("n").ToString();
            oText.text = elementalBallCounter.GetBallCount("o").ToString();
            hText.text = elementalBallCounter.GetBallCount("h").ToString();
            feText.text = elementalBallCounter.GetBallCount("fe").ToString();
    }
}
