using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AnimatorStartStage1 : MonoBehaviour
{
    public GameObject item; // 物體
    public GameObject item2; // 物體
    

    public LevelEndSequence levelEndSequence;
    public FeAniAnimationController feAniAnimationController;

    void Start()
    {
        // 延遲2秒後觸發動畫播放
        Invoke("PlayIgniteAnimation", 2.0f);
    }

    public void PlayIgniteAnimation()
    {
        item.SetActive(true);
        StartCoroutine(WaitForFristAnimationDelay());
    }

    IEnumerator WaitForFristAnimationDelay()
    {
        yield return new WaitForSeconds(12f);
        item.SetActive(false);
        item2.SetActive(true);
        feAniAnimationController.ResumeAnimation();

        StartCoroutine(End());
    }
    
    IEnumerator End()
    {
        yield return new WaitForSeconds(5f);
        
        levelEndSequence.EndLevel(false,true, 1f, 11f, 1f, "1", () => { });
    }
    
}
