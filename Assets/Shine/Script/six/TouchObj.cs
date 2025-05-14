using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObj : MonoBehaviour
{
    public string HitName;
    public GameObject[] CloseObj;
    public GameObject[] OpenObj;

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
        if (hit.GetComponent<Collider>().name == HitName) {
            for (int i = 0; i < CloseObj.Length; i++) {
                CloseObj[i].SetActive(false);
            }
            for (int j = 0; j < OpenObj.Length; j++)
            {
                OpenObj[j].SetActive(true);
            }
        }
    }
}
