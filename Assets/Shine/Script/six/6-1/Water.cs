using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Material Watermaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        Debug.Log("layer"+other.gameObject.layer);
        if (other.gameObject.tag == "yellowObj")
        {
            Debug.Log("碰到了黃色塊");
            Watermaterial = this.gameObject.GetComponent<Renderer>().material;

            Watermaterial.SetColor("_SpecColor", Color.yellow);
            Watermaterial.SetColor("_TopColor", Color.yellow);
            Watermaterial.SetColor("_BottomColor", Color.yellow);
            Watermaterial.SetColor("_EdgeColor", Color.yellow);
            Watermaterial.SetColor("_GlowColor", Color.yellow);
        }
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "acid")
        {
            Debug.Log("碰到了鹽酸");
            Material Auid= this.gameObject.GetComponent<Renderer>().material;

            Auid.SetColor("_SpecColor", new Color32(255,100,0,255));
            Auid.SetColor("_TopColor", new Color32(255, 100, 0, 255));
            Auid.SetColor("_BottomColor", new Color32(255, 100, 0, 255));
            Auid.SetColor("_EdgeColor", new Color32(255,100,0,255));
            Auid.SetColor("_GlowColor", new Color32(255, 100, 0, 255));
        }
        if (other.gameObject.tag == "potassium")
        {
            Debug.Log("碰到了鉀");
            Material pota = this.gameObject.GetComponent<Renderer>().material;

            pota.SetColor("_SpecColor", Color.yellow);
            pota.SetColor("_TopColor", Color.yellow);
            pota.SetColor("_BottomColor", Color.yellow);
            pota.SetColor("_EdgeColor", Color.yellow);
            pota.SetColor("_GlowColor", Color.yellow);
        }
    }
    
}
