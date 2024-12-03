using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchSticks : MonoBehaviour
{
    public int ID;
    public Statge4_4 Statge4_4Obj;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().name == "IncenseSticks ") {
            switch (ID) {
                //O2
                case 0:
                    other.transform.GetChild(0).gameObject.SetActive(true);
                    Statge4_4Obj.AtomStateObjs[0].SetActive(true);
                    Statge4_4Obj.AtomInfo[0].SetActive(true);
                    this.GetComponent<TouchSticks>().enabled = false;
                    Statge4_4Obj.AtomImages[0].sprite = Statge4_4Obj.CheckSprite;
                    StartCoroutine(Wait());
                    break;
                    //CO2
                case 1:
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    Statge4_4Obj.AtomStateObjs[1].SetActive(true);
                    Statge4_4Obj.AtomInfo[1].SetActive(true);
                    this.GetComponent<TouchSticks>().enabled = false;
                    Statge4_4Obj.AtomImages[1].sprite = Statge4_4Obj.CheckSprite;
                    StartCoroutine(Wait());

                    break;
                    //Air
                case 2:
                    other.transform.GetChild(2).gameObject.SetActive(true);
                    Statge4_4Obj.AtomStateObjs[2].SetActive(true);
                    Statge4_4Obj.AtomInfo[2].SetActive(true);
                    this.GetComponent<TouchSticks>().enabled = false;

                    Statge4_4Obj.AtomImages[2].sprite = Statge4_4Obj.CheckSprite;
                    StartCoroutine(Wait());

                    break;
            }
        }
    }
    IEnumerator Wait() {
        yield return new WaitForSeconds(5);
        for (int j = 0; j < Statge4_4Obj.AtomStateObjs.Length; j++)
        {
            Statge4_4Obj.AtomStateObjs[j].SetActive(false);
            Statge4_4Obj.AtomInfo[j].SetActive(false);
            Statge4_4Obj.SFXs[j].SetActive(false);

        }
        this.gameObject.SetActive(false);
        if (Statge4_4Obj.AtomImages[0].sprite == Statge4_4Obj.CheckSprite && Statge4_4Obj.AtomImages[1].sprite == Statge4_4Obj.CheckSprite && Statge4_4Obj.AtomImages[2].sprite == Statge4_4Obj.CheckSprite) {
            Statge4_4Obj.Finish.SetActive(true);
        }

    }
}
