using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpeechAudio : MonoBehaviour
{
    public bool isUIAudio = false; // 標記是否為 UI 語音
    public bool unielf = false; // 標記是否為 UI 語音
    public float ELF_1_1_Duration = 5.0f; // ELF_1-1 語音的持續時間
    public string unielfname;

    private int currentLevel = 0; // 當前關卡

    void OnEnable()
    {
        StartCoroutine(PlaySpeech()); // 當物件啟用時，開始執行播放語音的協程
    }

    // 定義一個協程，根據物件名稱播放對應的語音
    IEnumerator PlaySpeech()
    {
        if (isUIAudio)
        {
            // 如果是 UI 語音，播放物件自己的名稱
            yield return new WaitForSeconds(0.3f); // 等待 0.3 秒
            FindObjectOfType<AudioManager>().Play(this.gameObject.name);
            //Debug.Log("PLAY" + this.gameObject.name);
        }
        else
        {
            // 根據當前關卡播放對應的語音
            currentLevel=currentLevel+1;
            string speechName = "ELF_1-" + currentLevel;
            if (speechName == "ELF_1-6")
            {
                FindObjectOfType<AudioManager>().Play(speechName);
            }
            else
            {          
                if(unielf)
                {
                    yield return new WaitForSeconds(0.3f); // 等待 0.3 秒
                    FindObjectOfType<AudioManager>().Play(unielfname);
                    //Debug.Log("PLAY" + this.gameObject.name);
                }else
                {
                yield return new WaitForSeconds(1f); // 等待 0.3 秒
                FindObjectOfType<AudioManager>().Play(speechName);
                // yield return new WaitForSeconds(1.2f); 
                // yield return new WaitForSeconds(ELF_1_1_Duration); // 等待第一段語音結束
                // FindObjectOfType<AudioManager>().Play("ELF_MEBase"); 
                }
                             
            }


        }

    }

    // 設置當前關卡的方法，用於外部調用
    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }
    
}
