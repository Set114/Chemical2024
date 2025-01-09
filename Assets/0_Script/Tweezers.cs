using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweezers : MonoBehaviour
{
    [Tooltip("夾取位置")]
    [SerializeField] private Transform clampPoint;
    [Tooltip("鑷子模型")]
    [SerializeField] private GameObject[] models;

    private GameObject readyToClampObj; //待夾取的物件
    private GameObject clampingObj;     //夾取中的物件
    private string clampingObjName;     //夾取中的物件原名稱
    private Transform clampingObjParent;//夾取中的物件的元父物件

    private void Start()
    {
        models[0].SetActive(true);
        models[1].SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Clamp(true);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Clamp(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TweezersClamp"))
        {
            readyToClampObj = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (readyToClampObj == other)
        {
            readyToClampObj = null;
        }
    }

    public void Clamp(bool clamp)
    {
        models[0].SetActive(!clamp);
        models[1].SetActive(clamp);
        if (clamp && readyToClampObj&& readyToClampObj.CompareTag("TweezersClamp"))
        {
            clampingObj = readyToClampObj;
            clampingObjName = clampingObj.name;
            clampingObj.name = clampingObjName + "_Clamp";
            clampingObjParent = clampingObj.transform.parent;
            clampingObj.transform.SetParent(clampPoint);
            clampingObj.transform.position = clampPoint.position;
            clampingObj.transform.rotation = clampPoint.rotation;
        }
        else if (clampingObj)
        {
            clampingObj.transform.SetParent(clampingObjParent);
            clampingObj.name = clampingObjName;
            clampingObj = null;
        }
    }
}
