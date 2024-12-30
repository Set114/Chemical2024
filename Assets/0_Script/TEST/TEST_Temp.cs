using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TEST_Temp : MonoBehaviour
{
    [Header("1-2Test")]
    [SerializeField] Button TESTButton; // 顯示級別的文字
    [SerializeField] Animator ani;

    public AnimationTrue animationTrue;

    // Start is called before the first frame update
    void Start()
    {
        TESTButton.onClick.AddListener(aniTEST);
    }

    void aniTEST()
    {
        animationTrue.OnButtonClick();
    }
}
