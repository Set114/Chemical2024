using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage3TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject backBtn;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject confirmBtn;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int page;
    private GameManager gm;
    private AudioManager audioManager;          //音樂管理

    private void OnEnable()
    {
        gm = GetComponent<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        audioManager.PlayVoice("T3_0_0");

        page = 0;
        ui.SetActive(true);
        backBtn.SetActive(false);
        nextBtn.SetActive(true);
        confirmBtn.SetActive(false);
        image.sprite = sprites[page];
    }

    public void OnButtonClicked(int i)
    {
        page += i;
        if (page >= sprites.Length)
        {
            if (gm)
                gm.enabled = true;
            ui.SetActive(false);
        }
        else
        {
            if (page < 0)
            {
                page = 0;
            }
            image.sprite = sprites[page];
        }

        switch (page)
        {
            case 0:
                backBtn.SetActive(false);
                nextBtn.SetActive(true);
                confirmBtn.SetActive(false);
                break;
            case 1:
            case 2:
                backBtn.SetActive(true);
                nextBtn.SetActive(true);
                confirmBtn.SetActive(false);
                break;
            case 3:
                backBtn.SetActive(true);
                nextBtn.SetActive(false);
                confirmBtn.SetActive(true);
                break;
        }

    }
}
