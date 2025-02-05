using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Atom
{
    public string name;
    public int cost;
}

public class Tutorial_3_1 : MonoBehaviour
{
    [Header("商店")]
    [Tooltip("商店")]
    [SerializeField] private GameObject shop;
    [Tooltip("商店賣的原子")]
    [SerializeField] private List<Atom> atoms_Sell;
    [Tooltip("經費文字")]
    [SerializeField] private Text myMoneyText;
    [Tooltip("商店原子數量文字")]
    [SerializeField] private Text countText_shopC, countText_shopO2, countText_shopN2,
        countText_shopH, countText_shopFe;

    [Header("工作區")]
    [Tooltip("組合原子的工作區")]
    [SerializeField] private GameObject workSpace;
    [Tooltip("原子Prefab")]
    [SerializeField] private GameObject atomC_Prefab, atomO2_Prefab, atomN2_Prefab,
        atomH_Prefab, atomFe_Prefab;
    [Tooltip("生成出的原子")]
    private GameObject atomC, atomO2, atomN2, atomH, atomFe;
    [Tooltip("原子生成點")]
    [SerializeField] private Transform atomC_Spawn, atomO2_Spawn, atomN2_Spawn,
    atomH_Spawn, atomFe_Spawn;

    [Tooltip("工作區內存在的原子")]
    [SerializeField] private List<Atom> atoms_AreaA, atoms_AreaB,
        atoms_AreaC, atoms_AreaD;
    [Tooltip("工作區內該有的原子")]
    [SerializeField] private List<Atom> atomsAnswer_AreaA, atomsAnswer_AreaB,
        atomsAnswer_AreaC, atomsAnswer_AreaD;
    [Tooltip("工作區原子數量文字")]
    [SerializeField] private Text countText_workC, countText_workO2, countText_workN2,
    countText_workH, countText_workFe;

    private int count_C = 0;
    private int count_O2 = 0;
    private int count_N2 = 0;
    private int count_H = 0;
    private int count_Fe = 0;

    [Header("UI")]
    [Tooltip("回答正確頁面")]
    [SerializeField] private GameObject correctPage;
    [Tooltip("回答錯誤頁面")]
    [SerializeField] private GameObject wrongPage;

