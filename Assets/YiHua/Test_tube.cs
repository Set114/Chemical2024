using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;


public class Test_tube : MonoBehaviour
{

    [Header("Tag")]
    public string TonerTag = "Toner"; //碳粉Tag
    public string CopTag = "Cop"; //氧化銅粉Tag

    [Header("GameObject")]
    public GameObject test_tube01; //test_tube01 有放粉末那隻試管
    // public GameObject test_tube02; //test_tube02 燒燈上那隻
    // public GameObject test_tube03; //test_tube03 磅秤上那隻

    public GameObject Toner; //碳粉
    public GameObject Cop; //氧化銅粉
    public GameObject water;//試管中的水
    public GameObject beakershelf;//架子
                                  // public GameObject warning; //警告UI
    public GameObject Add_tonerUI;//添加碳粉UI
    public GameObject One_hundred_grams;//100克UI
    public GameObject placeUI;
    public GameObject stepUI01;//步驟UI
    public GameObject stepUI02;//步驟UI
                               //  public GameObject stepUI03;//步驟UI
                               //  public GameObject stepUI04;//步驟UI

    [Header("Location")]
    public GameObject Transform01; //空的碰撞框偵測 在架子上
                                   //  public GameObject Transform02; //空的碰撞框偵測 在磅秤上

    //紀錄順序狀態
    private bool isTonerEntered = false;  // 記錄碳粉進入
    private bool isCopEntered = true;  // 記錄氧化銅粉進入

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TonerTag)) //如果碳粉碰到 觸發
        {
            isCopEntered = false; // 標記氧化銅粉進入
            isTonerEntered = true;  //標記碳粉已加入
            water.SetActive(true); //開啟水物件
            Toner.SetActive(false); //關閉碳粉物件
            Debug.Log("已加入碳粉");
        }

        if (other.CompareTag(CopTag) && isTonerEntered) //如果氧化銅在碳粉放完後碰到 觸發
        {

            Cop.SetActive(false); // 關閉氧化銅粉物件
            One_hundred_grams.SetActive(true);
            Debug.Log("已加入氧化銅粉");
            stepUI01.SetActive(false); //第一步驟關閉
            stepUI02.SetActive(true); //第二步驟開啟
            beakershelf.SetActive(true); //開啟架子與燒燈
            Transform01.SetActive(true); //架子上的碰撞框開啟
            placeUI.SetActive(true); //放置提示UI開啟
            if (test_tube01 != null)
            {
                test_tube01.tag = "tag_test_tube01";
            }

        }
        if (other.CompareTag(CopTag) && isCopEntered) //如果氧化銅碰到 觸發
        {
            StartCoroutine(AShowAndHideUI()); // 啟動協程來顯示和隱藏 UI
        }



    }

    private IEnumerator AShowAndHideUI()
    {
        Add_tonerUI.SetActive(true); // 顯示 UI
        yield return new WaitForSeconds(3f); // 等待 3 秒
        Add_tonerUI.SetActive(false); // 隱藏 UI
    }

}