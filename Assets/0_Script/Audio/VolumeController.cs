// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class VolumeController : MonoBehaviour
// {
//     public Slider SoundSlider;
//     public Slider BGMSlider;

//     private AudioManager audioManager;

//     void Start()
//     {
//         audioManager = FindObjectOfType<AudioManager>();

//         // 初始化滑動條的值
//         SoundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
//         BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);

//         // 設置滑動條的監聽器
//         SoundSlider.onValueChanged.AddListener(SetSoundVolume);
//         BGMSlider.onValueChanged.AddListener(SetBGMVolume);

//         // 初始化音量
//         SetSoundVolume(SoundSlider.value);
//         SetBGMVolume(BGMSlider.value);
//     }

//     void SetSoundVolume(float volume)
//     {
//         foreach (Sound s in audioManager.Sounds)
//         {
//             if (s.source != null && s.name != "BackgroundMusic")
//             {
//                 s.source.volume = volume;
//             }
//         }
//         PlayerPrefs.SetFloat("SoundVolume", volume);
//     }

//     void SetBGMVolume(float volume)
//     {
//         if (audioManager.bgmSource != null)
//         {
//             audioManager.bgmSource.volume = volume;
//         }
//         PlayerPrefs.SetFloat("BGMVolume", volume);
//     }
// }
