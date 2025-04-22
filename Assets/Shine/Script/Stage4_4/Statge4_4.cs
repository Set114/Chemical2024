using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statge4_4 : MonoBehaviour
{
    public MoveTowards1[] AtomObjs;
    public GameObject[] AtomStateObjs;

    public GameObject[] AtomInfo;
    public GameObject[] SFXs;
    public GameObject[] ContainerMouths;
    public Sprite CheckSprite, DefaultSprite;
    public Image[] AtomImages;

    public GameObject[] FinishUI;
    [Header("­»")]
    public Transform Fragrant;
    public Vector3 FragrantPos;
    public Vector3 FragrantAngle;
    // Start is called before the first frame update
    void Awake()
    {
        FragrantPos = Fragrant.position;
        FragrantAngle = Fragrant.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ReButton();
        }
    }
    public void ReButton() {
        for (int j = 0; j < AtomStateObjs.Length; j++) {
            SFXs[j].SetActive(false) ;
            AtomStateObjs[j].SetActive(false);
            AtomInfo[j].SetActive(false);
            ContainerMouths[j].GetComponent<TouchSticks>().enabled = true;
            ContainerMouths[j].SetActive(true);

            AtomImages[j].sprite = DefaultSprite;
        }
        AtomInfo[0].SetActive(true);

        Fragrant.position = FragrantPos;
        Fragrant.eulerAngles = FragrantAngle;
        for (int i = 0; i < AtomObjs.Length; i++)
        {
            AtomObjs[i].Reset();
        }
    }
}
