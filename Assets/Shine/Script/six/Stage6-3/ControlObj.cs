using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlObj : MonoBehaviour
{
    public bool ControlUp, ControlDown;
    public float Speed;

    //public GameObject[] H2, N2, NH3;
    //public GameObject[] H2UI, N2UI, NH3UI;
    public GameObject[] State3D,StateUI;
    public float Rate;
    public float OriginalPosY;
    public bool isOpen;
    public GameObject[] InfoUI;
    bool isDown, isUp;
    public GameObject Final;

    public GameObject ObjTips;
    public GameObject FinishedUI;
    public Sprite FinishedSprite;

    // Start is called before the first frame update
    void Awake()
    {
        OriginalPosY = transform.localPosition.y;
    }
    private void Start()
    {
        FindObjectOfType<Shine_GM>().StartTimes6_L[2] = System.DateTime.Now.ToString();
        InfoUI[0].SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        /*  Rate =( transform.localPosition.y / OriginalPosY)*100;
          if (isOpen)
          {
              if (Rate > 99f & Rate < 100f)
              {
                  DeplayData();
                  if (isDown&&!isUp)
                  {
                      InfoUI[1].SetActive(true);

                      isUp = true;
                      StartCoroutine(FinalCheck());
                  }
              }
              if (Rate > 98f & Rate < 99f)
              {
                  for (int i = 0; i < H2.Length - 4; i++)
                  {
                      H2[i].SetActive(false);
                      N2[i].SetActive(false);
                      H2UI[i].SetActive(false);
                      N2UI[i].SetActive(false);
                      NH3[i].SetActive(true);
                      NH3UI[i].SetActive(true);
                  }
                  for (int j = 5; j > H2.Length - 4; j--)
                  {
                      H2[j].SetActive(true);
                      N2[j].SetActive(true);
                      H2UI[j].SetActive(true);
                      N2UI[j].SetActive(true);
                      NH3[j].SetActive(false);
                      NH3UI[j].SetActive(false);
                  }
              }
              if (Rate > 97f & Rate < 98f)
              {
                  for (int i = 0; i < H2.Length - 3; i++)
                  {
                      H2[i].SetActive(false);
                      N2[i].SetActive(false);
                      H2UI[i].SetActive(false);
                      N2UI[i].SetActive(false);
                      NH3[i].SetActive(true);
                      NH3UI[i].SetActive(true);
                  }
                  for (int j = 5; j > H2.Length - 3; j--)
                  {
                      H2[j].SetActive(true);
                      N2[j].SetActive(true);
                      H2UI[j].SetActive(true);
                      N2UI[j].SetActive(true);
                      NH3[j].SetActive(false);
                      NH3UI[j].SetActive(false);
                  }
              }
              if (Rate > 96f & Rate < 97f)
              {
                  for (int i = 0; i < H2.Length - 2; i++)
                  {
                      H2[i].SetActive(false);
                      N2[i].SetActive(false);
                      H2UI[i].SetActive(false);
                      N2UI[i].SetActive(false);
                      NH3[i].SetActive(true);
                      NH3UI[i].SetActive(true);
                  }
                  for (int j = 5; j > H2.Length - 2; j--)
                  {
                      H2[j].SetActive(true);
                      N2[j].SetActive(true);
                      H2UI[j].SetActive(true);
                      N2UI[j].SetActive(true);
                      NH3[j].SetActive(false);
                      NH3UI[j].SetActive(false);
                  }
              }
              if (Rate > 95f & Rate < 96f)
              {
                  for (int i = 0; i < H2.Length-1; i++)
                  {
                      H2[i].SetActive(false);
                      N2[i].SetActive(false);
                      H2UI[i].SetActive(false);
                      N2UI[i].SetActive(false);
                      NH3[i].SetActive(true);
                      NH3UI[i].SetActive(true);
                  }
                  for (int j = 5; j > H2.Length - 1; j--) {
                      H2[j].SetActive(true);
                      N2[j].SetActive(true);
                      H2UI[j].SetActive(true);
                      N2UI[j].SetActive(true);
                      NH3[j].SetActive(false);
                      NH3UI[j].SetActive(false);
                  }
              }
              if (Rate < 95f)
              {
                  for (int i = 0; i < H2.Length; i++)
                  {
                      H2[i].SetActive(false);
                      N2[i].SetActive(false);
                      H2UI[i].SetActive(false);
                      N2UI[i].SetActive(false);
                      NH3[i].SetActive(true);
                      NH3UI[i].SetActive(true);
                  }
                  H2[0].SetActive(true);
                  N2[0].SetActive(true);
                  H2UI[0].SetActive(true);
                  N2UI[0].SetActive(true);
                 if(!isDown) isDown = true;
              }
          }*/
        if (!Final.active)
        {
            if (ControlUp && isDown)
            {

                InfoUI[0].SetActive(false);
                if (transform.localPosition.y < 1.172405f)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Speed, transform.localPosition.z);
                    State3D[0].SetActive(false);
                    State3D[1].SetActive(false);
                    State3D[2].SetActive(true);
                    StateUI[0].SetActive(false);
                    StateUI[1].SetActive(false);
                    StateUI[2].SetActive(true);
                    InfoUI[1].SetActive(true);
                    if (InfoUI[1].active)
                    {
                        StartCoroutine(FinalCheck());
                    }
                }
            }
            if (ControlDown)
            {
                InfoUI[0].SetActive(false);
                if (transform.localPosition.y > 1.0913f)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - Speed, transform.localPosition.z);
                    State3D[0].SetActive(false);
                    State3D[1].SetActive(true);
                    State3D[2].SetActive(false);
                    StateUI[0].SetActive(false);
                    StateUI[1].SetActive(true);
                    StateUI[2].SetActive(false);
                    isDown = true;

                }

            }
        }
    }
    public void ControlUpObj(bool state) {
      
            ControlUp = state;
        
    }
    public void ControlDownObj(bool state)
    {
       
            ControlDown = state;
        
    }
    //¶}¾÷
    public void Open()
    {
        if (!Final.active)
        {
            isOpen = true;
            //DeplayData();
            StateUI[0].SetActive(true);
            State3D[0].SetActive(true);
        }
    }
    void DeplayData() {
        /* for (int i = 0; i < H2.Length; i++) {
             H2[i].SetActive(true);
             N2[i].SetActive(true);
             H2UI[i].SetActive(true);
             N2UI[i].SetActive(true);
             NH3[i].SetActive(false);
             NH3UI[i].SetActive(false);
         }
         NH3[0].SetActive(true);
         NH3UI[0].SetActive(true);*/
      
    }

    public void ReButton()
    {
        for (int i = 0; i < State3D.Length; i++)
        {
            State3D[i].SetActive(false);
            StateUI[i].SetActive(false);
        }
        isOpen = false;
        transform.localPosition = new Vector3(transform.localPosition.x,OriginalPosY, transform.localPosition.z);
        isDown = false;
        isUp = false;
        Final.SetActive(false);
        InfoUI[0].SetActive(true);
        InfoUI[1].SetActive(false);
        ObjTips.SetActive(true);

    }
    IEnumerator FinalCheck()
    {
        yield return new WaitForSeconds(3f);

        //if (isOpen && isUp)
        //{
            Final.SetActive(true);
            FindObjectOfType<Shine_GM>().EndTimes6_L[2] = System.DateTime.Now.ToString();
        FinishedUI.GetComponent<Image>().sprite = FinishedSprite;

        // }
    }
}
