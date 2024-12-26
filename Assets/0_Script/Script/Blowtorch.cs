using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowtorch : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private Collider fireCollider;
    [SerializeField] private GameObject tutorialObject;

    private bool isBurning = false;
    // Start is called before the first frame update
    void Start()
    {
        fireCollider.enabled = false;
        fireParticleSystem.Stop();
    }

    public void Fire(bool open)
    {
        isBurning = open;
        if (isBurning)
        {
            fireParticleSystem.Play();
            tutorialObject.SendMessage("OnBlowtorchGrabbed"); 
        }
        else
        {
            fireParticleSystem.Stop();
        }
        fireCollider.enabled = isBurning;
    }

    private void OnTriggerStay(Collider other)
    {
        // 碰到铁块时触发
        if (isBurning && other.gameObject.name == "IronType1")
            tutorialObject.SendMessage("StartHeating",true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "IronType1")
            tutorialObject.SendMessage("StartHeating", false);
    }
    void BackToInitial()
    {

    }
}
