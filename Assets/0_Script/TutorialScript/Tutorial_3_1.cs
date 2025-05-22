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
    [SerializeField] private Text countText_shopC, countText_shopO, countText_shopN,
        countText_shopH, countText_shopFe;
    [Header("工作區")]
    [Tooltip("組合原子的工作區")]
    [SerializeField] private GameObject workSpace;
    [Tooltip("原子Prefab")]
    [SerializeField] private GameObject atomC_Prefab, atomO_Prefab, atomN_Prefab,
        atomH_Prefab, atomFe_Prefab;
    [Tooltip("生成出的原子")]
    private GameObject atomC, atomO, atomN, atomH, atomFe;
    [Tooltip("原子生成點")]
    [SerializeField] private Transform atomC_Spawn, atomO_Spawn, atomN_Spawn,
    atomH_Spawn, atomFe_Spawn;

    [Tooltip("工作區內存在的原子")]
    [SerializeField] private List<Atom> atoms_AreaA, atoms_AreaB,
        atoms_AreaC, atoms_AreaD;
    [Tooltip("工作區內該有的原子")]
    [SerializeField] private List<Atom> atomsAnswer_AreaA, atomsAnswer_AreaB,
        atomsAnswer_AreaC, atomsAnswer_AreaD;
    [Tooltip("工作區原子數量文字")]
    [SerializeField] private Text countText_workC, countText_workO, countText_workN,
    countText_workH, countText_workFe;
    [Space]
    //等號右側相關設定
    [Tooltip("指定成品原子")]
    [SerializeField] private List<Atom> areaCAtomSample, areaDAtomSample;
    [Tooltip("指定成品原子Prefab")]
    [SerializeField] private GameObject areaCAtomSamplePrefab, areaDAtomSamplePrefab;
    [Tooltip("指定成品原子物件")]
    [SerializeField] private List<GameObject> areaCAtomObjs, areaDAtomObjs;
    [Tooltip("成品區域")]
    [SerializeField] private Transform areaC, areaD;
    [Tooltip("生成範圍最小、最大點")]
    [SerializeField] private Vector3 spawnAreaCMin, spawnAreaCMax;
    [Tooltip("生成範圍最小、最大點")]
    [SerializeField] private Vector3 spawnAreaDMin, spawnAreaDMax;
    [Tooltip("成品數量文字")]
    [SerializeField] private Text areaC_CountText, areaD_CountText;

    private int count_C = 0;
    private int count_O = 0;
    private int count_N = 0;
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
    [Space]
    [Tooltip("需要解釋氧為何不能只放單顆氧原子")]
    [SerializeField] private bool needExplainO2 = false;
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
        if (result != null)
        {
            myData.money -= result.cost;
            myData.atoms_Buy.Add(result);
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
        count_O = 0;
        count_N = 0;
        count_H = 0;
        count_Fe = 0;
        foreach (Atom atom in myData.atoms_Buy)
        {
            switch (atom.name)
            {
                case "C":
                    count_C++;
                    break;
                case "O":
                    count_O++;
                    break;
                case "N":
                    count_N++;
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
        countText_shopO.text = count_O.ToString();
        countText_shopN.text = count_N.ToString();
        countText_shopH.text = count_H.ToString();
        countText_shopFe.text = count_Fe.ToString();
        myMoneyText.text = myData.money.ToString();

        countText_workC.text = count_C.ToString();
        countText_workO.text = count_O.ToString();
        countText_workN.text = count_N.ToString();
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

        if (atomO == null && count_O > 0)
        {
            atomO = Instantiate(atomO_Prefab, atomO_Spawn);
            atomO.name = "O";
        }
        else if (atomO != null && count_O < 1)
        {
            Destroy(atomO);
        }

        if (atomN == null && count_N > 0)
        {
            atomN = Instantiate(atomN_Prefab, atomN_Spawn);
            atomN.name = "N";
        }
        else if (atomN != null && count_N < 1)
        {
            Destroy(atomN);
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
        else if (atomFe != null && count_Fe < 1)
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
                case "O":
                    atomO = null;
                    break;
                case "N":
                    atomN = null;
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
        else
        {
            switch (atomName)
            {
                case "N2":
                    result = atoms_Sell.Find(atom => atom.name == "N");
                    myData.atoms_Buy.Add(result);
                    myData.atoms_Buy.Add(result);
                    break;
                case "O2":
                    result = atoms_Sell.Find(atom => atom.name == "O");
                    myData.atoms_Buy.Add(result);
                    myData.atoms_Buy.Add(result);
                    break;
                case "H2":
                    result = atoms_Sell.Find(atom => atom.name == "H");
                    myData.atoms_Buy.Add(result);
                    myData.atoms_Buy.Add(result);
                    break;
                case "Fe2O3":
                    result = atoms_Sell.Find(atom => atom.name == "Fe");
                    myData.atoms_Buy.Add(result);
                    myData.atoms_Buy.Add(result);
                    result = atoms_Sell.Find(atom => atom.name == "O");
                    myData.atoms_Buy.Add(result);
                    myData.atoms_Buy.Add(result);
                    myData.atoms_Buy.Add(result);
                    break;
                case "CO":
                    result = atoms_Sell.Find(atom => atom.name == "C");
                    myData.atoms_Buy.Add(result);
                    result = atoms_Sell.Find(atom => atom.name == "O");
                    myData.atoms_Buy.Add(result);
                    break;
            }
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
                if (atomName == "O" && needExplainO2)
                {
                    audioManager.PlayVoice("W_O2");
                    needExplainO2 = false;
                }
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
            else
            {
                switch (atomName)
                {
                    case "N2":
                        result = atoms_Sell.Find(atom => atom.name == "N");
                        atoms.Add(result);
                        atoms.Add(result);
                        break;
                    case "O2":
                        result = atoms_Sell.Find(atom => atom.name == "O");
                        atoms.Add(result);
                        atoms.Add(result);
                        break;
                    case "H2":
                        result = atoms_Sell.Find(atom => atom.name == "H");
                        atoms.Add(result);
                        atoms.Add(result);
                        break;
                    case "Fe2O3":
                        result = atoms_Sell.Find(atom => atom.name == "Fe");
                        atoms.Add(result);
                        atoms.Add(result);
                        result = atoms_Sell.Find(atom => atom.name == "O");
                        atoms.Add(result);
                        atoms.Add(result);
                        atoms.Add(result);
                        break;
                    case "CO":
                        result = atoms_Sell.Find(atom => atom.name == "C");
                        atoms.Add(result);
                        result = atoms_Sell.Find(atom => atom.name == "O");
                        atoms.Add(result);
                        break;
                }
                print(atomName + " 已放入：" + area);
                if (result == null)
                    print("你沒有" + atomName + "可以取出"); ;
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
                switch (atomName)
                {
                    case "N2":
                        result = atoms_Sell.Find(atom => atom.name == "N");
                        atoms.Remove(result);
                        atoms.Remove(result);
                        break;
                    case "O2":
                        result = atoms_Sell.Find(atom => atom.name == "O");
                        atoms.Remove(result);
                        atoms.Remove(result);
                        break;
                    case "H2":
                        result = atoms_Sell.Find(atom => atom.name == "H");
                        atoms.Remove(result);
                        atoms.Remove(result);
                        break;
                    case "Fe2O3":
                        result = atoms_Sell.Find(atom => atom.name == "Fe");
                        atoms.Remove(result);
                        atoms.Remove(result);
                        result = atoms_Sell.Find(atom => atom.name == "O");
                        atoms.Remove(result);
                        atoms.Remove(result);
                        atoms.Remove(result);
                        break;
                    case "CO":
                        result = atoms_Sell.Find(atom => atom.name == "C");
                        atoms.Remove(result);
                        result = atoms_Sell.Find(atom => atom.name == "O");
                        atoms.Remove(result);
                        break;
                }
                print("已取出：" + atomName);
                if (result == null)
                    print("你沒有" + atomName + "可以取出"); ;
            }
        }      
    }

    //標準答案跟玩家提交的答案做對比
    private bool CheckAnswer(List<Atom> list1, List<Atom> list2)
    {
        // 數量不同，直接 false
        if (list1.Count != list2.Count) return false;

        // 用名稱統計兩邊出現次數
        var list1Count = list1.GroupBy(a => a.name)
                              .ToDictionary(g => g.Key, g => g.Count());

        var list2Count = list2.GroupBy(a => a.name)
                              .ToDictionary(g => g.Key, g => g.Count());

        // 用 SequenceEqual 檢查兩邊的名稱與數量完全一致
        return list1Count.OrderBy(kv => kv.Key)
                         .SequenceEqual(list2Count.OrderBy(kv => kv.Key));
    }

    /*回收區塊內多餘的原子
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
    */
    //變更成品的原子數量
    public void OnAreaCDButtonClicked(int index)
    {
        Vector3 spawnPosition;
        GameObject atomObj;
        switch (index)
        {
            case -1:    //C-
                foreach (Atom atom in areaCAtomSample)
                {
                    atoms_AreaC.Remove(atom);
                }

                if (areaCAtomObjs.Count > 0)
                {
                    atomObj = areaCAtomObjs[areaCAtomObjs.Count - 1]; // 取得最後一個物件
                    areaCAtomObjs.RemoveAt(areaCAtomObjs.Count - 1); // 從 List 移除
                    Destroy(atomObj); // 銷毀遊戲物件
                }
                areaC_CountText.text = areaCAtomObjs.Count.ToString();
                break;
            case -2:    //D-
                foreach (Atom atom in areaDAtomSample)
                {
                    atoms_AreaD.Remove(atom);
                }

                if (areaDAtomObjs.Count > 0)
                {
                    atomObj = areaDAtomObjs[areaDAtomObjs.Count - 1]; // 取得最後一個物件
                    areaDAtomObjs.RemoveAt(areaDAtomObjs.Count - 1); // 從 List 移除
                    Destroy(atomObj); // 銷毀遊戲物件
                }
                areaD_CountText.text = areaDAtomObjs.Count.ToString();
                break;
            case 1:    //C+
                foreach (Atom atom in areaCAtomSample)
                {
                    atoms_AreaC.Add(atom);
                }
                spawnPosition = new Vector3(
                    UnityEngine.Random.Range(spawnAreaCMin.x, spawnAreaCMax.x),
                    UnityEngine.Random.Range(spawnAreaCMin.y, spawnAreaCMax.y),
                    UnityEngine.Random.Range(spawnAreaCMin.z, spawnAreaCMax.z));      

                atomObj = Instantiate(areaCAtomSamplePrefab);
                atomObj.transform.SetParent(areaC);
                atomObj.transform.localPosition = spawnPosition;
                areaCAtomObjs.Add(atomObj);
                areaC_CountText.text = areaCAtomObjs.Count.ToString();
                break;
            case 2:    //D+
                foreach (Atom atom in areaDAtomSample)
                {
                    atoms_AreaD.Add(atom);
                }
                spawnPosition = new Vector3(
                    UnityEngine.Random.Range(spawnAreaDMin.x, spawnAreaDMax.x),
                    UnityEngine.Random.Range(spawnAreaDMin.y, spawnAreaDMax.y),
                    UnityEngine.Random.Range(spawnAreaDMin.z, spawnAreaDMax.z));

                atomObj = Instantiate(areaDAtomSamplePrefab);
                atomObj.transform.SetParent(areaD);
                atomObj.transform.localPosition = spawnPosition;
                areaDAtomObjs.Add(atomObj);
                areaD_CountText.text = areaDAtomObjs.Count.ToString();
                break;
        }
    }

    public void OnSubmitButtonClicked()
    {
        if (CheckAnswer(atomsAnswer_AreaA, atoms_AreaA) && CheckAnswer(atomsAnswer_AreaB, atoms_AreaB)
            && CheckAnswer(atomsAnswer_AreaC, atoms_AreaC) && CheckAnswer(atomsAnswer_AreaD, atoms_AreaD))
        {
            correctPage.SetActive(true);
            workSpace.SetActive(false);
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
            gm.GetMistake();
        }
        //移除提交的原子球
        AtomBall[] atoms = FindObjectsOfType<AtomBall>();
        foreach(AtomBall atom in atoms)
        {
            GameObject atomObj = atom.gameObject;
            if (atomObj != atomC && atomObj != atomO && atomObj != atomN
                && atomObj != atomH && atomObj != atomFe)
            {
                Atom result = myData.atoms_Buy.Find(atom => atom.name == atomObj.name);
                if (result != null)
                {
                    //myData.atoms_Buy.Remove(result);
                }
                Destroy(atomObj);
            }
        }

        AtomDetection[] atomDetections = FindObjectsOfType<AtomDetection>();
        foreach (AtomDetection atomDetection in atomDetections)
        {
            atomDetection.RemoveAllAtom();
        }
        atoms_AreaA = new List<Atom>();
        atoms_AreaB = new List<Atom>();
    }
    public void OnNextLevelButtonClicked()
    {
        /*回收原子
        ExtraAtomsReturn(atomsAnswer_AreaA, atoms_AreaA);
        ExtraAtomsReturn(atomsAnswer_AreaB, atoms_AreaB);
        ExtraAtomsReturn(atomsAnswer_AreaC, atoms_AreaC);
        ExtraAtomsReturn(atomsAnswer_AreaD, atoms_AreaD);
        */
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
