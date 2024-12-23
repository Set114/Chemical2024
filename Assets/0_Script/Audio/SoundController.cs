using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioSource myAudio;
    bool isStart = false;
    GameObject sender;
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
            {
                if (sender)
                    sender.SendMessage("FinishAudio");
                Destroy(this.gameObject);
            }
        }
    }

    void SetClip(AudioClip Clip)
    {
        myAudio.clip = Clip;
    }

    void SetClip(SoundMessage ClipData)
    {
        sender = ClipData.sender;
        myAudio.clip = ClipData.clip;
    }
}
