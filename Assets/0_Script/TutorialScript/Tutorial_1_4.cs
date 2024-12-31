using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_1_4 : MonoBehaviour
{
    [SerializeField] private CTrigger[] targetObjs;
    [SerializeField] private GameObject submitAnswerUI;
    [SerializeField] private GameObject wrongUI;
    [SerializeField] private GameObject image;
    [SerializeField] private int correctCount;
    private bool allPlaced = true;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_4_1");

        submitAnswerUI.SetActive(false);
        wrongUI.SetActive(false);
        image.SetActive(true);
    }

    // 确认所有物体是否都放置在正确位置上
    public void CheckAllObjectsPlaced()
    {
        int lockedCount = 0; // 記錄 isLocked 為 true 的物件數量
        allPlaced = true;

        foreach (CTrigger obj in targetObjs)
        {
            if (Vector3.Distance(obj.correctPoint.position, obj.transform.position) > 0.1f)
            {
                allPlaced = false;
            }
            if (obj.isLocked)
            {
                lockedCount++;
            }
        }

        // 檢查 isLocked 為 true 的物件數量是否等於 2
        if (lockedCount == correctCount)
        {
            submitAnswerUI.SetActive(true);
        }
        else
        {
            submitAnswerUI.SetActive(false);
        }
    }

    // 點擊確認按鈕
    public void OnSubmitButtonClicked()
    {
        CheckAllObjectsPlaced(); // 每次按钮点击时检查所有物体的状态

        if (allPlaced)
        {
            foreach (CTrigger script in targetObjs)
            {
                if (script.otherScript != null)
                {
                    script.otherScript.enabled = false;
                }
                else
                {
                    //Debug.LogWarning("otherScript is null in " + script.name + " script.");
                }
            }
            submitAnswerUI.SetActive(false);
            image.SetActive(false);

            EndTheTutorial();
        }
        else
        {
            submitAnswerUI.SetActive(false);
            wrongUI.SetActive(true);
            image.SetActive(true);
            Debug.Log("wrong");
        }
    }

    public void EndTheTutorial()
    {
        hintManager.SwitchStep("T1_4_2");
        hintManager.ShowNextButton(this.gameObject);

    }
    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
