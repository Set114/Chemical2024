using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Sound
{
    //搭配audioManger使用
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
}

public class SoundMessage
{
    public int index;           //曲目編號
    public AudioClip clip;           //曲目編號
    public GameObject sender;   //傳送者

    public SoundMessage(int Index, AudioClip Clip, GameObject Sender)
    {
        index = Index;
        clip = Clip;
        sender = Sender;
    }
}

public class AudioManager : MonoBehaviour
{
    // 跟Sound類搭配使用，用於播放音效的程式碼
    // 使用 FindObjectOfType<AudioManager>().Play("音效名稱") 來撥放音效
    // 在Hierarchy中有一個名為 AudioManager 的物件，可以自訂音效名稱
    public Sound[] Sounds;
    public Sound[] BGMList;
    public Sound[] VoiceList;
    public Sound[] SoundList;

    public static AudioManager instance;
    public AudioSource bgmSource; // 獨立的 AudioSource 變數，用於播放背景音樂
    public AudioSource voiceSource; // 獨立的 AudioSource 變數，用於播放音效
    [SerializeField] private GameObject soundSource; //音效物件
    public AudioClip bgmClip; // 用於儲存背景音樂的 AudioClip

    private SettingUIManager controlPanel;      //左上角控制板
    public Slider soundsSlider; // 用於控制音效音量的滑塊
    public Slider bgmSlider; // 用於控制背景音樂音量的滑塊
    void Awake()
    {
        if (soundsSlider == null || bgmSlider == null)
        {
            controlPanel = FindObjectOfType<SettingUIManager>();
            if (controlPanel)
            {
                soundsSlider = controlPanel.UIeffectSlider;
                bgmSlider = controlPanel.bgmSlider;
            }
        }

        if (instance != null && instance != this)
        {
            instance.Sounds = Sounds;
            instance.BGMList = BGMList;
            instance.VoiceList = VoiceList;
            instance.SoundList = SoundList;

            soundsSlider.value = instance.voiceSource.volume;
            soundsSlider.onValueChanged.AddListener(instance.AdjustSoundsVolume);
            instance.soundsSlider = soundsSlider;

            bgmSlider.value = instance.bgmSource.volume;
            bgmSlider.onValueChanged.AddListener(instance.AdjustBGMVolume);
            instance.bgmSlider = bgmSlider;

            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        /*foreach (Sound s in Sounds)
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
        }*/

        // 初始化背景音樂的AudioSource
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = 0.5f; // 設置初始音量

        PlayBackgroundMusic();

        // 初始化滑塊
        if (soundsSlider != null)
        {
            soundsSlider.value = 1.0f; // 設置初始值
            soundsSlider.onValueChanged.AddListener(delegate { AdjustSoundsVolume(soundsSlider.value); });
        }

        if (bgmSlider != null)
        {
            bgmSlider.value = 0.5f; // 設置初始值
            bgmSlider.onValueChanged.AddListener(delegate { AdjustBGMVolume(bgmSlider.value); });
        }
    }

    //用來撥放語音的
    public void PlayVoice(string chapterName)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == chapterName); //在 Sounds 陣列中查找具有指定名稱的音效
        if (s == null || s.clip == null)
        {
            Debug.LogWarning("找不到該語音： " + chapterName); // 若找不到指定名稱的音效，輸出警告
            return;
        }
        voiceSource.Stop();
        voiceSource.clip = s.clip;
        if (soundsSlider != null)
            voiceSource.volume = soundsSlider.value * s.volume;
        voiceSource.Play();
    }

    //用來撥放音效的
    public void PlaySound(int index)
    {
        GameObject tempObj = Instantiate(soundSource);
        tempObj.GetComponent<AudioSource>().volume = soundsSlider.value * SoundList[index].volume;
        tempObj.SetActive(true);
        tempObj.SendMessage("SetClip", SoundList[index].clip);
    }

    //用來撥放音效的
    public void PlaySound( SoundMessage ClipData)
    {
        GameObject tempObj = Instantiate(soundSource);
        tempObj.GetComponent<AudioSource>().volume = soundsSlider.value * SoundList[ClipData.index].volume;
        tempObj.SetActive(true);
        tempObj.SendMessage("SetClip", new SoundMessage(ClipData.index, SoundList[ClipData.index].clip, ClipData.sender));
    }

    public float GetClipLength(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name); //在 Sounds 陣列中查找具有指定名稱的音效
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name); // 若找不到指定名稱的音效，輸出警告
            return 0;
        }
        return s.clip.length;
    }

    // 停止所有音效的播放，排除背景音樂
    public void Stop()
    {
        voiceSource.Stop();
    }
    // 打開遊戲音效
    public void VolumeOn()
    {
        /*foreach (Sound s in Sounds)
         {
             if (s.source != null && s.source.isPlaying)
             {
                 s.source.volume = 1; // 設置音量為最大
             }
         }*/
        voiceSource.volume = 1f; // 設置音量為最大
        soundsSlider.value = 1f; // 在循環外設置 Slider 的值為最大音量
    }

    // 關閉遊戲音效
    public void VolumeOff()
    {
        /*foreach (Sound s in Sounds)
        {
            if (s.source != null && s.source.isPlaying)
            {
                s.source.volume = 0; // 設置音量為0
            }
        }*/

        voiceSource.volume = 0; // 設置音量為0
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

    // 停止所有音效播放
    public void StartStop()
    {
        voiceSource.Stop();
        bgmSource.Stop();
        /*foreach (Sound s in Sounds)
        {
            if (s.source != null && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }*/
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
    public void AdjustSoundsVolume(float volume)
    {
        voiceSource.volume = volume;
        /*foreach (Sound s in Sounds)
        {
            if (s.source != null && s.name != "BackgroundMusic")
            {
                s.source.volume = volume;
            }
        }*/
    }

    // 調整背景音樂的音量
    public void AdjustBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    //繼承音量設定
    public void GetVolumeSetting(float seVolume, float bgmVolume)
    {
        if (voiceSource != null)
        {
            soundsSlider.value = seVolume;
            voiceSource.volume = seVolume;
        }
        if (bgmSource != null)
        {
            bgmSlider.value = bgmVolume;
            bgmSource.volume = bgmVolume;
        }
    }
}