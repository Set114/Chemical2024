using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Material Watermaterial;
    public GameObject[] ChemicalImage;
    public Collider PotassiumCollider, AcidCollider;
    public GameObject[] InfoUI;

    public GameObject[] TackObjs;
    public Vector3[] RecordObjs;
    public GameObject Final;
    // Start is called before the first frame update
    void Start()
    {
        RecordObjs[0] = TackObjs[0].transform.position;
        RecordObjs[1] = TackObjs[1].transform.position;
        RecordObjs[2] = TackObjs[2].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReButton() {
        TackObjs[0].transform.position = RecordObjs[0];
        TackObjs[1].transform.position = RecordObjs[1];
        TackObjs[2].transform.position = RecordObjs[2];

        TackObjs[0].SetActive(true);
        TackObjs[1].SetActive(true);
        TackObjs[2].SetActive(true);

        ChemicalImage[0].SetActive(false);
        ChemicalImage[1].SetActive(false);
        ChemicalImage[2].SetActive(false);

        InfoUI[0].SetActive(false);
        InfoUI[1].SetActive(false);
        TackObjs[0].GetComponent<Collider>().enabled = true;
        TackObjs[1].GetComponent<Collider>().enabled = false;
        TackObjs[2].GetComponent<Collider>().enabled = false;

        TackObjs[1].transform.localEulerAngles = Vector3.zero;
        TackObjs[2].transform.localEulerAngles = Vector3.zero;

       /* Watermaterial.SetColor("_SpecColor", Color.white);
        Watermaterial.SetColor("_TopColor", Color.white);
        Watermaterial.SetColor("_BottomColor", Color.white);
        Watermaterial.SetColor("_EdgeColor", Color.white);
        Watermaterial.SetColor("_GlowColor", Color.white);*/
        Watermaterial.SetColor("_Color", new Color32(191, 234, 255, 64));

        TackObjs[0].GetComponent<Shine_MouseController>().Reset();
        TackObjs[1].GetComponent<Shine_MouseController>().Reset();
        TackObjs[2].GetComponent<Shine_MouseController>().Reset();

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        Debug.Log("layer"+other.gameObject.layer);
        if (other.gameObject.name == "yellowObj")
        {
            Watermaterial = this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
            other.gameObject.SetActive(false);
            /*Watermaterial.SetColor("_SpecColor", Color.yellow);
            Watermaterial.SetColor("_TopColor", Color.yellow);
            Watermaterial.SetColor("_BottomColor", Color.yellow);
            Watermaterial.SetColor("_EdgeColor", Color.yellow);
            Watermaterial.SetColor("_GlowColor", Color.yellow);*/
            Watermaterial.SetColor("_Color", new Color32(255, 235, 4, 64));

            ChemicalImage[0].SetActive(true);
            PotassiumCollider.enabled = true;
            AcidCollider.enabled = true;
            InfoUI[0].SetActive(true);

        }
        if (other.gameObject.name == "Acid")
        {
           // Material Auid= this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
            other.gameObject.SetActive(false);
            /*Auid.SetColor("_SpecColor", new Color32(255,100,0,255));
            Auid.SetColor("_TopColor", new Color32(255, 100, 0, 255));
            Auid.SetColor("_BottomColor", new Color32(255, 100, 0, 255));
            Auid.SetColor("_EdgeColor", new Color32(255,100,0,255));
            Auid.SetColor("_GlowColor", new Color32(255, 100, 0, 255));*/
            Watermaterial.SetColor("_Color", new Color32(255, 100, 0, 64));

            ChemicalImage[1].SetActive(true);
            ChemicalImage[0].SetActive(false);
            ChemicalImage[2].SetActive(false);

        }
        if (other.gameObject.name == "Potassium")
        {
            //Material pota = this.gameObject.GetComponent<Renderer>().material;
            other.gameObject.SetActive(false);
           /* pota.SetColor("_SpecColor", Color.yellow);
            pota.SetColor("_TopColor", Color.yellow);
            pota.SetColor("_BottomColor", Color.yellow);
            pota.SetColor("_EdgeColor", Color.yellow);
            pota.SetColor("_GlowColor", Color.yellow);*/
            ChemicalImage[1].SetActive(false);
            ChemicalImage[0].SetActive(false);
            ChemicalImage[2].SetActive(true);
            InfoUI[0].SetActive(false);
            InfoUI[1].SetActive(true);
            StartCoroutine(FinalCheck());
        }
    }

    IEnumerator FinalCheck() {
        yield return new WaitForSeconds(6f);
        if (!TackObjs[0].active && !TackObjs[1].active && !TackObjs[2].active) {
            Final.SetActive(true);
        }
    }
    
}
