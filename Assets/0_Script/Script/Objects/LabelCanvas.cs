using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelCanvas : MonoBehaviour
{

    public Transform targetObject; // 需要跟隨的目標物件
    public Vector3 offset = new Vector3(0, 2, 0); // Canvas 相對於目標物件的偏移量

    void Update()
    {
        if (targetObject != null)
        {
            // 將 Canvas 的位置設為目標物件的位置加上偏移量
            transform.position = targetObject.position + offset;

            // 保持 Canvas 朝向攝影機
            transform.rotation = Camera.main.transform.rotation;
        }
        if(!targetObject)
        {
            this.gameObject.SetActive(false);
        }
        else if (!targetObject.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }
}
