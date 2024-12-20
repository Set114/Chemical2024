using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Molecula
{
    public string name;
    public GameObject moleculaObj;
    public float animTime;
}
//  管理分子視窗顯示
public class MoleculaDisplay : MonoBehaviour
{
    [Tooltip("顯示螢幕")]
    [SerializeField] private GameObject screen;
    [Tooltip("分子")]
    [SerializeField] private Molecula[] moleculas;

    private GameObject targerMolecula;
    private GameManager gm;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        screen.SetActive(false);
        foreach (Molecula molecula in moleculas)
        {
            if (molecula.moleculaObj != null)
            {
                molecula.moleculaObj.SetActive(false);
            }
        }
    }

    //  切換顯示分子
    public float SwitchDisplay(int step)
    {
        string moleculaName = gm.GetCurrLevel() + 1 + "-" + (step + 1);
        print("MoleculaDisplay: " + moleculaName);
        float delay = 0f;
        screen.SetActive(false);
        foreach (Molecula molecula in moleculas)
        {
            if (molecula.moleculaObj != null)
            {
                if (molecula.name == moleculaName)
                {
                    screen.SetActive(true);
                    targerMolecula = molecula.moleculaObj;
                    delay = molecula.animTime;
                }
                molecula.moleculaObj.SetActive(false);
            }
        }
        if (targerMolecula != null)
            targerMolecula.SetActive(true);
        return delay;
    }
}
