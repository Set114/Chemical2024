using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Stage4Test : MonoBehaviour
{
    [Header("�O�����D���ﵪ��")]
    public bool[] Ans5State;
    [Header("�O�����D��ܪ�����")]
    public string[] Ans5;
    [Header("���D�D��")]
    public GameObject[] Iteam5;
    [Header("���D���G")]
    public GameObject[] FinalIteam5;
    //--�Ĥ@�D
    [Header("�Ĥ@�D�T�ӿﶵ")]
    public GameObject[] Iteam1Ans;

    //--�ĤG�D
    [Header("�ĤG�D�T�ӿﶵ")]
    public Image[] Iteam2Ans;

    //--�ĤT�D
    [Header("�ĤT�D2�ӿﶵ")]
    public Image[] Iteam3Ans;

    //--�ĥ|�D
    [Header("�ĥ|�D2�ӿﶵ")]
    public Image[] Iteam4Ans;

    [Header("�Ĥ��D3�ӿﶵ")]
    public Image[] Iteam5Ans;

    public Sprite SelectSprite;

    public int Score;
    public TMP_Text ScoreText;
    public TMP_Text[] Iteams;

    private int iUnitSheetIndex = 7;
    public void CheckIteam1(string ObjName) {
        switch (ObjName) {
            case "Leaves":
                Ans5[0] = "��";
                break;
            case "Hay":
                Ans5[0] = "����";
                break;

            case "Branches":
                Ans5[0] = "��K";
                break;

        }
        for (int i = 0; i < Iteam1Ans.Length; i++) {
            if (Iteam1Ans[i].active) {
                if (ObjName == "Hay") {
                    Ans5State[0] = true;
                }
              
            }
        }
        if (Ans5State[0])
        {
            FinalIteam5[0].transform.GetChild(3).gameObject.SetActive(true);
        }
        else {
            FinalIteam5[0].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[0].SetActive(false);
        FinalIteam5[0].SetActive(true);
    }
    public void CheckIteam2()
    {
        for (int i = 0; i < Iteam2Ans.Length; i++)
        {
            if (Iteam2Ans[2].sprite == SelectSprite) {
                Ans5State[1] = true;
            }
            if (Iteam2Ans[i].sprite == SelectSprite)
            {
                Ans5[1] = Iteam2Ans[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
            }
        }
        if (Ans5State[1])
        {
            FinalIteam5[1].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            FinalIteam5[1].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[1].SetActive(false);
        FinalIteam5[1].SetActive(true);
    }
    public void CheckIteam3()
    {
        for (int i = 0; i < Iteam3Ans.Length; i++)
        {
            if (Iteam3Ans[0].sprite == SelectSprite)
            {
                Ans5State[2] = true;
            }
            if (Iteam3Ans[i].sprite == SelectSprite)
            {

                Ans5[2] = Iteam3Ans[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
            }
        }
        if (Ans5State[2])
        {
            FinalIteam5[2].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            FinalIteam5[2].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[2].SetActive(false);
        FinalIteam5[2].SetActive(true);
    }
    public void CheckIteam4()
    {
        for (int i = 0; i < Iteam4Ans.Length; i++)
        {
            if (Iteam4Ans[1].sprite == SelectSprite)
            {
                Ans5State[3] = true;
            }
            if (Iteam4Ans[i].sprite == SelectSprite)
            {

                Ans5[3] = Iteam4Ans[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
            }
        }
        if (Ans5State[3])
        {
            FinalIteam5[3].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            FinalIteam5[3].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[3].SetActive(false);
        FinalIteam5[3].SetActive(true);
    }

    public void CheckIteam5()
    {
        for (int i = 0; i < Iteam5Ans.Length; i++)
        {
            if (Iteam5Ans[0].sprite == SelectSprite)
            {
                Ans5State[4] = true;
            }
            if (Iteam5Ans[i].sprite == SelectSprite)
            {

                Ans5[4] = Iteam5Ans[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
            }
        }
        if (Ans5State[4])
        {
            FinalIteam5[4].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            FinalIteam5[4].transform.GetChild(4).gameObject.SetActive(true);
        }
        Iteam5[4].SetActive(false);
        FinalIteam5[4].SetActive(true);
    }

    public TMP_Text GetScoreText()
    {
        return ScoreText;
    }

    public void Final() {
        for (int i = 0; i < Ans5State.Length; i++) {
   
           // if (i>0)
                Iteams[i].text = (i+1) + "." + Ans5[i];

            FindObjectOfType<Shine_GM>().TestAns4[i]=Iteams[i].text;
            if (Ans5State[i])
            {
                Score += 20;
                Iteams[i].color = Color.black;
                FindObjectOfType<Shine_GM>().TestScroe4[i]="20";
            }
            else
            {
                Iteams[i].color = Color.red;
                FindObjectOfType<Shine_GM>().TestScroe4[i]="0";
            }
        }

        ScoreText.text = Score + "��";
        ScoreText.color = Score < 60 ? Color.red : Color.black;

        //FindObjectOfType<DataInDevice>().AddDataExcel(iUnitSheetIndex);
        FindObjectOfType<Shine_GM>().Save4TestDataExcel();
    }
}
