using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class AtomDetection : MonoBehaviour
{
    [SerializeField] private Area area;
    private enum Area
    {
        A, B, C, D
    }
    [Tooltip("目標的原子")]
    [SerializeField] private List<string> atoms_Target;
    [Tooltip("合成後的Prefab")]
    [SerializeField] private GameObject resultPrefab;
    [Tooltip("區域內的原子")]
    [SerializeField] private List<GameObject> allAtoms;

    [SerializeField] private float combineDistance = 0.5f;
    [SerializeField] private float moveSpeed = 1f;

    private Tutorial_3_1 tutorialObject;

    private void Start()
    {
        tutorialObject = FindObjectOfType<Tutorial_3_1>();
        allAtoms = new List<GameObject>();
    }

    private void OnTriggerStay(Collider other)
    {
        AtomBall atom = other.GetComponent<AtomBall>();
        if (atom)
        {
            if (!atom.isGrabbing)
            {
                if (!atom.isUsing)
                {
                    allAtoms.Add(atom.gameObject);
                    TryCombine();
                    atom.isUsing = true;
                    tutorialObject.Reaction(area.ToString(), other.gameObject.name, true);
                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        AtomBall atom = other.GetComponent<AtomBall>();
        if (atom && atom.isUsing)
        {
            allAtoms.Remove(atom.gameObject);
            TryCombine();
            tutorialObject.Reaction(area.ToString(), atom.gameObject.name, false);
            //atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            atom.isUsing = false;
        }
    }

    public void RemoveAllAtom()
    {
        allAtoms = new List<GameObject>();
    }

    private void TryCombine()
    {
        if (atoms_Target.Count <= 0)
            return;

        List<GameObject> matchedObjects = FindMatchingObjects();

        if (matchedObjects != null && matchedObjects.Count == atoms_Target.Count)
        {
            StartCoroutine(MoveTogetherAndCombine(matchedObjects));
        }
        else
        {
            Debug.Log("找不到符合條件的物件");
        }
    }
    private List<GameObject> FindMatchingObjects()
    {
        // 複製一份條件名稱 List 來比對
        List<string> needed = new List<string>(atoms_Target);
        List<GameObject> matched = new List<GameObject>();

        foreach (GameObject obj in allAtoms)
        {
            string objName = obj.name.Replace("(Clone)", "").Trim();

            if (needed.Contains(objName))
            {
                matched.Add(obj);
                needed.Remove(objName);
            }

            if (needed.Count == 0)
                break;
        }

        return needed.Count == 0 ? matched : null;
    }

    private IEnumerator MoveTogetherAndCombine(List<GameObject> objectsToCombine)
    {
        Vector3 center = GetCenterPosition(objectsToCombine);
        bool moving = true;

        while (moving)
        {
            moving = false;

            foreach (GameObject obj in objectsToCombine)
            {
                float dist = Vector3.Distance(obj.transform.position, center);
                if (dist > combineDistance)
                {
                    Vector3 direction = (center - obj.transform.position).normalized;
                    obj.transform.position += direction * moveSpeed * Time.deltaTime;
                    moving = true;
                }
            }

            yield return null;
        }

        // Combine 完成
        Vector3 finalPos = GetCenterPosition(objectsToCombine);
        GameObject result = Instantiate(resultPrefab, finalPos, Quaternion.identity);
        result.name = result.name.Replace("(Clone)", "").Trim();

        result.transform.SetParent(transform);

        foreach (GameObject obj in objectsToCombine)
        {
            tutorialObject.Reaction(area.ToString(), obj.name, false);
            allAtoms.Remove(obj);
            Destroy(obj);
        }

        Debug.Log("合成完成！");
    }

    private Vector3 GetCenterPosition(List<GameObject> objs)
    {
        Vector3 sum = Vector3.zero;
        foreach (GameObject obj in objs)
        {
            sum += obj.transform.position;
        }
        return sum / objs.Count;
    }
}