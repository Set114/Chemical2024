using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PourSolution : MonoBehaviour
{
    [SerializeField] GameObject Liquid;
    [SerializeField] GameObject putpoint;
    [SerializeField] GameObject TestTube;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    public AudioSource audioSource5;

    public string[] Text;
    public Text text;
    bool isputed = false;
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

    private IEnumerator WaitForAudioToEnd()
    {
        audioSource1.Play(); // 播放音频
        text.text = Text[0];

        // 等待音频播放完成
        yield return new WaitForSeconds(audioSource1.clip.length);

        // audioSource2.Play();
    }

}
