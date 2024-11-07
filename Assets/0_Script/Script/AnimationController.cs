using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    public Button Button;
    public Button Button2;   
    public Button Button3;   
    public Button Button4;   
    public GameObject canva1;
    public GameObject canva2;  
    public GameObject canva3;  
    public GameObject canva4;  
    public GameObject ELF1_3_1;  
    public TMP_Text levelIndex;

    public ELFStatus elfStatus;
    public GlucoseScaleCube glucoseScaleCube1;
    public GlucoseScaleCube glucoseScaleCube2;
    
    public LevelEndSequence levelEndSequence;
    
    public Vector3 scale2 = new Vector3(0, 0.1f, 0);

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        Button.onClick.AddListener(OnButtonClicked);
        Button2.onClick.AddListener(OnButtonClicked2);
        Button3.onClick.AddListener(OnButtonClicked2);
        Button4.onClick.AddListener(OnButtonClicked2);
        Debug.Log("start");
    }                   

    public void OnButtonClicked()
    {
        StartCoroutine(ShowElfAndPlayAnimation());
    }

    private IEnumerator ShowElfAndPlayAnimation()
    {
        ELF1_3_1.SetActive(true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("isClick", true);
        yield return new WaitForSeconds(7f);
        ELF1_3_1.SetActive(false);
        yield return new WaitForSeconds(3f);
        glucoseScaleCube1.OnButtonClicked();
    }    
    public void OnButtonClicked2()
    {
        Debug.Log("OnButtonClicked2 start");
        StartCoroutine(AnimationMiddle2Routine());
        // Button2.gameObject.SetActive(false);
    }

    private IEnumerator AnimationMiddle2Routine()
    {
        ResumeAnimation();
        if (levelIndex.text == "5-3")
        {
            glucoseScaleCube1.NotToClickAgain();
            glucoseScaleCube2.NotToClickAgain();
        }
        if (levelIndex.text == "5-5")
        {
            levelEndSequence.EndLevel(true,true, 1f, 7f, 1f, "1", () => { });
        }
        else
        {
            levelEndSequence.EndLevel(false,true, 1f, 7f, 1f, "1", () => { });
        }
        
        yield return new WaitForSeconds(1f);
        //canva2.SetActive(true);
    }


    public void OnAnimationMiddle1()
    {
        animator.speed = 0f;
        glucoseScaleCube1.OnButtonClicked();
        glucoseScaleCube2.OnButtonClicked();
    }
    public void OnAnimationMiddle2()
    {       
        animator.speed = 0f;//動畫暫停
        Debug.Log("OnAnimationMiddle2");
    }
    public void OnAnimationMiddle3()
    {
        animator.speed = 0f;
        Debug.Log("OnAnimationMiddle3");
    }

    public void OnAnimationEnd()
    {
        animator.StopPlayback();
        Debug.Log("OnAnimationEnd");
    }

    public void ResumeAnimation()
    {
        animator.speed = 1f;
        Debug.Log("ResumeAnimation");
    }
}
