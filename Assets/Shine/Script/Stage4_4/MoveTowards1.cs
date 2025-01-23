using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards1 : MonoBehaviour
{
    public Transform StartTarget;
    public Transform EndTarget;

    public float Speed;
    float t = 0f;
    public bool isTouchMiss;
    public bool isToEndPoint;
    Vector3 StartPos;
    public Vector3 StartPosRectTransform;

    // Start is called before the first frame update
    void Awake()
    {
       // StartPosRectTransform = StartTarget.GetComponent<RectTransform>().anchoredPosition;
        StartPos = StartTarget.position;
    }

    // Update is called once per frame
    void Update()
    {

        t += Time.deltaTime * Speed;
        if (Vector3.Distance(transform.position, EndTarget.position) > 0.01f&&!isToEndPoint)
        {
            transform.position = Vector3.Lerp(StartTarget.position, EndTarget.position, Mathf.PingPong(t, 1f));
        }
        if (Vector3.Distance(transform.position, EndTarget.position) < 0.01f)
        {
            isToEndPoint = true;
            if (isTouchMiss)
            {
                gameObject.SetActive(false);
            }
           
        }
        if(isToEndPoint&&!isTouchMiss)
        {
            transform.position = Vector3.Lerp(EndTarget.position, StartPos, Mathf.PingPong(t, 1f));

        }
        
    }
    public void Reset()
    {
        StartTarget.GetComponent<RectTransform>().anchoredPosition = StartPosRectTransform;
        gameObject.SetActive(true);
        isToEndPoint = false;
    }
}
