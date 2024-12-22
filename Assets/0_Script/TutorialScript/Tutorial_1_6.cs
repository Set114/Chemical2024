using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_1_6 : MonoBehaviour
{
    LevelObjManager levelObjManager;
    HintManager hintManager;            //管理提示板
    MoleculaDisplay moleculaManager;    //管理分子螢幕
    AudioManager audioManager;          //音樂管理
    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        audioManager = FindObjectOfType<AudioManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_6_1");

        moleculaManager.ShowMoleculas(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
