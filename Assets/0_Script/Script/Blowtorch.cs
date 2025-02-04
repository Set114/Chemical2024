using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowtorch : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private Collider fireCollider;
    [SerializeField] private GameObject tutorialObject;

    // Start is called before the first frame update
    void Start()
    {
        fireCollider.enabled = false;
        fireParticleSystem.Stop();
    }

    public void Fire(bool open)
    {
        if (open)
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

    void BackToInitial()
    {

    }
}
