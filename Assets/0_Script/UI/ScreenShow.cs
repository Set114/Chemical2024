using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShow : MonoBehaviour
{
    public GameObject screen; 

    void Start()
    {
        // 初始設定，確保screen是隱藏的
        //screen.SetActive(false);
    }

    // 顯示screen的方法
    public void ShowScreen(float delayTime)
    {
        StartCoroutine(ShowScreenActive(delayTime));
        // Debug.Log("show");
    }

    // 隱藏screen的方法
    public void HideScreen()
    {
        screen.SetActive(false);
        // Debug.Log("hide");
    }
    
    IEnumerator ShowScreenActive(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        screen.SetActive(true);
    }

}
