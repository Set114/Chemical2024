using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElzManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button elz_close;
    [SerializeField] Button elz_inform_btn;

    [Header("UI")]
    [SerializeField] GameObject ElzUI;

    // Start is called before the first frame update
    void Start()
    {
        elz_close.onClick.AddListener(Elz_close);
        elz_inform_btn.onClick.AddListener(Elz_open);

        elz_inform_btn.gameObject.SetActive(false);
    }

    void Elz_close()
    {
        ElzUI.SetActive(false);
        elz_inform_btn.gameObject.SetActive(true);
    }

    void Elz_open()
    {
        ElzUI.SetActive(true);
        elz_inform_btn.gameObject.SetActive(false);
    }
}
