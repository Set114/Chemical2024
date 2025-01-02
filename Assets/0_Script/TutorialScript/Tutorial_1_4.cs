using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial_1_4 : MonoBehaviour
{
    [Tooltip("酒精燈")]
    [SerializeField] private Transform alcoholLamp;
    [Tooltip("試管")]
    [SerializeField] private Transform testTube;
    [Tooltip("酒精燈目標位置")]
    [SerializeField] private Transform alcoholLampPoint;
    [Tooltip("試管目標位置")]
    [SerializeField] private Transform testTubePoint;
    private Vector3 alcoholLampStartPos;
    private Vector3 testTubeStartPos;

    [SerializeField] private float minDistance = 0.05f;
    private bool alcoholLampPlaced = false;
    private bool testTubePlaced = false;

    [SerializeField] private GameObject submitAnswerUI;
    [SerializeField] private GameObject wrongUI;
    [SerializeField] private GameObject image;
    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_4_1");

        submitAnswerUI.SetActive(false);
        wrongUI.SetActive(false);
        image.SetActive(true);

        alcoholLampStartPos = alcoholLamp.position;
        testTubeStartPos = testTube.position;
    }

    // 确认所有物体是否都放置在正确位置上
    public void CheckAllObjectsPlaced()
    {
        alcoholLampPlaced = Vector3.Distance(alcoholLamp.position, alcoholLampPoint.position) <= minDistance;
        testTubePlaced = Vector3.Distance(testTube.position, testTubePoint.position) <= minDistance;
        if (testTubePlaced && alcoholLampPlaced)
        {
            alcoholLamp.position = alcoholLampPoint.position;
            testTube.position = testTubePoint.position;
            submitAnswerUI.SetActive(true);
        }
        else
        {
            submitAnswerUI.SetActive(false);
        }
    }

    // 點擊確認按鈕
    public void OnSubmitButtonClicked()
    {
        CheckAllObjectsPlaced(); // 每次按钮点击时检查所有物体的状态

        if (testTubePlaced && alcoholLampPlaced)
        {
            submitAnswerUI.SetActive(false);
            image.SetActive(false);

            EndTheTutorial();
        }
        else
        {
            submitAnswerUI.SetActive(false);
            wrongUI.SetActive(true);
            image.SetActive(true);
            Debug.Log("wrong");
        }
    }

    public void Reaction(GameObject sender)
    {
        if (sender.name == "Point_AlcoholLamp")
        {
            alcoholLampPlaced = true;
            alcoholLamp.position = alcoholLampPoint.position;

        }
        else if (sender.name == "Point_TestTube")
        {
            testTubePlaced = true;
            testTube.position = testTubePoint.position;
        }

        CheckAllObjectsPlaced();
    }

    public void EndTheTutorial()
    {
        //hintManager.SwitchStep("T1_4_2");
        //hintManager.ShowNextButton(this.gameObject);
        levelObjManager.LevelClear(0);

    }
    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
