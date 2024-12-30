using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Molecula
{
    public string name;
    public GameObject moleculaObj;
    public float animTime;
}
//  管理分子視窗顯示
public class MoleculaDisplay : MonoBehaviour
{
    [Tooltip("顯示螢幕")] [SerializeField] GameObject screen;
    [Tooltip("分子")] [SerializeField] Molecula[] moleculas;
    public Animator moleculasAnimator;

    void Awake()
    {
        screen.SetActive(false);
    }

    //  切換顯示分子
    public void ShowMoleculas(int index)
    {
        screen.SetActive(false);
        foreach (Molecula molecula in moleculas)
        {
            if (molecula.moleculaObj != null)
            {
                molecula.moleculaObj.SetActive(false);
            }
        }
        if (index< moleculas.Length)
        {
            GameObject moleculaObj = moleculas[index].moleculaObj;
            if (moleculaObj != null)
            {
                screen.SetActive(true);
                moleculas[index].moleculaObj.SetActive(true);
                moleculasAnimator = moleculas[index].moleculaObj.GetComponentInChildren<Animator>();
            }
        }
    }

    public void PlayMoleculasAnimation()
    {
        if (moleculasAnimator == null)
            return;
        moleculasAnimator.SetTrigger("isClick");
        moleculasAnimator.speed = 1f;
    }
    public void StopMoleculasAnimation()
    {
        if (moleculasAnimator == null)
            return;
        moleculasAnimator.speed = 0f;
    }

    public void SetMoleculasAnimationSpeed(float speed)
    {
        if (moleculasAnimator == null)
            return;
        moleculasAnimator.speed = speed;
    }

    public void CloseDisplay()
    {
        screen.SetActive(false);
    }

}
