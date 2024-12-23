using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  負責管理關卡物件
public class LevelObjManager : MonoBehaviour
{
    [Tooltip("關卡物品")]
    [SerializeField] GameObject[] levelObjects; // 關卡物品

    [Tooltip("LoadingSign")]
    [SerializeField] GameObject loading_sign;

    //---system
    GameManager gm;
    QuestionManager questionManager; //管理題目介面

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        loading_sign.SetActive(false);

        foreach (GameObject obj in levelObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void SetLevel()    //給GameManager 使用的
    {
        //應該要依照currentLevel 去切換對應的
        questionManager.ShowQuestion(gm.currLevel);
    }


    public void LevelStart()
    {
        if (gm.currLevel < levelObjects.Length)
        {
            if (levelObjects[gm.currLevel] != null)
                levelObjects[gm.currLevel].SetActive(true);
        }
    }

    //關卡完成 NextStageType 0:下一關 1:教學結束 2:測驗結束
    public void LevelClear( int NextStageType )
    {
        //  播放最後的提示語音
        levelObjects[gm.currLevel].SetActive(false);

        switch (NextStageType)
        {
            case 0: //下一關
                loading_sign.SetActive(true);
                gm.LevelClear("我不知道這個Answer是啥");
                StartCoroutine(ShowFinishDialog());
                break;
            case 1: //教學結束
                questionManager.ShowFinishLearnUI();
                break;
            case 2: //測驗結束
                questionManager.ShowFinishExamUI();
                break;
        }
    }

    IEnumerator ShowFinishDialog()
    {
        yield return new WaitForSeconds(1.0f);
        loading_sign.SetActive(false);            
        //??????  gm.LevelClear(answer); 
        this.SetLevel();
    }

    public void LevelClear(string answer, string hintSoundName)
    {
        //  播放最後的提示語音
        loading_sign.SetActive(true);

        StartCoroutine(ShowHintDelay(answer, gm.NextStep()));
    }
    IEnumerator ShowHintDelay(string answer, float delay)
    {
        yield return new WaitForSeconds(delay);
        loading_sign.SetActive(false);
        gm.LevelClear(answer);
    }
}
