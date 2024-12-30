using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    private AudioSource _audio;
    public GameObject QA;
    void Awake()
    {
       
    }
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        PlayAudio(_audio.clip);
    }
    void Update()
    {
       

    }
    public void PlayAudio(AudioClip clip, UnityAction callback = null, bool isLoop = false)
    {
        _audio.clip = clip;
        _audio.loop = isLoop;
        _audio.Play();
        StartCoroutine(AudioPlayFinished(_audio.clip.length, callback));
    }
    private IEnumerator AudioPlayFinished(float time, UnityAction callback)
    {
        yield return new WaitForSeconds(time);
        QA.SetActive(true);
    }
}
