using UnityEngine;
using System.Collections;

public class AnimationEventMediator : MonoBehaviour
{
    //public LevelEndSequence levelEndSequence;
    private LevelObjManager levelObjManager;

    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
    }
    public void OnAnimationEnd()
    {
        //levelEndSequence.EndLevel(true,true, 2f, 20f, 1f, "1", () => { });
        levelObjManager.LevelClear("1","");
    }
}
