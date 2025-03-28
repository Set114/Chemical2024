using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class AtomDetection : MonoBehaviour
{
    [SerializeField] private Area area;
    private enum Area
    {
        A, B, C, D
    }
    [SerializeField] private List<AtomBall> atoms = new List<AtomBall>();
    [SerializeField] private float moveSpeed = 1f;              // 物件移動速度
    [SerializeField] private float mergeDistance = 0.1f;        // 物件合併的最小距離
    [SerializeField] private GameObject molecularPrefab;


    private Tutorial_3_1 tutorialObject;

    private void Start()
    {
        tutorialObject = FindObjectOfType<Tutorial_3_1>();
    }

    private void OnTriggerStay(Collider other)
    {
        AtomBall atom = other.GetComponent<AtomBall>();
        if (atom)
        {
            if (!atom.isGrabbing)
            {
                if (atom.isUsing)
                {
                    // 當有兩個以上的相同物件進入
                    if (atoms.Count >= 2)
                    {
                        AtomBall atomA, atomB;
                        if (FindClosestPair(out atomA, out atomB))
                        {
                            // 讓兩個物件逐漸靠近
                            atomA.transform.position = Vector3.MoveTowards(atomA.transform.position, atomB.transform.position, moveSpeed * Time.deltaTime);
                            atomB.transform.position = Vector3.MoveTowards(atomB.transform.position, atomA.transform.position, moveSpeed * Time.deltaTime);

                            // 如果距離小於指定值，進行合併
                            if (Vector3.Distance(atomA.transform.position, atomB.transform.position) < mergeDistance)
                            {
                                atomA.SetAtomCount(atomA.count + atomB.count);
                                atomB.SetAtomCount(0);
                                atoms.Remove(atomB);
                                Destroy(atomB.gameObject);
                            }
                        }
                    }
                }
                else
                {
                    atom.isUsing = true;
                    atoms.Add(atom);
                    tutorialObject.Reaction(area.ToString(), other.gameObject.name, true, atom.count);
                    other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        AtomBall atom = other.GetComponent<AtomBall>();
        if (atom && atom.isUsing)
        {
            tutorialObject.Reaction(area.ToString(), atom.gameObject.name, false, atom.count);
            //atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            atom.isUsing = false;
            atoms.Remove(atom);
        }
    }

    /// <summary>
    /// 找出距離最近且相同名字的一對物件
    /// </summary>
    public bool FindClosestPair(out AtomBall atomA, out AtomBall atomB)
    {
        atomA = null;
        atomB = null;
        float minDistance = float.MaxValue;

        // 用字典分類相同名稱的物件
        Dictionary<string, List<AtomBall>> groupedObjects = new Dictionary<string, List<AtomBall>>();

        foreach (var atom in atoms)
        {
            if (!groupedObjects.ContainsKey(atom.gameObject.name))
            {
                groupedObjects[atom.gameObject.name] = new List<AtomBall>();
            }
            groupedObjects[atom.gameObject.name].Add(atom);
        }

        // 遍歷每一組相同名稱的物件
        foreach (var group in groupedObjects.Values)
        {
            for (int i = 0; i < group.Count; i++)
            {
                for (int j = i + 1; j < group.Count; j++)
                {
                    float distance = Vector3.Distance(group[i].transform.position, group[j].transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        atomA = group[i];
                        atomB = group[j];
                    }
                }
            }
        }
        return atomA != null && atomB != null;
    }
}