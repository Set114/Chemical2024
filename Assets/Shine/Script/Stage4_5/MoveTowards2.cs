using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards2 : MonoBehaviour
{
    public Transform StartTarget;
    public Transform EndTarget;

    public float Speed;
    //float t = 0f;
    public bool isTouchMiss;
    public bool isToEndPoint;
    public Vector3 StartPos;

    public GameObject CO2;
    public int ID;
    public Stage4_5 Stage4_5Obj;
    bool isChangeScale;
    public float ChangeScaleValue;
    public bool isH2;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = StartTarget.position;
    }

    // Update is called once per frame
    void Update()
    {

        //t += Time.deltaTime * Speed;
        
            if (Vector3.Distance(transform.position, EndTarget.position) > 0.01f && !isToEndPoint)
            {
                transform.position = Vector3.MoveTowards(StartTarget.position, EndTarget.position, Speed);
            }
            if (Vector3.Distance(transform.position, EndTarget.position) < 0.01f)
            {
                isToEndPoint = true;
                if (isTouchMiss&& !isChangeScale)
                {
                    gameObject.SetActive(false);
                    CO2.SetActive(true);
                    Stage4_5Obj.MarbleUIs[ID].transform.GetChild(0).localScale -= new Vector3(ChangeScaleValue, ChangeScaleValue, ChangeScaleValue);
                isChangeScale = true;
                }

            }
        
        
    }
    public void Reset()
    {
        StartTarget.position = StartPos;
        if (isH2)
        {
            gameObject.SetActive(true);
            Stage4_5Obj.MarbleUIs[0].transform.GetChild(0).localScale = Vector3.one;
            Stage4_5Obj.MarbleUIs[1].transform.GetChild(0).localScale = Vector3.one;
            Stage4_5Obj.MarbleUIs[2].transform.GetChild(0).localScale = Vector3.one;
        }
        else {
            gameObject.SetActive(false);

        }
        isToEndPoint = false;
        isChangeScale = false;


    }
}
