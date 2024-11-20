using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentWater : MonoBehaviour
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
    void touchValve()
    { 

      Watermaterial = this.gameObject.GetComponent<Renderer>().material;
      
      Watermaterial.SetColor("_SpecColor", Color.yellow);
      Watermaterial.SetColor("_TopColor", Color.yellow);
      Watermaterial.SetColor("_BottomColor", Color.yellow);
      Watermaterial.SetColor("_EdgeColor", Color.yellow);
      Watermaterial.SetColor("_GlowColor", Color.yellow);
  
    }
    
}
