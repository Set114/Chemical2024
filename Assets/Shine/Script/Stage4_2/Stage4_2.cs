using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_2 : MonoBehaviour
{
    [Header("0Áâ1¾N2»É")]
    public GameObject[] Spoons;
    [Header("0Áâ1¾N2»É")]
    public GameObject[] LeftHandSpoons;
    [Header("0Áâ1¾N2»É")]
    public GameObject[] RightHandSpoons;
    public Animator LeftAnim;
    public Animator RightAnim;
    public bool isLeft, isRight;
    public GameObject[] Finish4_2Button;
    public bool[] State;
    public AlcoholLamp AlcoholLamp;

    bool isRecordData;

    //¬ö¿ý¶}©l®É¶¡
    private void OnEnable()
    {
        FindObjectOfType<Shine_GM>().StartTimes4_L[1] = System.DateTime.Now.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (State[0]&& State[1]&& State[2])
        {
            Finish4_2Button[0].SetActive(true);
            Finish4_2Button[1].SetActive(true);
            Finish4_2Button[2].SetActive(true);
            if (!isRecordData)
            {
                FindObjectOfType<Shine_GM>().EndTimes4_L[1] = System.DateTime.Now.ToString();
                isRecordData = true;
            }
        }
        else {
            Finish4_2Button[0].SetActive(false);
            Finish4_2Button[1].SetActive(false);
            Finish4_2Button[2].SetActive(false);

        }
    }
    public void GetObj(int i) {
        if (LeftAnim.GetFloat("Trigger") > 0 || LeftAnim.GetFloat("Grip") > 0) {
            if (!isLeft&& !Spoons[i].transform.GetChild(2).GetChild(0).GetChild(1).gameObject.active)
            {
                Spoons[i].SetActive(false);
                LeftHandSpoons[i].SetActive(true);
                isLeft = true;
            }
        }
        if (RightAnim.GetFloat("Trigger") > 0 || RightAnim.GetFloat("Grip") > 0)
        {
            if (!isRight && !Spoons[i].transform.GetChild(2).GetChild(0).GetChild(1).gameObject.active)
            {
                Spoons[i].SetActive(false);
                RightHandSpoons[i].SetActive(true);
                isRight = true;
            }
        }
    }

    public void ReButton() {
        AlcoholLamp.Copper_L.SetTrigger("Reset");
        AlcoholLamp.Copper_R.SetTrigger("Reset");
        AlcoholLamp.Copper.SetTrigger("Reset");
        AlcoholLamp.MagnesiumTime = 0;
        AlcoholLamp.ZincTime = 0;
        AlcoholLamp.InfoObj[1].SetActive(false);
        AlcoholLamp.InfoObj[0].SetActive(true);
        AlcoholLamp.InfoObj[2].SetActive(false);
        AlcoholLamp.InfoObj[3].SetActive(false);
        for (int i = 0; i < Spoons.Length; i++) {
            Spoons[i].SetActive(true);
            Spoons[i].transform.GetChild(0).gameObject.SetActive(true);
            Spoons[i].transform.GetChild(1).gameObject.SetActive(false);
            Spoons[i].transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(false);
            LeftHandSpoons[i].SetActive(false);
            LeftHandSpoons[i].transform.GetChild(0).gameObject.SetActive(true);
            LeftHandSpoons[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            LeftHandSpoons[i].transform.GetChild(1).gameObject.SetActive(false);

            RightHandSpoons[i].SetActive(false);
            RightHandSpoons[i].transform.GetChild(0).gameObject.SetActive(true);
            RightHandSpoons[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            RightHandSpoons[i].transform.GetChild(1).gameObject.SetActive(false);
            State[i] = false;

        }
        isRight = false;
        isLeft = false;

    }
}
