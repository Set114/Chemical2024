using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  負責管理關卡物件
public class LevelObjManager : MonoBehaviour
{
    [Tooltip("關卡物品")]
    [SerializeField] private GameObject[] levelObjects; // 關卡物品

    [Tooltip("LoadingSign")]
    [SerializeField] private GameObject loading_sign;
    private GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        loading_sign.SetActive(false);
    }

    //  切換關卡物件
    public void SwitchObject(int index)
    {
        foreach (GameObject obj in levelObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        if (index < levelObjects.Length)
        {
            if (levelObjects[index] != null)
                levelObjects[index].SetActive(true);
        }
    }

    //  關卡完成
    public void LevelClear(string answer,string hintSoundName)
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
