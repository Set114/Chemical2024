using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C6H12O6_0624ShowController : MonoBehaviour
{
    public GameObject C6H12O6;   // 用於控制顯示的 GameObject
    public SwitchUI switchUI;    // 用於取得等級的 UI 物件
    private Animator animator;   // 宣告 Animator

    void OnEnable()
    {
        C6H12O6.SetActive(true);  
        animator = C6H12O6.GetComponent<Animator>(); 

        int levelindex = switchUI.GetLevelCount();    
        if (levelindex == 2)
        {
            animator.Play("C6H12O6", 0, 0f);  // 第0幀, 787 是動畫總幀數
        }
        else if (levelindex == 3)
        {
            animator.Play("C6H12O6", 0, 402f / 787f);  // 第402幀, 787 是動畫總幀數
        }
        else if (levelindex == 4)
        {
            animator.Play("C6H12O6", 0, 589f / 787f);  // 第589幀, 787 是動畫總幀數
        }
        animator.speed = 0f;
    }
}
