using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shine_GM : MonoBehaviour
{
    public GameObject TestMode, TeachMode;
    public GameObject TestModeButton, TeachModeButton;
    public GameObject ELF;
    public GameObject[] LevelScene, LevelSceneUI;
    // Start is called before the first frame update
    void Start()
    {
        //MenuUIManager.SharedChapterModeData = 1;
        switch (MenuUIManager.SharedChapterModeData) {
            case 0:
                TeachMode.SetActive(true);
                TestModeButton.SetActive(true);
                ELF.SetActive(true);
                break;
            case 1:
                TestMode.SetActive(true);
                TeachModeButton.SetActive(true);
                ELF.SetActive(false);

                break;
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
}
