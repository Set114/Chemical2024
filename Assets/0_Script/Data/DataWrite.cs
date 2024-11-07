using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataWrite : MonoBehaviour
{
    public LevelEndSequence levelEndSequence;
    
    public void DataWriting()
    {
        levelEndSequence.EndLevel(true,false, 1f, 6f, 1f, "1", () => { });
        
    }
}
