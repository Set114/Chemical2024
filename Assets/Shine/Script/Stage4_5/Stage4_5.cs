using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Stage4_5 : MonoBehaviour
{
    public GameObject[] TableMarbles;
    public GameObject[] CupMarbles;
    public GameObject[] MarbleUIs;
    public GameObject[] MarbleInfos;
    public GameObject[] Hs;
    public GameObject[] CO2s;

    public Vector3[] TableMarblesPos;
    public GameObject Finished,ReButtonUI;
    //�����}�l�ɶ�
    private void OnEnable()
    {
        FindObjectOfType<Shine_GM>().StartTimes4_L[4] = System.DateTime.Now.ToString();
    }
    public void FinishRecord()
    {
        FindObjectOfType<Shine_GM>().EndTimes4_L[4] = System.DateTime.Now.ToString();
        FindObjectOfType<Shine_GM>().Save4LearnDataExcel();
    }
    private void Awake()
    {
        TableMarblesPos[0] = TableMarbles[0].transform.position;
        TableMarblesPos[1] = TableMarbles[1].transform.position;
        TableMarblesPos[2] = TableMarbles[2].transform.position;

    }
    public void SetTableMarblesCollider(bool state) {
        for (int i = 0; i < TableMarbles.Length; i++) {
            TableMarbles[i].GetComponent<XRGrabInteractable>().enabled = state;
        }
    }
    public void ReButton() {
        SetTableMarblesCollider(true);
       TableMarbles[0].transform.position= TableMarblesPos[0];
       TableMarbles[1].transform.position= TableMarblesPos[1];
        TableMarbles[2].transform.position= TableMarblesPos[2];
        for (int i = 0; i < TableMarbles.Length; i++)
        {
            TableMarbles[i].GetComponent<Shine_MouseController>().Reset();
            TableMarbles[i].SetActive(true);
            CupMarbles[i].SetActive(false);
            MarbleUIs[i].SetActive(false);
            MarbleInfos[i].SetActive(false);
        }
        MarbleInfos[3].SetActive(true);
        for (int j = 0; j < Hs.Length; j++) {
            Hs[j].GetComponent<MoveTowards2>().Reset();
            CO2s[j].GetComponent<MoveTowards2>().Reset();

        }
    }
}
