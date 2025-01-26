using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelCanvas : MonoBehaviour
{

    public Transform targetObject; // �ݭn���H���ؼЪ���
    public Vector3 offset = new Vector3(0, 2, 0); // Canvas �۹��ؼЪ��󪺰����q

    void Update()
    {
        if (targetObject != null)
        {
            // �N Canvas ����m�]���ؼЪ��󪺦�m�[�W�����q
            transform.position = targetObject.position + offset;

            // �O�� Canvas �¦V��v��
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
