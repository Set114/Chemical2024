using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public Transform StartTarget;
    public Transform EndTarget;

    public float Speed;
    float t = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * Speed;

            transform.position = Vector3.Lerp(StartTarget.position, EndTarget.position,Mathf.PingPong(t,1f));
    }
}
