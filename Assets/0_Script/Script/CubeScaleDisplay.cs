using UnityEngine;
using TMPro;

public class CubeScaleDisplay : MonoBehaviour
{
    public TMP_Text scaleText; 
    private const float referenceScaleY = 0.2f;

    void Start()
    {
        if (scaleText == null)
        {
            Debug.LogError("ScaleText is not assigned.");
            return;
        }       
    }

    void Update()
    {
        float scaleYPercentage = (transform.localScale.y / referenceScaleY ) * 100;
        int roundedScaleYPercentage = Mathf.RoundToInt(scaleYPercentage);
        scaleText.text = $"{roundedScaleYPercentage}%";
    }
}
