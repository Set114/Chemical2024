using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeAniAnimationController : MonoBehaviour
{
    public Animator animator;
        
    void Start()
    {
        // animator = GetComponent<Animator>();
    }     
    public void OnAnimationMiddle1()
    {
        animator.speed = 0f;
    }
    public void OnAnimationMiddle2()
    {       
        animator.speed = 0f;//動畫暫停
    }
    public void OnAnimationMiddle3()
    {       
        animator.speed = 0f;//動畫暫停
    }
    public void ResumeAnimation()
    {
        animator.speed = 1f;
        animator.SetBool("isClick", true);
        // Debug.Log("ResumeAnimation");
    }

    public void OnAnimationEnd()
    {
        animator.StopPlayback();
        // Debug.Log("OnAnimationEnd");
    }
}
