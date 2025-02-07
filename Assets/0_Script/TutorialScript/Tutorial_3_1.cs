using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [Tooltip("測驗過關頁面")]
    [SerializeField] private GameObject clearPage;
    [Tooltip("測驗過關頁面文字")]
    [SerializeField] private Text clearText;

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
        clearPage.SetActive(false);
        CheckMyAtom();
    }
    //切換到工作區
    public void SwitchToWorkSpace()
    {
        workSpace.SetActive(false);
        //atoms_AreaA = new List<Atom>();
        //atoms_AreaB = new List<Atom>();
        //atoms_AreaC = new List<Atom>();
        //atoms_AreaD = new List<Atom>();
        shop.SetActive(false);
        workSpace.SetActive(true);
        correctPage.SetActive(false);
        wrongPage.SetActive(false);
        clearPage.SetActive(false);
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
        else if (atomC != null && count_C < 1)
        {
            Destroy(atomC);
        }

        if (atomO2 == null && count_O2 > 0)
        {
            atomO2 = Instantiate(atomO2_Prefab, atomO2_Spawn);
            atomO2.name = "O2";
        }
        else if (atomO2 != null && count_O2 < 1)
        {
            Destroy(atomO2);
        }

        if (atomN2 == null && count_N2 > 0)
        {
            atomN2 = Instantiate(atomN2_Prefab, atomN2_Spawn);
            atomN2.name = "N2";
        }
        else if (atomN2 != null && count_N2 < 1)
        {
            Destroy(atomN2);
        }

        if (atomH == null && count_H > 0)
        {
            atomH = Instantiate(atomH_Prefab, atomH_Spawn);
            atomH.name = "H";
        }
        else if (atomH != null && count_H < 1)
        {
            Destroy(atomH);
        }

        if (atomFe == null && count_Fe > 0)
        {
            atomFe = Instantiate(atomFe_Prefab, atomFe_Spawn);
            atomFe.name = "Fe";
        }
        else if (atomFe!= null && count_Fe < 1)
        {
            Destroy(atomFe);
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


    //抓取物件時觸發
    public void Grab(GameObject obj)
    {
        AtomBall atom = obj.GetComponent<AtomBall>();
        if (atom)
        {
            atom.Grab(true);
        }
    }

    //鬆開物件時觸發
    public void Release(GameObject obj)
    {
        AtomBall atom = obj.GetComponent<AtomBall>();
        if (atom)
        {
            atom.Grab(false); 
        }
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
        //if (list1.Count != list2.Count) return false;

        // 統計 list1 內每種名稱的數量
        var list1Count = list1.GroupBy(a => a.name)
                              .ToDictionary(g => g.Key, g => g.Count());

        // 統計 list2 內每種名稱的數量
        var list2Count = list2.GroupBy(a => a.name)
                              .ToDictionary(g => g.Key, g => g.Count());

        // 檢查 list2 是否至少包含 list1 需要的種類與數量
        foreach (var kvp in list1Count)
        {
            string atomName = kvp.Key;
            int requiredCount = kvp.Value;

            if (!list2Count.TryGetValue(atomName, out int availableCount) || availableCount < requiredCount)
            {
                return false; // 如果某種類在 list2 不足，直接回傳 false
            }
        }

        return true; // list2 至少有 list1 需要的所有種類與數量
    }

    //回收區塊內多餘的原子
    private void ExtraAtomsReturn(List<Atom> list1, List<Atom> list2)
    {
        // 統計 list1 內每種名稱的數量
        var list1Count = list1.GroupBy(a => a.name)
                              .ToDictionary(g => g.Key, g => g.Count());

        // 統計 list2 內每種名稱的數量
        var list2Count = list2.GroupBy(a => a.name)
                              .ToDictionary(g => g.Key, g => g.Count());

        // 計算 list2 多出來的部分
        List<Atom> extraAtoms = new List<Atom>();

        foreach (var kvp in list2Count)
        {
            string atomName = kvp.Key;
            int availableCount = kvp.Value;
            int requiredCount = list1Count.ContainsKey(atomName) ? list1Count[atomName] : 0;

            int extraCount = availableCount - requiredCount;

            if (extraCount > 0)
            {
                // 加入多出來的 Atom
                for (int i = 0; i < extraCount; i++)
                {
                    AtomReturn(atomName);
                }
            }
        }
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
        ExtraAtomsReturn(atomsAnswer_AreaA, atoms_AreaA);
        ExtraAtomsReturn(atomsAnswer_AreaB, atoms_AreaB);
        ExtraAtomsReturn(atomsAnswer_AreaC, atoms_AreaC);
        ExtraAtomsReturn(atomsAnswer_AreaD, atoms_AreaD);

        switch (gm.currLevel)
        {
            case 0:
                levelObjManager.LevelClear(1);
                myData.Reset();
                break;
            case 1:
            case 2:
            case 3:
            case 4:
                levelObjManager.LevelClear(0);
                break;
            case 5:
                clearPage.SetActive(true);
                clearText.text = "恭喜你已經全部答對了，\n你還剩餘" + myData.money + "枚金幣，\n真是太厲害了！";
                break;
        }
    }
    public void OnClearButtonClicked()
    {
        levelObjManager.LevelClear(2);
    }
}
