using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OkButtonHandler : MonoBehaviour
{
    public GameObject p1; // 引用p2对象
    public GameObject part0; // 引用part0对象
    public GameObject ball;

    public void OnOkClicked()
    {
        
       
        // 隐藏p2，显示part0
        p1.SetActive(false);
        part0.SetActive(true);
        ball.SetActive(true);

    }
}

