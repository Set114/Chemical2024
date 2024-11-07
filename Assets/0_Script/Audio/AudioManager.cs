using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AudioManager : MonoBehaviour
{
    // 跟Sound類搭配使用，用於播放音效的程式碼
    // 使用 FindObjectOfType<AudioManager>().Play("音效名稱") 來撥放音效
    // 在Hierarchy中有一個名為 AudioManager 的物件，可以自訂音效名稱
    public Sound[] Sounds; 

    public static AudioManager instance;
    private AudioSource bgmSource; // 獨立的 AudioSource 變數，用於播放背景音樂
    public AudioClip bgmClip; // 用於儲存背景音樂的 AudioClip

    public Slider soundsSlider; // 用於控制音效音量的滑塊
    public Slider bgmSlider; // 用於控制背景音樂音量的滑塊

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy (gameObject);
            return;
        }

        foreach (Sound s in Sounds)
        {
            if (s.name == "BackgroundMusic")
            {
                // 如果Sound是BackgroundMusic，不做处理
                continue;
            }
            else
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        // 初始化背景音樂的AudioSource
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = 0.5f; // 設置初始音量

        PlayBackgroundMusic();

        // 初始化滑塊
        if (soundsSlider != null)
        {
            soundsSlider.value = 0.5f; // 設置初始值
            soundsSlider.onValueChanged.AddListener(delegate { AdjustSoundsVolume(soundsSlider.value); });
        }

        if (bgmSlider != null)
        {
            bgmSlider.value = 0.5f; // 設置初始值
            bgmSlider.onValueChanged.AddListener(delegate { AdjustBGMVolume(bgmSlider.value); });
        }
    }

    // 播放指定名稱的音效
    public void Play (string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name); //在 Sounds 陣列中查找具有指定名稱的音效
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name); // 若找不到指定名稱的音效，輸出警告
            return;
        }
        s.source.Play(); // 播放音效
    }

    // 停止所有音效的播放，排除背景音樂
    public void Stop()
    {
        foreach (Sound s in Sounds)
        {
            if (s.source != null && s.source.isPlaying && s.name != "BackgroundMusic")
            {
                s.source.Stop();
            }
        }
        Click_Sound(); // 播放點擊音效
    }
// 打開遊戲音效
public void VolumeOn()
{
    foreach (Sound s in Sounds)
    {
        if (s.source != null && s.source.isPlaying)
        {
            s.source.volume = 1; // 設置音量為最大
        }
    }
    soundsSlider.value = 1f; // 在循環外設置 Slider 的值為最大音量
}

// 關閉遊戲音效
public void VolumeOff()
{
    foreach (Sound s in Sounds)
    {
        if (s.source != null && s.source.isPlaying)
        {
            s.source.volume = 0; // 設置音量為0
        }
    }
    soundsSlider.value = 0f; // 在循環外設置 Slider 的值為0音量
}

// 關閉背景音樂
public void BGMVolumeOff()
{
    if (bgmSlider != null)
    {
        bgmSlider.value = 0f; // 設置 Slider 的值為0音量
    }
    if (bgmSource != null)
    {
        bgmSource.volume = 0; // 設置背景音樂音量為0
    }
}

// 打開背景音樂
public void BGMVolumeOn()
{
    if (bgmSlider != null)
    {
        bgmSlider.value = 1f; // 設置 Slider 的值為最大音量
    }
    if (bgmSource != null)
    {
        bgmSource.volume = 1f; // 設置背景音樂音量為最大
    }
}


    // 播放點擊音效
    public void Click_Sound()
    {
        Play("Click");
    }

    // 停止所有音效播放
    public void StartStop()
    {
        foreach (Sound s in Sounds)
        {
            if (s.source != null && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }

    // 播放背景音樂
    private void PlayBackgroundMusic()
    {
        if (bgmSource != null)
        {
            bgmSource.Play();
        }
    }

    // 調整所有音效的音量
    private void AdjustSoundsVolume(float volume)
    {
        foreach (Sound s in Sounds)
        {
            if (s.source != null && s.name != "BackgroundMusic")
            {
                s.source.volume = volume;
            }
        }
    }

    // 調整背景音樂的音量
    private void AdjustBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }
}
