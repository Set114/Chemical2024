using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HammerIronInteraction : MonoBehaviour
{
    [SerializeField] GameObject tutorialObject;
    bool isHitable = true;
    float timer;

    private void OnEnable()
    {
        
    }

    void Update()
    {
        if (!isHitable)
        {
            if (Time.time > timer)
                isHitable = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isHitable) {
            isHitable = false;
            timer = Time.time + 0.3f;
            // 碰到铁块时触发
            if ( other.gameObject.name == "IronCenter")
                tutorialObject.SendMessage("HammerHit");
        }
    }

    void backToInitial()
    {

    }
}
