using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeHiObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "Manganese") {
            FindObjectOfType<Stage4_3>().ObjTouchTube(0);
        }
        if (hit.GetComponent<Collider>().name == "Potato")
        {
            FindObjectOfType<Stage4_3>().ObjTouchTube(1);
        }
        if (hit.GetComponent<Collider>().name == "Pork")
        {
            FindObjectOfType<Stage4_3>().ObjTouchTube(2);
        }
        if (hit.GetComponent<Collider>().name == "Marbles")
        {
            FindObjectOfType<Stage4_3>().ObjTouchTube(3);
        }
        if (hit.GetComponent<Collider>().name == "Plastic")
        {
            FindObjectOfType<Stage4_3>().ObjTouchTube(4);
        }
    }
}
