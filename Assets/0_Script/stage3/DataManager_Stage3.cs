using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager_Stage3 : MonoBehaviour
{
    [Tooltip("自己買的原子")]
    [SerializeField] public List<Atom> atoms_Buy;
    [Tooltip("經費")]
    [SerializeField] public int money = 0;
}
