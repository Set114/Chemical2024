using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Shine_GM : MonoBehaviour
{
    public Text UserID, UserName;
    public GameObject TestMode, TeachMode;
    public GameObject TestModeButton, TeachModeButton;
    public GameObject ELF;
    public GameObject[] LevelScene, LevelSceneUI;
    public GameObject Lesson_ListTeach, Lesson_ListTest;

    public GameObject[] Infos;

    public GameObject VRMode, PCMode;
    public DataInDevice DataInDeviceScript;
    private void Awake()
    {
#if UNITY_STANDALONE_WIN
        VRMode.SetActive(false);
        PCMode.SetActive(true);
#endif
#if UNITY_ANDROID
        VRMode.SetActive(true);
PCMode.SetActive(false);

#endif
    }
    // Start is called before the first frame update
    void Start()
    {
      
        //MenuUIManager.SharedChapterModeData = 1;
        switch (MenuUIManager.SharedChapterModeData) {
            case 0:
                TeachMode.SetActive(true);
                TestModeButton.SetActive(true);
                ELF.SetActive(true);
                Lesson_ListTeach.SetActive(true);
                Lesson_ListTest.SetActive(false);
                break;
            case 1:
                TestMode.SetActive(true);
                TeachModeButton.SetActive(true);
                ELF.SetActive(false);
                Lesson_ListTest.SetActive(true);
                Lesson_ListTeach.SetActive(false);
                break;
        }
       
    }
    private void Update()
    {
        if (UserID.text == "")
        {
            if (GameObject.Find("UserDataManager"))
            {
                UserID.text = FindObjectOfType<UserDataManager>().currentPlayerID;
                UserName.text = FindObjectOfType<UserDataManager>().currentPlayerName;

            }
            else
            {
                UserID.text = "1234";
                UserName.text = "Test";
            }
            #region ¼È¦s¸ê®Æ
            DataInDeviceScript.SaveData4Teach.Add(UserID.text);
            DataInDeviceScript.SaveData4Teach.Add(UserName.text);
            DataInDeviceScript.SaveData4Test.Add(UserID.text);
            DataInDeviceScript.SaveData4Test.Add(UserName.text);
            DataInDeviceScript.SaveData6Teach.Add(UserID.text);
            DataInDeviceScript.SaveData6Teach.Add(UserName.text);
            DataInDeviceScript.SaveData6Test.Add(UserID.text);
            DataInDeviceScript.SaveData6Test.Add(UserName.text);
            #endregion
        }
       
    }
    public void SelectTestMode() {
        MenuUIManager.SharedChapterModeData= 1;
        Application.LoadLevel(Application.loadedLevel);
    }
    public void SelectTeachMode()
    {
        MenuUIManager.SharedChapterModeData = 0;
        Application.LoadLevel(Application.loadedLevel);
    }
    public void BackMenu()
    {
        MenuUIManager.shouldOpenMenu = true;
        Application.LoadLevel("MainMenu");
    }
    public void BackLoginMenu()
    {
        Application.LoadLevel("MainMenu");
    }
    public void ReScene() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void SelectLevel(int ID) {
        for (int i = 0; i < LevelScene.Length; i++) {
            LevelScene[i].SetActive(false);
            LevelSceneUI[i].SetActive(false);
        }
        LevelScene[ID].SetActive(true);
        LevelSceneUI[ID].SetActive(true);
    }
    public void CloseInfos() {
        for (int i = 0; i < Infos.Length; i++) {
            Infos[i].SetActive(false);
        }
    }
    public void Stage4RecordTeachTime() {
        DataInDeviceScript.SaveData4Teach.Add(DateTime.Now.ToString());

    }
    public void Stage4PushToExcel()
    {
        DataInDeviceScript.AddDataTeach(0);

    }
    public void Stage6RecordTeachTime()
    {
        DataInDeviceScript.SaveData6Teach.Add(DateTime.Now.ToString());

    }
    public void Stage6PushToExcel()
    {
        DataInDeviceScript.AddDataTeach(2);

    }
}
