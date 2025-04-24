using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddErrorCount : MonoBehaviour
{
    private void OnEnable()
    {
        Error();
    }
    public void Error() {
        FindObjectOfType<Shine_GM>().ErrorNumber4_1++;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
