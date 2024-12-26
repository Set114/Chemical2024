using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_1_6 : MonoBehaviour
{
    [SerializeField] private Animator paper;
    [Tooltip("反應時間")]
    [SerializeField] private float reactionTime = 6f;

    private float timer = 0.0f;
    private bool reaction = false;
    private bool reactionDone = false;

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
        if (reaction && !reactionDone)
        {
            timer += Time.deltaTime;

            if (timer > reactionTime)
            {
                reactionDone = true;
                EndTheTutorial();
            }
        }
    }
    public void PaperTouched()
    {
        paper.SetBool("move", true);
        reaction = true;
    }

    public void EndTheTutorial()
    {
        hintManager.SwitchStep("T1_6_2");
        hintManager.showNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(4);
    }

}
