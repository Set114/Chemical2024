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
    [Tooltip("商店賣的原子")]
    [SerializeField] private List<Atom> atoms_Sell;
    [Tooltip("自己買的原子")]
    [SerializeField] private List<Atom> atoms_Buy;
    [Tooltip("經費")]
    [SerializeField] private int money = 0;


    [Header("UI")]
    [Tooltip("經費文字")]
    [SerializeField] private Text myMoneyText;
    [Tooltip("原子數量文字")]
    [SerializeField] private Text countText_C, countText_O2, countText_N2,
        countText_H, countText_Fe;

    // Start is called before the first frame update
    void Start()
    {
        CheckMyAtom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //買原子
    public void Buy(string atomName)
    {
        Atom result = atoms_Sell.Find(atom => atom.name == atomName);
        if (result!=null)
        {
            int cost = result.cost;
            if (money >= cost)
            {
                money -= cost;
                atoms_Buy.Add(result);
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
        Atom result = atoms_Buy.Find(atom => atom.name == atomName);
        if (result != null)
        {
            money += result.cost;
            atoms_Buy.Remove(result);
            print("已賣出：" + atomName);
        }
        else
        {
            print("你沒有" + atomName + "可以賣出");
        }
        CheckMyAtom();
    }
    private void CheckMyAtom()
    {
        int count_C = 0;
        int count_O2 = 0;
        int count_N2 = 0;
        int count_H = 0;
        int count_Fe = 0;
        foreach (Atom atom in atoms_Buy)
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
        countText_C.text = count_C.ToString();
        countText_O2.text = count_O2.ToString();
        countText_N2.text = count_N2.ToString();
        countText_H.text = count_H.ToString();
        countText_Fe.text = count_Fe.ToString();

        myMoneyText.text = money.ToString();
    }
}
