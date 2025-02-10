using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blowtorch : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private Collider fireCollider;
    [SerializeField] private GameObject tutorialObject;
    private bool isFire = false;
    private bool isGrab = false;

    private bool isPC;

    private void Start()
    {
        //判斷平台
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        isPC = true;
#else
        isPC = false;
#endif

        fireCollider.enabled = false;
        fireParticleSystem.Stop();
    }

    private void Update()
    {
        if (isGrab)
        {
            if (isPC)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Fire(!isFire);
                }
            }
            else if (isGrab)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Fire(!isFire);
                }
            }
        }
    }

    public void Grab(bool grab)
    {
        isGrab = grab;
        if (!isGrab)
        {
            Fire(false);
        }
    }
    public void Fire(bool open)
    {
        isFire = open;
        if (isFire)
        {
            fireParticleSystem.Play();
            tutorialObject.SendMessage("OnBlowtorchGrabbed", SendMessageOptions.DontRequireReceiver); 
        }
        else
        {
            fireParticleSystem.Stop();
            tutorialObject.SendMessage("StartHeating", false, SendMessageOptions.DontRequireReceiver);
        }
        fireCollider.enabled = open;
    }

    private void OnTriggerStay(Collider other)
    {
        // 碰到铁块时触发
        if (other.gameObject.name == "IronType1")
        {
            tutorialObject.SendMessage("StartHeating", true, SendMessageOptions.DontRequireReceiver);
        }
        else if (other.gameObject.name == "WoodPowders" || other.gameObject.name == "SteelWool")
        {
            tutorialObject.SendMessage("ReactionStay", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "IronType1")
            tutorialObject.SendMessage("StartHeating", false, SendMessageOptions.DontRequireReceiver);
    }
}
