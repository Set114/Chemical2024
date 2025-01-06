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
    private XRGrabInteractable testTubeXRGrab;
    [Tooltip("酒精燈目標位置")]
    [SerializeField] private Transform alcoholLampPoint;
    [Tooltip("試管目標位置")]
    [SerializeField] private Transform testTubePoint;

    [SerializeField] private float minDistance = 0.05f;
    private bool alcoholLampGrab = false;
    private bool testTubeGrab = false;

    [SerializeField] private GameObject submitAnswerUI;
    [SerializeField] private GameObject wrongUI;
    [SerializeField] private GameObject image;
    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板

    bool isPC;
    public MouseController pcController;
    int Status = 0;

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

        //判斷平台
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        isPC = true;
        pcController = FindObjectOfType<MouseController>();
#else
        isPC = false;
#endif

        //初始化，一個是改tag，一個開關碰撞
        alcoholLamp.tag = "Pickable";
        alcoholLampPoint.gameObject.SetActive(true);
        testTube.tag = "Untagged";
        testTubePoint.gameObject.SetActive(false);
        testTubeXRGrab= testTube.GetComponent<XRGrabInteractable>();
        testTubeXRGrab.enabled = false;
        Status = 0;
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //等待使用者將物件放上正確位置
                break;
            case 1: 
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    /*
    // 确认所有物体是否都放置在正确位置上 ----取消這做法
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

    // 點擊確認按鈕 ----取消這做法
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
    }*/

    //當碰撞產生時觸發
    public void ReactionStay(GameObject sender)
    {
        if (isPC)
        {
            if (pcController.selectedObject)
                return;
        }
        else if(alcoholLampGrab || testTubeGrab)
        {
            return;
        }

        switch (sender.name)
        {
            case "Point_AlcoholLamp":
                alcoholLamp.tag = "Untagged";
                alcoholLamp.GetComponent<XRGrabInteractable>().enabled = false;
                alcoholLamp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                alcoholLampPoint.gameObject.SetActive(false);
                alcoholLamp.position = alcoholLampPoint.position;
                alcoholLamp.rotation = alcoholLampPoint.rotation;

                testTube.tag = "Pickable";
                testTubeXRGrab.enabled = true;
                testTubePoint.gameObject.SetActive(true);
                hintManager.SwitchStep("T1_4_2");
                break;
            case "Point_TestTube":
                testTube.tag = "Untagged";
                testTubeXRGrab.enabled = false;
                testTube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                testTubePoint.gameObject.SetActive(false);
                testTube.position = testTubePoint.position;
                testTube.rotation = testTube.rotation;
                EndTheTutorial();
                break;
        }
    }

    //  VR環境中抓取、放開時觸發
    public void GrabAlcoholLamp(bool grab)
    {
        alcoholLampGrab = grab;
    }
    public void GrabTestTube(bool grab)
    {
        testTubeGrab = grab;
    }

    public void EndTheTutorial()
    {
        hintManager.SwitchStep("T1_4_3");
        hintManager.ShowNextButton(this.gameObject);
    }
    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
