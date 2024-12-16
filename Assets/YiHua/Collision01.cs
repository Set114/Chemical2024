using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Collision01 : MonoBehaviour
{

    private bool istube01Entered = false;  // 記錄01試管
    private bool istube02Entered = false;  // 記錄02試管

    [Header("Tag")]
    public string tag_test_tube01 = "tag_test_tube01"; //test_tube01 有放粉末那隻試管
    public string tag_test_tube02 = "tag_test_tube02"; //test_tube02 燒燈上那隻


    [Header("GameObject")]
    public GameObject test_tube01; //test_tube01 有放粉末那隻試管
    public GameObject test_tube02; //test_tube02 燒燈上那隻

    public GameObject hints01;//提示放在燒燈上
    public GameObject hints02;//提示請在酒精燈上
    public GameObject hints03;//提示放回磅秤上

    public Animator a1; //cover 酒精燈動畫
    public GameObject fire; //火特效
    public Animator a2; //粉末黑變紅
    public GameObject One_hundred_grams;//100克UI
    public GameObject stepUI02;//步驟UI
    public GameObject stepUI03;//步驟UI
    public GameObject stepUI04;//步驟UI

    [Header("Location")]
    public GameObject Transform01; //空的碰撞框偵測 在架子上
    public GameObject Transform02; //空的碰撞框偵測 在磅秤上


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag_test_tube01)) //如果範圍碰撞到 粉末試管
        {
            Debug.Log("碰到燒燈上");
            test_tube01.SetActive(false); //粉末試管 物件關閉
            test_tube02.SetActive(true); //開啟 架上試管
            stepUI02.SetActive(false); //步驟二 關閉
            stepUI03.SetActive(true); //步驟三 開啟
            One_hundred_grams.SetActive(false); //100克UI關閉
            a1.SetBool("cover", true);
            fire.SetActive(true);
            hints01.SetActive(true);//提示UI開啟 放置燒燈上

            istube01Entered = true;//紀錄01狀態
        }

        if (other.CompareTag(tag_test_tube02) && istube01Entered)
        {
            hints01.SetActive(false);//提示UI關閉 放置燒燈上
            hints02.SetActive(true);//請拿起架上試管來回在火焰中加熱
            a2.SetBool("Liquid", true);
            Debug.Log("水變色");
            Debug.Log("等待10秒");
            StartCoroutine(ShowAndHideUI()); //等待x秒

            istube02Entered = true;//紀錄02狀態
        }
    }

    private IEnumerator ShowAndHideUI()
    {

        yield return new WaitForSeconds(10f); // 等待 x秒

        hints02.SetActive(false);
        stepUI03.SetActive(false);//關閉步驟三
        stepUI04.SetActive(true);//開啟步驟四
        hints03.SetActive(true);//提示放回磅秤上
        Transform02.SetActive(true);//開啟 磅秤上 碰撞
        Transform01.SetActive(false);//關閉 架上 碰撞

    }


}