    private GameManager gm;
    private LevelObjManager levelObjManager;
    private QuestionManager questionManager;    //管理題目介面
    private AudioManager audioManager;          //音樂管理
    private DataManager_Stage3 myData;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        levelObjManager = FindObjectOfType<LevelObjManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        audioManager = FindObjectOfType<AudioManager>();
        myData = FindObjectOfType<DataManager_Stage3>();
        SwitchToShop();
    }

    //買原子
    public void Buy(string atomName)
    {
        Atom result = atoms_Sell.Find(atom => atom.name == atomName);
        if (result!=null)
        {
            int cost = result.cost;
            if (myData.money >= cost)
            {
                myData.money -= cost;
                myData.atoms_Buy.Add(result);
                print("已購買：" + atomName);
            }
            else
            {
                print("金額不足");
            }
        }
        else
        {
            print(atomName + " 原子不存在");
        }
        CheckMyAtom();
    }
    //賣原子
    public void Sell(string atomName)
    {
        Atom result = myData.atoms_Buy.Find(atom => atom.name == atomName);
        if (result != null)
        {
            myData.money += result.cost;
            myData.atoms_Buy.Remove(result);
            print("已賣出：" + atomName);
        }
        else
        {
            print("你沒有" + atomName + "可以賣出");
        }
        CheckMyAtom();
    }

    //切換到商店
    public void SwitchToShop()
    {
        shop.SetActive(true);
        workSpace.SetActive(false);
        correctPage.SetActive(false);
        wrongPage.SetActive(false);
        CheckMyAtom();
    }
    //切換到工作區
    public void SwitchToWorkSpace()
    {
        workSpace.SetActive(false);
        atoms_AreaA = new List<Atom>();
        atoms_AreaB = new List<Atom>();
        atoms_AreaC = new List<Atom>();
        atoms_AreaD = new List<Atom>();
        shop.SetActive(false);
        workSpace.SetActive(true);
        correctPage.SetActive(false);
        wrongPage.SetActive(false);
        SpawnAtom();

        switch (gm.currLevel)
        {
            case 0:
                audioManager.PlayVoice("T3_1_1");
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                audioManager.PlayVoice("E3_1_1");
                break;
        }
    }

    //確認原子數量
    private void CheckMyAtom()
    {
        count_C = 0;
        count_O2 = 0;
        count_N2 = 0;
        count_H = 0;
        count_Fe = 0;
        foreach (Atom atom in myData.atoms_Buy)
        {
            switch (atom.name)
            {
                case "C":
                    count_C++;
                    break;
                case "O2":
                    count_O2++;
                    break;
                case "N2":
                    count_N2++;
                    break;
                case "H":
                    count_H++;
                    break;
                case "Fe":
                    count_Fe++;
                    break;
            }
        }
        countText_shopC.text = count_C.ToString();
        countText_shopO2.text = count_O2.ToString();
        countText_shopN2.text = count_N2.ToString();
        countText_shopH.text = count_H.ToString();
        countText_shopFe.text = count_Fe.ToString();
        myMoneyText.text = myData.money.ToString();

        countText_workC.text = count_C.ToString();
        countText_workO2.text = count_O2.ToString();
        countText_workN2.text = count_N2.ToString();
        countText_workH.text = count_H.ToString();
        countText_workFe.text = count_Fe.ToString();
    }

    public void SpawnAtom()
    {
        CheckMyAtom();
        if (atomC == null && count_C > 0)
        {
            atomC = Instantiate(atomC_Prefab, atomC_Spawn);
            atomC.name = "C";
        }
        if (atomO2 == null && count_O2 > 0)
        {
            atomO2 = Instantiate(atomO2_Prefab, atomO2_Spawn);
            atomO2.name = "O2";
        }
        if (atomN2 == null && count_N2 > 0)
        {
            atomN2 = Instantiate(atomN2_Prefab, atomN2_Spawn);
            atomN2.name = "N2";
        }
        if (atomH == null && count_H > 0)
        {
            atomH = Instantiate(atomH_Prefab, atomH_Spawn);
            atomH.name = "H";
        }
        if (atomFe == null && count_Fe > 0)
        {
            atomFe = Instantiate(atomFe_Prefab, atomFe_Spawn);
            atomFe.name = "Fe";
        }
    }

    //使用購買的原子
    public void UseAtom(string atomName)
    {
        Atom result = myData.atoms_Buy.Find(atom => atom.name == atomName);
        if (result != null)
        {
            myData.atoms_Buy.Remove(result);
            switch (atomName)
            {
                case "C":
                    atomC = null;
                    break;
                case "O2":
                    atomO2 = null;
                    break;
                case "N2":
                    atomN2 = null;
                    break;
                case "H":
                    atomH = null;
                    break;
                case "Fe":
                    atomFe = null;
                    break;
            }
            print("已使用：" + atomName);
        }
        else
        {
            print("你沒有" + atomName + "可以使用");
        }
        SpawnAtom();
    }

    //回收原子
    public void AtomReturn(string atomName)
    {
        Atom result = atoms_Sell.Find(atom => atom.name == atomName);
        if (result != null)
        {
            myData.atoms_Buy.Add(result);
            print("已回收：" + atomName);
        }
        SpawnAtom();
    }

    public void Reaction(string area, string atomName, bool enter)
    {
        List<Atom> atoms = new List<Atom>();

        switch (area)
        {
            case "A":
                atoms = atoms_AreaA;
                break;
            case "B":
                atoms = atoms_AreaB;
                break;
            case "C":
                atoms = atoms_AreaC;
                break;
            case "D":
                atoms = atoms_AreaD;
                break;
        }

        if (enter)
        {
            Atom result = atoms_Sell.Find(atom => atom.name == atomName);
            if (result != null)
            {
                atoms.Add(result);
                print(atomName + " 已放入：" + area);
            }
        }
        else
        {
            Atom result = atoms.Find(atom => atom.name == atomName);
            if (result != null)
            {
                atoms.Remove(result);
                print("已取出：" + atomName);
            }
            else
            {
                print("你沒有" + atomName + "可以取出");
            }
        }
    }

    private bool CheakAnswer(List<Atom> list1, List<Atom> list2)
    {
        // 先檢查數量是否相同
        if (list1.Count != list2.Count) return false;

        // 使用 GroupBy 按名稱統計數量，然後比對是否相等
        var grouped1 = list1.GroupBy(a => a.name)
                            .Select(g => new { Name = g.Key, Count = g.Count() })
                            .OrderBy(g => g.Name);

        var grouped2 = list2.GroupBy(a => a.name)
                            .Select(g => new { Name = g.Key, Count = g.Count() })
                            .OrderBy(g => g.Name);

        return grouped1.SequenceEqual(grouped2);
    }

    public void OnSubmitButtonClicked()
    {
        if(CheakAnswer(atomsAnswer_AreaA, atoms_AreaA)&& CheakAnswer(atomsAnswer_AreaB, atoms_AreaB)
            && CheakAnswer(atomsAnswer_AreaC, atoms_AreaC)&& CheakAnswer(atomsAnswer_AreaD, atoms_AreaD))
        {
            correctPage.SetActive(true);
            audioManager.PlayVoice("Success");
        }
        else
        {
            wrongPage.SetActive(true);
            switch (gm.currLevel)
            {
                case 0:
                    audioManager.PlayVoice("Wrong");
                    break;
                case 1:
                    audioManager.PlayVoice("QA3_1");
                    break;
                case 2:
                    audioManager.PlayVoice("QA3_2");
                    break;
                case 3:
                    audioManager.PlayVoice("QA3_3");
                    break;
                case 4:
                    audioManager.PlayVoice("QA3_4");
                    break;
                case 5:
                    audioManager.PlayVoice("QA3_5");
                    break;
            }
            questionManager.TriggerHapticFeedback();
        }
    }
    public void OnNextLevelButtonClicked()
    {
        //回收原子
        foreach (Atom atom in atoms_AreaA)
        {
            AtomReturn(atom.name);
        }
        foreach (Atom atom in atoms_AreaB)
        {
            AtomReturn(atom.name);
        }
        foreach (Atom atom in atoms_AreaC)
        {
            AtomReturn(atom.name);
        }
        foreach (Atom atom in atoms_AreaD)
        {
            AtomReturn(atom.name);
        }
        switch (gm.currLevel)
        {
            case 0:
                levelObjManager.LevelClear(1);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
                levelObjManager.LevelClear(0);
                break;
            case 5:
                levelObjManager.LevelClear(2);
                break;
        }
    }
}
