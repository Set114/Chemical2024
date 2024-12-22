using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioSource myAudio;
    bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        myAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            if (myAudio.isPlaying)
                isStart = true;
        }
        else
        {
            if (!myAudio.isPlaying)
                Destroy(this.gameObject);
        }
    }

    public void SetClip(AudioClip Clip)
    {
        myAudio.clip = Clip;
    }
}
