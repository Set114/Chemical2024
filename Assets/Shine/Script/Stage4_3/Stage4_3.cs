using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage4_3 : MonoBehaviour
{
    public GameObject[] DeskObjs;
    public GameObject[] TubeObjs;
    public Toggle[] ObjToggles;
    public GameObject TubeClloder;

    public GameObject ParticleEffects, BalloonNoAir, Balloon, ContainerMouth;
    public GameObject[] Tips;

    public Vector3[] SaveDeskObjsPos;
    public Vector3[] SaveDeskObjsAng;

    public GameObject[] FinishButtons;
    bool isRecordData;

    //�����}�l�ɶ�
    private void OnEnable()
    {
        FindObjectOfType<Shine_GM>().StartTimes4_L[2] = System.DateTime.Now.ToString();
    }
    // Start is called before the first frame update
    void Awake()
    {
        for(int i=0;i< DeskObjs.Length; i++)
        {
            SaveDeskObjsPos[i] = DeskObjs[i].transform.position;
            SaveDeskObjsAng[i] = DeskObjs[i].transform.eulerAngles;
        }
    }

    public void Rebutton() {
        for (int i = 0; i < DeskObjs.Length; i++)
        {
            DeskObjs[i].GetComponent<Shine_MouseController>().Reset();
           DeskObjs[i].transform.position= SaveDeskObjsPos[i];
            DeskObjs[i].transform.eulerAngles = SaveDeskObjsAng[i];
            TubeObjs[i].SetActive(false);
            DeskObjs[i].SetActive(true);
            ObjToggles[i].isOn = false;
            Tips[i].SetActive(false);
            FinishButtons[i].SetActive(false);
        }
        Tips[5].SetActive(true);
        BalloonNoAir.SetActive(false);
        Balloon.SetActive(false);
        ParticleEffects.SetActive(false);
        ContainerMouth.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ObjTouchTube(int ObjID) {
        DeskObjs[ObjID].SetActive(false);
        TubeObjs[ObjID].SetActive(true);
        TubeClloder.SetActive(false);
        if (ObjID == 0 || ObjID == 1 || ObjID == 2)
        {
            StartCoroutine(Effective(ObjID));
        }
        if (ObjID ==3 || ObjID == 4)
        {
            StartCoroutine(UnEffective(ObjID));

        }
    }
    IEnumerator Effective(int ObjID) {
        for (int i = 0; i < DeskObjs.Length; i++)
        {
            Tips[i].SetActive(false);
        }
            Tips[ObjID].SetActive(true);
        Tips[5].SetActive(false);
        ObjToggles[ObjID].isOn = true;
        ParticleEffects.SetActive(true);
        BalloonNoAir.SetActive(true);
        yield return new WaitForSeconds(5f);
        BalloonNoAir.SetActive(false);
        Balloon.SetActive(true);
        yield return new WaitForSeconds(5f);
       // Tips[ObjID].SetActive(false);
        ParticleEffects.SetActive(false);
        Balloon.SetActive(false);
        TubeObjs[ObjID].SetActive(false);
        CheckAnswer();
    }
    IEnumerator UnEffective(int ObjID)
    {
        for (int i = 0; i < DeskObjs.Length; i++)
        {
            Tips[i].SetActive(false);
        }
        Tips[ObjID].SetActive(true);
        ObjToggles[ObjID].isOn = true;
        BalloonNoAir.SetActive(true);
        yield return new WaitForSeconds(5f);
       // Tips[ObjID].SetActive(false);
        BalloonNoAir.SetActive(false);
        TubeObjs[ObjID].SetActive(false);
        CheckAnswer();

    }
    void CheckAnswer()
    {
        TubeClloder.SetActive(true);

        if (ObjToggles[0].isOn && ObjToggles[1].isOn && ObjToggles[2].isOn && ObjToggles[3].isOn && ObjToggles[4].isOn)
        {
            for (int i = 0; i < DeskObjs.Length; i++)
            {
                FinishButtons[i].SetActive(true);
            }
            if (!isRecordData)
            {
                FindObjectOfType<Shine_GM>().EndTimes4_L[2] = System.DateTime.Now.ToString();
                isRecordData = true;
            }
        }
    }

}
