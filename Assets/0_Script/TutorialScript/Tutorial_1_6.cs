using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_1_6 : MonoBehaviour
{
    [Tooltip("試紙")]
    [SerializeField] private Animator paper;

    [Header("Mark")]
    public GameObject q1questionMark;
    public GameObject q2questionMark;
    [Header("Button")]
    public Button q1question_btn;
    public Button q2question_btn;

    [Tooltip("反應時間")]
    [SerializeField] private float reactionTime = 6f;

    private float timer = 0.0f;
    private bool reaction = false;
    private bool isEnd = false;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板
    private ZoomDisplay zoomDisplay;            //管理近看視窗

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_6_1");
    }

    private void Update()
    {
        if (reaction && !isEnd)
        {
            timer += Time.deltaTime;

            if (timer > reactionTime)
            {
                isEnd = true;
                EndTheTutorial();
            }
        }
    }

    //  開始反應
    public void Reaction()
    {
        if (!reaction)
        {
            paper.SetBool("move", true);
            reaction = true;
        }
    }

    public void EndTheTutorial()
    {
        hintManager.SwitchStep("T1_6_2");
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(1);
    }
}
