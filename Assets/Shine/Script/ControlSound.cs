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
    public AudioSource bgmSource;    // Start is called before the first frame update
    void Start()
    {
        SetBGM_SFX();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetBGM_SFX()
    {
        int BGMresult = Mathf.RoundToInt(Mathf.Lerp(-40f, 0f, AudioManager.bgmSliderValue));
        BGMSlider.value = BGMresult;

        int result = Mathf.RoundToInt(Mathf.Lerp(-40f, 0f, AudioManager.soundsSliderValue));
        SFXSlider.value = result;

       /* // �T�O clip �s�b�B�X�k
        if (bgmSource != null && bgmSource.clip != null)
        {
            // Clamp �ɶ��קK���~
            float clampedTime = Mathf.Clamp(AudioManager.AudioSourceLength, 0f, bgmSource.clip.length);

            // �����A�]�w�ɶ�
            bgmSource.Play();
            bgmSource.time = clampedTime;
        }
        else
        {
            Debug.LogWarning("BGM ������ clip �|�����w�I");
        }*/
    }

    public void SetBGM() {
 
        AudioMixerObj.SetFloat("BGM", BGMSlider.value);
        AudioManager.bgmSliderValue=Mathf.InverseLerp(-40f, 0f, BGMSlider.value);
    }
    public void SetSFX()
    {
     
        AudioMixerObj.SetFloat("SFX", SFXSlider.value);
        AudioManager.soundsSliderValue = Mathf.InverseLerp(-40f, 0f, SFXSlider.value);

    }
}
