using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimationEnable : MonoBehaviour
{
    public WaterScaleCube WaterScaleCubeScript;
    public Button Button;
    public GameObject h2o;
    public GameObject h2oNo;
    public GameObject canva1;
    public GameObject canva2;


    private bool a1 = false;
    public LevelEndSequence levelEndSequence;
    public FeAniAnimationController feAniAnimationController1;
    public FeAniAnimationController feAniAnimationController2;

    void Start()
    {
        // Button.onClick.AddListener(() => OnButtonClicked(h2o, canva1));
        Button.onClick.AddListener(Level1);
    }

    private void Level1()
    {
        feAniAnimationController1.ResumeAnimation();
        End();
    }

    IEnumerator HideScreenActive(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }
    
    private void Update()
    {
        if(WaterScaleCubeScript.isMove &&!a1)
        {
            feAniAnimationController2.ResumeAnimation();
            End();
            // OnButtonClicked(h2oNo , canva2);
            a1 = true;
        }
   }
    
    private void End()
    {
        levelEndSequence.EndLevel(false,true, 1f, 9f, 1f, "1", () => { });
        // StartCoroutine(HideScreenActive(5f));
    }


    // public void OnButtonClicked(GameObject a , GameObject canva)
    // {
    //     // a.SetActive(true);
    //     //canva.SetActive(true);
    // }
}
