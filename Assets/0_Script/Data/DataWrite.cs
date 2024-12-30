using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataWrite : MonoBehaviour
{
    //public LevelEndSequence levelEndSequence;
    private LevelObjManager levelObjManager;

    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
    }
    public void DataWriting()
    {
        //levelEndSequence.EndLevel(true,false, 1f, 6f, 1f, "1", () => { });
        levelObjManager.LevelClear("1","");
    }
}
