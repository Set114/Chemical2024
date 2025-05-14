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
    public GameObject VRTeachMode, PCTeachMode;
    public DataInDevice DataInDeviceScript;

    //紀錄資料
    string[] inputRow;

    public string[] StartTimes4_L={ "無紀錄＿開始時間", "無紀錄＿開始時間", "無紀錄＿開始時間", "無紀錄＿開始時間", "無紀錄＿開始時間" };
    public string[] EndTimes4_L = { "無紀錄＿結束時間", "無紀錄＿結束時間", "無紀錄＿結束時間", "無紀錄＿結束時間", "無紀錄＿結束時間" };
    public string[] Counts4_L = { "無紀錄＿次數", "無紀錄＿次數", "無紀錄＿次數", "無紀錄＿次數", "無紀錄＿次數" };
    public int ErrorNumber4_1;

    public string[] StartTimes6_L = { "無紀錄＿開始時間", "無紀錄＿開始時間", "無紀錄＿開始時間" };
    public string[] EndTimes6_L = { "無紀錄＿結束時間", "無紀錄＿結束時間", "無紀錄＿結束時間" };
    public string[] Counts6_L = { "無紀錄＿次數", "無紀錄＿次數", "無紀錄＿次數"};
    public int ErrorNumber6_1;

    public string[] TestAns4 = { "無紀錄＿選項", "無紀錄＿選項", "無紀錄＿選項", "無紀錄＿選項", "無紀錄＿選項" };
    public string[] TestScroe4 = { "無紀錄＿分數", "無紀錄＿分數", "無紀錄＿分數", "無紀錄＿分數", "無紀錄＿分數" };
    public string[] TestAns6 = { "無紀錄＿選項", "無紀錄＿選項", "無紀錄＿選項", "無紀錄＿選項", "無紀錄＿選項" };
    public string[] TestScroe6 = { "無紀錄＿分數", "無紀錄＿分數", "無紀錄＿分數", "無紀錄＿分數", "無紀錄＿分數" };
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
                #if UNITY_STANDALONE_WIN
                    VRTeachMode.SetActive(false);
                    PCTeachMode.SetActive(true);
                #endif
                #if UNITY_ANDROID
                  VRTeachMode.SetActive(true);
                   PCTeachMode.SetActive(false);
                #endif
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
            #region 暫存資料  
            //string[] userData = { UserID.text, UserName.text };
            //int[] indices = { 6, 7, 10, 11 };
            //foreach (int index in indices)
            //DataInDeviceScript.SaveData[index].AddRange(userData);           
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

    public void Save4LearnDataExcel()
    {
        var inputRow = new string[]
        {
        UserID.text, UserName.text , StartTimes4_L[0],  EndTimes4_L[0],Counts4_L[0],StartTimes4_L[1],EndTimes4_L[1],Counts4_L[1],
        StartTimes4_L[2],EndTimes4_L[2],Counts4_L[2], StartTimes4_L[3],EndTimes4_L[3],Counts4_L[3], StartTimes4_L[4], EndTimes4_L[4],Counts4_L[4]
        };

        int tabIndex = 6; // 單元四 教學
        var dataManager = FindObjectOfType<DataInDevice>();
        dataManager.SaveData[tabIndex] = new List<string>(inputRow);
        dataManager.AddDataExcel(tabIndex);
    }
    public void Save4TestDataExcel()
    {
        var inputRow = new string[]
        {
        UserID.text, UserName.text , TestAns4[0],TestScroe4[0], TestAns4[1],TestScroe4[1], TestAns4[2],TestScroe4[2], TestAns4[3],TestScroe4[3], TestAns4[4],TestScroe4[4]
        };

        int tabIndex =7; // 單元四 測驗
        var dataManager = FindObjectOfType<DataInDevice>();
        dataManager.SaveData[tabIndex] = new List<string>(inputRow);
        dataManager.AddDataExcel(tabIndex);
    }
    public void Save6LearnDataExcel()
    {
        var inputRow = new string[]
        {
        UserID.text, UserName.text , StartTimes6_L[0],  EndTimes6_L[0],Counts6_L[0],StartTimes6_L[1],EndTimes6_L[1],Counts6_L[1],
        StartTimes6_L[2],EndTimes6_L[2],Counts6_L[2]};

        int tabIndex = 10; // 單元六 教學
        var dataManager = FindObjectOfType<DataInDevice>();
        dataManager.SaveData[tabIndex] = new List<string>(inputRow);
        dataManager.AddDataExcel(tabIndex);
    }
    public void Save6TestDataExcel()
    {
        var inputRow = new string[]
        {
        UserID.text, UserName.text , TestAns6[0],TestScroe6[0], TestAns6[1],TestScroe6[1], TestAns6[2],TestScroe6[2], TestAns6[3],TestScroe6[3], TestAns6[4],TestScroe6[4]
        };

        int tabIndex = 11; // 單元六 測驗
        var dataManager = FindObjectOfType<DataInDevice>();
        dataManager.SaveData[tabIndex] = new List<string>(inputRow);
        dataManager.AddDataExcel(tabIndex);
    }
    /* public void Stage4RecordTeachTime(bool isStart)
     { //教學4結束時間
         DataInDeviceScript.SaveData[6].Add(DateTime.Now.ToString());
         if (!isStart)
             DataInDeviceScript.SaveData[6].Add("0");
     }
     public void Stage4PushToExcel()
     { //教學4
         DataInDeviceScript.AddDataExcel(6);
     }
     public void Stage6RecordTeachTime(bool isStart)
     { //教學6結束時間
         DataInDeviceScript.SaveData[10].Add(DateTime.Now.ToString());
         if (!isStart)
             DataInDeviceScript.SaveData[10].Add("0");
     }
     public void Stage6PushToExcel()
     { //教學6
         DataInDeviceScript.AddDataExcel(10);
     }*/
}
