using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class AtomDetection : MonoBehaviour
{
    [SerializeField] private Area area;
    private enum Area
    {
        A, B, C, D
    }
    [SerializeField] List<Transform> targetTransforms = new List<Transform>(); // 存放目標 Transform

    private Tutorial_3_1 tutorialObject;

    private void Start()
    {
        tutorialObject = FindObjectOfType<Tutorial_3_1>();
    }

    private void OnTriggerStay(Collider other)
    {
        AtomBall atom = other.GetComponent<AtomBall>();
        if (atom)
        {
            if (!atom.isGrabbing)
            {
                if (!atom.isUsing)
                {
                    PlaceObject(atom.transform);
                    atom.isUsing = true;
                    tutorialObject.Reaction(area.ToString(), other.gameObject.name, true);
                    other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        AtomBall atom = other.GetComponent<AtomBall>();
        if (atom && atom.isUsing)
        {
            tutorialObject.Reaction(area.ToString(), atom.gameObject.name, false);
            //atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            atom.isUsing = false;
        }
    }

    /// <summary>
    /// 設定原子位置，若無匹配則隨機放置
    /// </summary>
    public void PlaceObject(Transform obj)
    {
        // 檢查名稱是否匹配，並確認該 Transform 沒有子物件
        foreach (Transform target in targetTransforms)
        {
            if (target.name == obj.name && target.childCount == 0)
            {
                obj.SetParent(target);
                obj.localPosition = Vector3.zero; // 重置位置
                return;
            }
        }
        obj.SetParent(transform);
    }
}