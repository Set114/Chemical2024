using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4_1 : MonoBehaviour
{
    [Header("O2的Prefab")]
    public GameObject O2, O2PrefabUI,CO2, CO2PrefabUI;
    [Header("所有生成的O2的Prefab")]
    public List<GameObject> TotalO2;
    [Header("所有生成的O2的PrefabUI")]
    public List<GameObject> TotalO2UI;
    [Header("所有生成的CO2的Obj")]
    public List<GameObject> TotalCO2;
    [Header("所有生成的CO2的PrefabUI")]
    public List<GameObject> TotalCO2UI;
    [Header("UI 父物件")]
    public Transform O2PrefabParent;
    [Header("隔板")]
    public Animator PartitionObj;
    public bool ClickPartitionObj;
    [Header("所有C")]
    public GameObject[] CObject;
    [Header("所有C")]
    public GameObject[] CUIObject;
    //加熱器
    [Header("加熱器的溫度文字")]
    public GameObject HeaterText;
    public bool ClickHeaterObj;
    [Header("粒子移動速度")]
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            BreatherValve();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            AirExtractionValve();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReButton();
        }
        if (ClickPartitionObj && ClickHeaterObj) {
            float step = Speed * Time.deltaTime;
                for (int i = 0; i < TotalO2.Count; i++)
                {
                    TotalO2[i].transform.position = Vector3.MoveTowards(TotalO2[i].transform.position, CObject[i].transform.position, step);
                    TotalO2UI[i].transform.position = Vector3.MoveTowards(TotalO2UI[i].transform.position, CUIObject[i].transform.position, step);
                    if (TotalO2UI[i].transform.position == CUIObject[i].transform.position) {
                    if (TotalO2UI[i].active)
                    {
                        GameObject CO2UIPrefab = Instantiate(CO2PrefabUI) as GameObject;
                        CO2UIPrefab.transform.parent = O2PrefabParent;
                        CO2UIPrefab.transform.localScale = new Vector3(1, 1, 1);
                        CO2UIPrefab.transform.localEulerAngles = new Vector3(0, -180, 0);
                        CO2UIPrefab.transform.localPosition = TotalO2UI[i].transform.localPosition;
                        CO2UIPrefab.SetActive(true);

                        TotalO2UI[i].SetActive(false);
                        CUIObject[i].SetActive(false);
                        TotalCO2UI.Add(CO2UIPrefab);
                    }
                }
            }
            
        }
    }
    //通氣閥
    public void BreatherValve()
    {
        if (TotalO2.Count < 9)
        {
            GameObject O2Prefab = Instantiate(O2, new Vector3(Random.Range(0.085f, 0.185f), Random.Range(0.58f, 0.68f), 1.092661f), transform.rotation) as GameObject;
            TotalO2.Add(O2Prefab);
            GameObject O2UIPrefab = Instantiate(O2PrefabUI) as GameObject;
            O2UIPrefab.transform.parent= O2PrefabParent;
            O2UIPrefab.transform.localScale = new Vector3(1, 1, 1);
            O2UIPrefab.transform.localEulerAngles = new Vector3(0, -180, 0);
            O2UIPrefab.transform.localPosition = new Vector3(Random.Range(0,700), Random.Range(0, 400), 0);

            O2UIPrefab.SetActive(true);
            TotalO2UI.Add(O2UIPrefab);
        }
    }
    //抽氣閥
    public void AirExtractionValve()
    {
        if (TotalO2.Count > 0)
        {
            Destroy(TotalO2[TotalO2.Count - 1]);
            TotalO2.Remove(TotalO2[TotalO2.Count - 1]);
            Destroy(TotalO2UI[TotalO2UI.Count - 1]);
            TotalO2UI.Remove(TotalO2UI[TotalO2UI.Count - 1]);
        }
    }
    //隔板
    public void Partition()
    {

        PartitionObj.SetBool("Click", ClickPartitionObj = !ClickPartitionObj);


    }
    //加熱器
    public void Heater()
    {
        HeaterText.SetActive(ClickHeaterObj = !ClickHeaterObj);
        if (ClickHeaterObj)
        {
            FindObjectOfType<NumberJump>().StartAddTemp();
        }
    }

    public void ReButton() {
        for (int i = 0; i < TotalO2.Count; i++)
        {
            Destroy(TotalO2[i]);
        }
        TotalO2.Clear();
        for (int j = 0; j < TotalO2UI.Count; j++)
        {
            Destroy(TotalO2UI[j]);
            Destroy(TotalCO2UI[j]);
        }
        TotalO2UI.Clear();
        TotalCO2UI.Clear();
        for (int k = 0; k < TotalCO2.Count; k++)
        {
            Destroy(TotalCO2[k]);
        }
        TotalCO2.Clear();

        for (int l = 0; l < CObject.Length; l++)
        {
            CObject[l].SetActive(true);
            CUIObject[l].SetActive(true);
        }
        ClickPartitionObj = false;
        PartitionObj.SetBool("Click", ClickPartitionObj);

    }
    public void CreateCO2(Vector3 pos) {
        GameObject CO2Prefab = Instantiate(CO2, pos, transform.rotation) as GameObject;
        CO2Prefab.SetActive(true);
        TotalCO2.Add(CO2Prefab);

    }
}
