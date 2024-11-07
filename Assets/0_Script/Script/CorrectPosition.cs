using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CorrectPosition : MonoBehaviour
{
    public CTrigger[] Scripts;
    public GameObject wrong;
    public GameObject a1;
    public GameObject submitAnswerUI;
    public Button bnt_t;
    public bool allPlaced = true;
    public bool flag = false;

    public LevelEndSequence levelEndSequence;
    public ControllerHaptics controllerHaptics;

    void Start()
    {
        bnt_t.onClick.AddListener(OnButtonClick_t);
    }

    // 确认所有物体是否都放置在正确位置上
    public void CheckAllObjectsPlaced()
    {
        if (Scripts == null)
        {
            Debug.LogError("Scripts array is not initialized.");
            return;
        }

        int lockedCount = 0; // 記錄 isLocked 為 true 的物件數量
        allPlaced = true;

        foreach (CTrigger script in Scripts)
        {
            if (Vector3.Distance(script.correctPoint.position, script.transform.position) > 0.1f)
            {
                allPlaced = false;
            }
            if(script.isLocked)
            {
                lockedCount++;
                //Debug.Log(2);
                //Debug.Log("lockedCount:"+lockedCount);
            }
            else
            {
                lockedCount--;
                //Debug.Log(3);
                //Debug.Log("lockedCount:" + lockedCount);
            }
        }

        // 檢查 isLocked 為 true 的物件數量是否等於 2
        if (lockedCount == 2)
        {
            submitAnswerUI.SetActive(true);
        }
        else
        {
            submitAnswerUI.SetActive(false);
        }
    }

    // 按钮点击事件处理方法
    public void OnButtonClick_t()
    {
        CheckAllObjectsPlaced(); // 每次按钮点击时检查所有物体的状态

        if (allPlaced)
        {
            foreach (CTrigger script in Scripts)
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
            a1.SetActive(false);

            DelayedLevelChange();
        }
        else
        {
            controllerHaptics.TriggerHapticFeedback(true);
            submitAnswerUI.SetActive(false);
            wrong.SetActive(true);
            a1.SetActive(true);
            flag = true;
            Debug.Log("wrong");
        }
    }

    void DelayedLevelChange()
    {   
        if (flag == true)
        {
            levelEndSequence.EndLevel(false,false, 1f, 0f, 1f, "0", () => { });
        }
        else if (flag == false)
        {
            levelEndSequence.EndLevel(false,false, 1f, 0f, 1f, "1", () => { });
        }
        
    }

}