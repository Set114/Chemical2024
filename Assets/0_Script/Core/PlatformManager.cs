using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private GameObject pcController;
    [SerializeField] private GameObject vrController;

    [SerializeField] private GameObject tutorialUI_pc;
    [SerializeField] private GameObject tutorialUI_vr;

    private GameManager gm;
    private AudioManager audioManager;          //音樂管理
    bool isPC = false;

    // Start is called before the first frame update
    void Awake()
    {
        gm = GetComponent<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        // �b�s�边���Ҷ}�Ҫ���
        isPC = true;
#else
        // �b��L���x�O������
        isPC = false;
#endif
        pcController.SetActive(isPC);
        vrController.SetActive(!isPC);

        if (tutorialUI_pc)
        {
            tutorialUI_pc.SetActive(isPC);
        }
        if (tutorialUI_vr)
        {
            tutorialUI_vr.SetActive(!isPC);
        }
    }

    private void Start()
    {
        if (isPC)
        {
            audioManager.PlayVoice("PC_Control");
        }
        else
        {
            audioManager.PlayVoice("VR_Control");
        }
    }

    //按下操作教學確認按鈕
    public void OnConfirmButtonClicked()
    {
        if (gm)
        {
            if (gm.currStage == 3)
            {
                Stage3TutorialUI stage3Tutorial = FindObjectOfType<Stage3TutorialUI>();
                if (stage3Tutorial)
                    stage3Tutorial.enabled = true;
            }
            else
            {
                gm.enabled = true;
            }
        }
        if (tutorialUI_pc)
            tutorialUI_pc.SetActive(false);
        if (tutorialUI_vr)
            tutorialUI_vr.SetActive(false);
    }
}
