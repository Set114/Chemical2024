using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomDisplay : MonoBehaviour
{
    [Tooltip("顯示螢幕")][SerializeField] GameObject screen;
    [Tooltip("分子")][SerializeField] GameObject[] zoomObjs;
    public Animator zoomAnimator;

    void Awake()
    {
        screen.SetActive(false);
    }

    //  切換顯示分子
    public void ShowZoomObj(int index)
    {
        screen.SetActive(false);

        foreach (GameObject obj in zoomObjs)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        if (index < zoomObjs.Length)
        {
            GameObject obj = zoomObjs[index];
            if (obj != null)
            {
                screen.SetActive(true);
                zoomObjs[index].SetActive(true);
                zoomAnimator = zoomObjs[index].GetComponentInChildren<Animator>();
            }
        }
    }

    public void PlayAnimation()
    {
        if (zoomAnimator == null)
            return;
        zoomAnimator.SetTrigger("isClick");
        zoomAnimator.speed = 1f;
    }

    public void StopAnimation()
    {
        if (zoomAnimator == null)
            return;
        zoomAnimator.speed = 0f;
    }

    public void SetAnimationSpeed(float speed)
    {
        if (zoomAnimator == null)
            return;
        zoomAnimator.speed = speed;
    }

    public void CloseDisplay()
    {
        screen.SetActive(false);
    }

}
