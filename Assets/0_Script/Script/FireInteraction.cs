using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FireInteraction : MonoBehaviour
{
    public GameObject CFXR; 
    public Transform originalPosition; 
    public Animator ani;
    private ParticleSystem fireParticleSystem; 
    private bool isAnimating = false; 
    [SerializeField] TMP_Text targetshowLevelText;

    
    private const float tolerance = 0.01f;

    
    private bool conditionMet = false;

    void Start()
    {
        
        fireParticleSystem = CFXR.GetComponent<ParticleSystem>();
        if (fireParticleSystem == null)
        {
            Debug.LogError("Particle system component not found on the fire effect!");
            return;
        }

       
        fireParticleSystem.Stop();
    }

    void Update()
    {
        
        if (conditionMet)
        {
            return;
        }

        if (targetshowLevelText.text == "1-2" && !IsApproximatelyEqual(transform.position, originalPosition.position))
        {
            //ani.SetBool("firewow", true);
            StartCoroutine(PlayFireEffectWithDelay());
            //AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
            conditionMet = true;
        }
        else
        {
            //ani.SetBool("firewow", false);
            //print("animate end");
        }
    }

    bool IsApproximatelyEqual(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < tolerance * tolerance;
    }

    public void OnAnimationEvent()
    {
        Debug.Log("Animation event triggered");
    }

    private IEnumerator PlayFireEffectWithDelay()
    {
        yield return new WaitForSeconds(1.8f);

        if (!fireParticleSystem.isPlaying)
        {
            fireParticleSystem.Play();
        }
    }

    public void fireStop()
    {
        fireParticleSystem.Stop();
    }
}
