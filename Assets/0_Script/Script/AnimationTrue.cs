using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationTrue : MonoBehaviour
{
    public Animator a1;
    public Animator a2;
    public GameObject fire;
    public GameObject bubble;

    void Start()
    {
        OnButtonClick();
        // print("AnimationTrue_start is on");
    }
    public void OnButtonClick()
    {
        a1.SetBool("cover", true);
        StartCoroutine(SetAnimatorParameterAfterDelay());
    }
    private IEnumerator SetAnimatorParameterAfterDelay()
    {
        yield return new WaitForSeconds(1.2f);
        a2.SetBool("matchelev1", true);
        yield return new WaitForSeconds(1.0f);
        fire.SetActive(true);
        bubble.SetActive(true);
    }
}
