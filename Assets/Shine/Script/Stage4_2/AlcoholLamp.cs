using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlcoholLamp : MonoBehaviour
{
    [Header("解說文字 0鎂1鋅2銅")]
    public GameObject[] InfoObj;

    public int MagnesiumTime;
    public Text MagnesiumText;
    public GameObject Magnesium_L, Magnesium_R, Magnesium, OldMagnesium;
    public GameObject MagnesiumLight_L, MagnesiumLight_R, MagnesiumAsh_L, MagnesiumAsh_R, MagnesiumAsh;
    public GameObject MagnesiumCheckUI;

    public int ZincTime;
    public Text ZincText;
    public GameObject Zinc_L, Zinc_R, Zinc, OldZinc;
    public GameObject ZincLight_L, ZincLight_R, ZincAsh_L, ZincAsh_R, ZincAsh;
    public GameObject ZincCheckUI;

    public Animator Copper_L, Copper_R, Copper;
    public GameObject Copper_LObj, Copper_RObj, CopperObj, CopperCheckUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider Hit)
    {
        if (Hit.GetComponent<Collider>().tag == "Magnesium")
        {
            InvokeRepeating("MagnesiumTiming", 0, 1);
            InfoObj[0].SetActive(true);
        }
        if (Hit.GetComponent<Collider>().tag == "Zinc")
        {
            InvokeRepeating("ZincTiming", 0, 1);
            InfoObj[1].SetActive(true);

        }
        if (Hit.GetComponent<Collider>().tag == "Copper")
        {
            InfoObj[2].SetActive(true);
            Copper_L.SetTrigger("ChangeColor");
            Copper_R.SetTrigger("ChangeColor");
            StartCoroutine(WaitCopper());

        }
    }
    private void OnTriggerExit(Collider Hit)
    {
        if (Hit.GetComponent<Collider>().tag == "Magnesium")
        {
            CancelInvoke("MagnesiumTiming");
        }
        if (Hit.GetComponent<Collider>().tag == "Zinc")
        {
            CancelInvoke("Zinc");
        }
       
    }
    #region 鎂
    void MagnesiumTiming()
    {
        MagnesiumTime++;
        MagnesiumTime = Mathf.Clamp(MagnesiumTime, 0, 14);
        MagnesiumText.text = "鎂粉加熱至燃燒需10秒！ \n 目前加熱秒數：" + Mathf.Clamp(MagnesiumTime, 0, 10) + "秒";
        if (MagnesiumTime >= 10 && MagnesiumTime < 14)
        {
            MagnesiumLight_L.SetActive(true);
            MagnesiumLight_R.SetActive(true);

        }
        if (MagnesiumTime >= 14)
        {
            MagnesiumLight_L.transform.parent.gameObject.SetActive(false);
            MagnesiumAsh_L.SetActive(true);
            MagnesiumLight_R.transform.parent.gameObject.SetActive(false);
            MagnesiumAsh_R.SetActive(true);
            OldMagnesium.SetActive(false);
            MagnesiumAsh.SetActive(true);
            MagnesiumCheckUI.SetActive(true);

            CancelInvoke("MagnesiumTiming");
            StartCoroutine(WaitMagnesium());
        }
    }
    IEnumerator WaitMagnesium()
    {
        yield return new WaitForSeconds(5f);
        Magnesium_L.SetActive(false);
        Magnesium_R.SetActive(false);
        Magnesium.SetActive(true);
        if (FindObjectOfType<Stage4_2>().isLeft) FindObjectOfType<Stage4_2>().isLeft = false;
        if (FindObjectOfType<Stage4_2>().isRight) FindObjectOfType<Stage4_2>().isRight = false;
        InfoObj[0].SetActive(false);
        FindObjectOfType<Stage4_2>().State[0] = true;

    }
    #endregion
    #region 鋅
    void ZincTiming()
    {
        ZincTime+=10;
        ZincTime = Mathf.Clamp(ZincTime, 0, 100);
        ZincText.text = "鋅粉加熱至燃燒需60秒！ \n 目前加熱秒數：" + Mathf.Clamp(ZincTime, 0, 60) + "秒";
        if (ZincTime >= 60 && ZincTime < 100)
        {
            ZincLight_L.SetActive(true);
            ZincLight_R.SetActive(true);

        }
        if (ZincTime >= 100)
        {
            ZincLight_L.transform.parent.gameObject.SetActive(false);
            ZincAsh_L.SetActive(true);
            ZincLight_R.transform.parent.gameObject.SetActive(false);
            ZincAsh_R.SetActive(true);
            OldZinc.SetActive(false);
            ZincAsh.SetActive(true);
            ZincCheckUI.SetActive(true);

            CancelInvoke("ZincTiming");
            StartCoroutine(WaitZinc());
        }
    }
    IEnumerator WaitZinc()
    {
        yield return new WaitForSeconds(5f);

        Zinc_L.SetActive(false);
        Zinc_R.SetActive(false);
        Zinc.SetActive(true);
        if (FindObjectOfType<Stage4_2>().isLeft) FindObjectOfType<Stage4_2>().isLeft = false;
        if (FindObjectOfType<Stage4_2>().isRight) FindObjectOfType<Stage4_2>().isRight = false;
        InfoObj[1].SetActive(false);
        FindObjectOfType<Stage4_2>().State[1] = true;

    }
    #endregion
    IEnumerator WaitCopper()
    {
        yield return new WaitForSeconds(5f);
        CopperCheckUI.SetActive(true);

        Copper_LObj.SetActive(false);
        Copper_RObj.SetActive(false);
        CopperObj.SetActive(true);
        Copper.SetTrigger("ChangeColor");

        if (FindObjectOfType<Stage4_2>().isLeft) FindObjectOfType<Stage4_2>().isLeft = false;
        if (FindObjectOfType<Stage4_2>().isRight) FindObjectOfType<Stage4_2>().isRight = false;
        InfoObj[2].SetActive(false);
        FindObjectOfType<Stage4_2>().State[2] = true;
    }

}