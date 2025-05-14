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

    //�������
    string[] inputRow;

    public string[] StartTimes4_L={ "�L�����Ķ}�l�ɶ�", "�L�����Ķ}�l�ɶ�", "�L�����Ķ}�l�ɶ�", "�L�����Ķ}�l�ɶ�", "�L�����Ķ}�l�ɶ�" };
    public string[] EndTimes4_L = { "�L�����ĵ����ɶ�", "�L�����ĵ����ɶ�", "�L�����ĵ����ɶ�", "�L�����ĵ����ɶ�", "�L�����ĵ����ɶ�" };
    public string[] Counts4_L = { "�L�����Ħ���", "�L�����Ħ���", "�L�����Ħ���", "�L�����Ħ���", "�L�����Ħ���" };
    public int ErrorNumber4_1;

    public string[] StartTimes6_L = { "�L�����Ķ}�l�ɶ�", "�L�����Ķ}�l�ɶ�", "�L�����Ķ}�l�ɶ�" };
    public string[] EndTimes6_L = { "�L�����ĵ����ɶ�", "�L�����ĵ����ɶ�", "�L�����ĵ����ɶ�" };
    public string[] Counts6_L = { "�L�����Ħ���", "�L�����Ħ���", "�L�����Ħ���"};
    public int ErrorNumber6_1;

    public string[] TestAns4 = { "�L�����Ŀﶵ", "�L�����Ŀﶵ", "�L�����Ŀﶵ", "�L�����Ŀﶵ", "�L�����Ŀﶵ" };
    public string[] TestScroe4 = { "�L�����Ĥ���", "�L�����Ĥ���", "�L�����Ĥ���", "�L�����Ĥ���", "�L�����Ĥ���" };
    public string[] TestAns6 = { "�L�����Ŀﶵ", "�L�����Ŀﶵ", "�L�����Ŀﶵ", "�L�����Ŀﶵ", "�L�����Ŀﶵ" };
    public string[] TestScroe6 = { "�L�����Ĥ���", "�L�����Ĥ���", "�L�����Ĥ���", "�L�����Ĥ���", "�L�����Ĥ���" };
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
            #region �Ȧs���  
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

        int tabIndex = 6; // �椸�| �о�
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

        int tabIndex =7; // �椸�| ����
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

        int tabIndex = 10; // �椸�� �о�
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

        int tabIndex = 11; // �椸�� ����
        var dataManager = FindObjectOfType<DataInDevice>();
        dataManager.SaveData[tabIndex] = new List<string>(inputRow);
        dataManager.AddDataExcel(tabIndex);
    }
    /* public void Stage4RecordTeachTime(bool isStart)
     { //�о�4�����ɶ�
         DataInDeviceScript.SaveData[6].Add(DateTime.Now.ToString());
         if (!isStart)
             DataInDeviceScript.SaveData[6].Add("0");
     }
     public void Stage4PushToExcel()
     { //�о�4
         DataInDeviceScript.AddDataExcel(6);
     }
     public void Stage6RecordTeachTime(bool isStart)
     { //�о�6�����ɶ�
         DataInDeviceScript.SaveData[10].Add(DateTime.Now.ToString());
         if (!isStart)
             DataInDeviceScript.SaveData[10].Add("0");
     }
     public void Stage6PushToExcel()
     { //�о�6
         DataInDeviceScript.AddDataExcel(10);
     }*/
}
