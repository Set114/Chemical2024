using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  管理提示板、角色
public class HintManager : MonoBehaviour
{
    [Tooltip("提示板")]
    [SerializeField] private GameObject hintPanel;
    [Tooltip("提示訊息")]
    [SerializeField] private GameObject[] hints;

    [Tooltip("角色")]
    [SerializeField] private GameObject character;

    [Tooltip("目前步驟")]
    [SerializeField] private int currStep = 1;

    private GameManager gm;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        hintPanel.SetActive(false);
        character.SetActive(false);
    }

    //  切換步驟
    public float SwitchStep(int step)
    {
        currStep = step;
        string stepName = gm.GetCurrLevel() + 1 + "-" + (currStep + 1);
        bool hasHint = false;

        if (gm == null)
            gm = FindObjectOfType<GameManager>();

        float delay = gm.GetDefaultDelay();

        foreach (GameObject ui in hints)
        {
            if (ui != null)
            {
                if(ui.name == stepName)
                {
                    ui.SetActive(true);
                    hasHint = true;
                }
                else
                {
                    ui.SetActive(false);
                }
            }
        }
        if (!hasHint)
        {
            //  回傳預設等待時間
            return delay;
        }

        string soundName = "Hint_" + stepName;
        // 取得提示音效長度
        float soundLength = gm.GetClipLength(soundName);

        //  判斷是否有語音
        if (soundLength > 0f)
        {
            //  可穿插角色出場效果
            character.SetActive(true);
            //  等角色出現後再出現
            StartCoroutine(DisplayHintPanel(delay));
            gm.PlaySound(soundName, delay);
            //  加上角色出場、語音播放、離場時間
            delay += soundLength + delay;
            StartCoroutine(CharacterLeaveDelay(delay));
        }
        else
        {
            StartCoroutine(DisplayHintPanel(0f));
            character.SetActive(false);
        }
        //  回傳預設等待時間＋語音播放時間
        return delay;
    }

    IEnumerator DisplayHintPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        hintPanel.SetActive(true);
    }

    //  等語音播完角色再消失
    IEnumerator CharacterLeaveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //  可穿插角色離場效果，再調整delay
        character.SetActive(false);
    }

    //  按下確認按鈕
    public void OnConfirmBtnClicked()
    {
        CloseHintPanel();
        gm.LevelStart();
    }
    //  按下關閉按鈕
    public void OnCloseBtnClicked()
    {
        CloseHintPanel();
    }
    //  關閉提示版
    public void CloseHintPanel()
    {
        hintPanel.SetActive(false);
    }
    //  取得目前步驟
    public int GetCurrStep()
    {
        return currStep;
    }
}
