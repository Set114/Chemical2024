using System.Collections;
using UnityEngine;

public class PlaySpeechAudio_M : MonoBehaviour
{
    public bool unielf = false; // 標記是否為 UI 語音
    public string[] unielfnames; // 改為陣列以存放多個語音名稱

    private int currentLevel = 0; // 當前關卡
    private AudioManager audioManager;
    private AudioSource audioSource;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>(); // 確保找到音訊管理器的實例
        if (audioManager != null)
        {
            audioSource = audioManager.GetComponent<AudioSource>(); // 確保從音訊管理器獲取 AudioSource
        }
        else
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
    }

    void OnEnable()
    {
        StartCoroutine(PlaySpeech_M()); // 當物件啟用時，開始執行播放語音的協程
    }

    // 定義一個協程，根據物件名稱播放對應的語音
    IEnumerator PlaySpeech_M()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on AudioManager.");
            yield break; // 如果沒有找到 AudioSource，終止協程
        }

        if (unielf)
        {
            foreach (string name in unielfnames)
            {
                // 播放語音
                audioManager.Play(name);

                // 等待直到前一個語音播放完畢
                yield return new WaitUntil(() => !audioSource.isPlaying);

                // 等待 3 秒再播放下一個語音，或者下一段語音的長度（以防止語音長度不同）
                yield return new WaitForSeconds(3f); // 調整成你需要的等待時間
            }
        }

        // 播放完畢後使自己的物件消失
        //gameObject.SetActive(false);
    }

    // 設置當前關卡的方法，用於外部調用
    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }
}
