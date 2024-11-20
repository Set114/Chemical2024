using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weighing : MonoBehaviour
{
    public GameObject Text_UI;
    public LevelEndSequence levelEndSequence;
    bool isEnd = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Vial_fake"))
        {
            Text_UI.SetActive(true);
            if (!other.transform.Find("Liquid (1)").gameObject.activeSelf)
            {
                StartCoroutine(WaitForAudioToEnd(other.gameObject));
            }
            else
            {
                // other.GetComponentInChildren<PourSolution>().audioSource5.Play();
                //other.GetComponentInChildren<PourSolution>().text.text = other.GetComponentInChildren<PourSolution>().Text[4];
                if (!isEnd) 
                {
                    levelEndSequence.EndLevel(false, true, 1f, 6f, 1f, "1", () => { });
                    isEnd = true;
                }
                
            }
        }
    }
    private IEnumerator WaitForAudioToEnd(GameObject gameObject)
    {
        gameObject.GetComponentInChildren<PourSolution>().audioSource2.Play();
        gameObject.GetComponentInChildren<PourSolution>().text.text = gameObject.GetComponentInChildren<PourSolution>().Text[1];
        yield return new WaitForSeconds(gameObject.GetComponentInChildren<PourSolution>().audioSource2.clip.length);
        gameObject.GetComponentInChildren<PourSolution>().audioSource3.Play();
        gameObject.GetComponentInChildren<PourSolution>().text.text = gameObject.GetComponentInChildren<PourSolution>().Text[2];
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("Vial_fake"))
        {
            Text_UI.SetActive(false);
        }
    }
}
