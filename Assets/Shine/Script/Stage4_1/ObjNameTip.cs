using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjNameTip : MonoBehaviour
{
     float Timer;
    public float SetTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > SetTimer) {
            Timer = -0.5f;
            gameObject.SetActive(false);

        }
    }
}
