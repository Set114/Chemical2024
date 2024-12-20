using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PourSolution : MonoBehaviour
{
    [SerializeField] GameObject Liquid;
    [SerializeField] GameObject putpoint;
    [SerializeField] GameObject TestTube;
    public string audioSource1, audioSource2, audioSource3, audioSource4, audioSource5;
    public string[] Text;
    public Text text;
    bool isputed = false;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("mouth"))
        {
            Liquid.SetActive(true);
        }
        else if (other.gameObject.name.Equals("TestTube") && TestTube.transform.Find("Liquid").gameObject.activeSelf)
        {
            isputed = true;
            StartCoroutine(WaitForAudioToEnd());
        }
    }

    void Update()
    {
        if (isputed)
        {
            TestTube.gameObject.transform.position = putpoint.transform.position;
            TestTube.gameObject.transform.rotation = putpoint.transform.rotation;
        }
    }
    public float Play(string audioSource)
    {
        float waitSeconds = audioManager.GetClipLength(audioSource);
        audioManager.Play(audioSource);
        return waitSeconds;
    }
    private IEnumerator WaitForAudioToEnd()
    {
        //audioSource1.Play(); // 播放音频
        float waitSeconds = audioManager.GetClipLength(audioSource1);
        audioManager.Play(audioSource1);
        text.text = Text[0];

        // 等待音频播放完成
        yield return new WaitForSeconds(waitSeconds);

        // audioSource2.Play();
    }

}
