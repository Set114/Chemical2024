using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_UI : MonoBehaviour
{
    public AudioSource audioSource;  // 指向要監控的 AudioSource

    public GameObject TV;
    //public GameObject TV_Prefab;
    public GameObject TVnext_level;
    public GameObject TVnext_level_UI;
    public GameObject TVnext_level_UICanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource != null)
        {
            StartCoroutine(WaitForAudioToFinish());
        }
    }

    private IEnumerator WaitForAudioToFinish()
    {
        // 等待音頻播放結束
        while (audioSource.isPlaying)
        {
            yield return null; // 等待下一幀
        }

        // 音頻播放完成，啟用目標物件
        TV.SetActive(false); //關閉電視
        TVnext_level.SetActive(true);  //開啟2-2
        TVnext_level_UI.SetActive(true); //開啟UI
        TVnext_level_UICanvas.SetActive(true);  //開啟Canvas_Test
    }
}
