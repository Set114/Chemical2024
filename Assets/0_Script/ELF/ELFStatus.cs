using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELFStatus : MonoBehaviour
{
    public GameObject elfObject; // 拖动精灵对象到此字段

    // 显示精灵对象的方法
    public void ShowELF()
    {
        if (elfObject != null)
        {
            elfObject.SetActive(true);
            //Debug.Log("ELF is now visible.");
        }
        else
        {
            Debug.LogError("elfObject is not assigned.");
        }
    }

    // 隐藏精灵对象的方法
    public void HideELF()
    {
        if (elfObject != null)
        {
            elfObject.SetActive(false);
            //Debug.Log("ELF is now hidden.");
        }
        else
        {
            Debug.LogError("elfObject is not assigned.");
        }
    }
}
