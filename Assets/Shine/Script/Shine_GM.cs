using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shine_GM : MonoBehaviour
{
    public GameObject TestMode, TeachMode;
    public GameObject TestModeButton, TeachModeButton;

    // Start is called before the first frame update
    void Start()
    {
        switch (MenuUIManager.SharedChapterModeData) {
            case 0:
                TeachMode.SetActive(true);
                TestModeButton.SetActive(true);
                break;
            case 1:
                TestMode.SetActive(true);
                TeachModeButton.SetActive(true);
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
}
