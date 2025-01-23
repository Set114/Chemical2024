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
    public GameObject Finish4_2;
    public bool[] State;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (State[0]&& State[1]&& State[2])
        {
            Finish4_2.SetActive(true);
        }
        else {
            Finish4_2.SetActive(false);
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
        FindObjectOfType<AlcoholLamp>().Copper_L.SetTrigger("Reset");
        FindObjectOfType<AlcoholLamp>().Copper_R.SetTrigger("Reset");
        FindObjectOfType<AlcoholLamp>().Copper.SetTrigger("Reset");
        FindObjectOfType<AlcoholLamp>().MagnesiumTime = 0;
        FindObjectOfType<AlcoholLamp>().ZincTime = 0;

    }
}
