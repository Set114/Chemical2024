using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_1_6 : MonoBehaviour
{
    LevelObjManager levelObjManager;
    HintManager hintManager;            //�޲z���ܪO
    MoleculaDisplay moleculaManager;    //�޲z���l�ù�
    AudioManager audioManager;          //���ֺ޲z
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
