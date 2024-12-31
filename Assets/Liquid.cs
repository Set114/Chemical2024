using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField] private string targetName = "Object";
    [SerializeField] private GameObject tutorialObject;


    // 檢測是否碰撞
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == targetName)
        {
            tutorialObject.SendMessage("Reaction");
        }
    }
}
