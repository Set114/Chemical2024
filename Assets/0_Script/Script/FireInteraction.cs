using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FireInteraction : MonoBehaviour
{
    public GameObject CFXR;
    public Collider fireCollider;         //  火焰碰撞
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
        //  先關閉火焰碰撞
        fireCollider.enabled = false;
        fireParticleSystem.Stop();
    }

    void Update()
    {
        if (conditionMet)
        {
            return;
        }
        //  修改打開火焰的判斷
        if ((targetshowLevelText.text == "1-2" || targetshowLevelText.text == "2-1")
            && !IsApproximatelyEqual(transform.position, originalPosition.position))
        {
            //ani.SetBool("firewow", true);
            StartCoroutine(PlayFireEffectWithDelay());
            //AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);

            //影響流程暫時先拿掉：
            //conditionMet = true;
        }
        else
        {
            //ani.SetBool("firewow", false);
            //print("animate end");
            //  關閉火焰
            fireStop();
        }
    }

    bool IsApproximatelyEqual(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < tolerance;
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
            //  打開火焰碰撞
            fireCollider.enabled = true;
            fireParticleSystem.Play();
        }
    }

    public void fireStop()
    {
        //  關閉火焰碰撞
        fireCollider.enabled = false;
        fireParticleSystem.Stop();
    }
}
