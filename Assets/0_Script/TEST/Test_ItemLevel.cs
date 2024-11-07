using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test_ItemLevel : MonoBehaviour
{
    public Button LearnEnd_btn;
    public Button TestEnd_btn;

    public bool EndBool;
    public string AnswerInt;

    public LevelEndSequence levelEndSequence;

    void Start()
    {
        EndBool = false;
        AnswerInt = "1";
        LearnEnd_btn.onClick.AddListener(End);
        TestEnd_btn.onClick.AddListener(End);
    }

    void End()
    {
        // levelEndSequence.EndLevel(EndBool, false, 2f, 0f, 5f, 0f, AnswerInt);
        levelEndSequence.EndLevel(EndBool,false, 1f, 4f, 1f, AnswerInt, () => { });
    }
}
