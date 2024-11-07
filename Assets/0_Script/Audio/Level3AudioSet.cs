using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3AudioSet : MonoBehaviour
{
    public float ELF_1_1_Duration = 5.0f; // ELF_1-1 語音的持續時間
    public GameObject ELF; // ELF_1-1 語音的持續時間

    public void leve1Start()
    {
        StartCoroutine(leve1StartAudio()); // 當物件啟用時，開始執行播放語音的協程
    }

    // 定義一個協程，根據物件名稱播放對應的語音
    IEnumerator leve1StartAudio()
    {
        ELF.SetActive(true);
        yield return new WaitForSeconds(1f); 
        FindObjectOfType<AudioManager>().Play("ELF_3-1-1");
        yield return new WaitForSeconds(13f); 
        FindObjectOfType<AudioManager>().Play("ELF_3-1-2");
        yield return new WaitForSeconds(13f); 

        ELF.SetActive(false);
    }
    
    public void leve1Error()
    {
        StartCoroutine(leve1ErrorAudio()); // 當物件啟用時，開始執行播放語音的協程
    }

    IEnumerator leve1ErrorAudio()
    {
        ELF.SetActive(true);
        yield return new WaitForSeconds(1f); 
        FindObjectOfType<AudioManager>().Play("ELF_error_3-1-1");
        yield return new WaitForSeconds(13f); 
        FindObjectOfType<AudioManager>().Play("ELF_error_3-1-2");
        yield return new WaitForSeconds(13f); 

        ELF.SetActive(false);
    }
}
