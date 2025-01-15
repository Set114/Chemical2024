using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControlSound : MonoBehaviour
{
    public AudioMixer AudioMixerObj;
    public Slider BGMSlider;
    public Slider SFXSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBGM() {
        AudioMixerObj.SetFloat("BGM", BGMSlider.value);
    }
    public void SetSFX()
    {
        AudioMixerObj.SetFloat("SFX", SFXSlider.value);
    }
}
