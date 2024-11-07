using UnityEngine;
using System.Collections;
using TMPro;

public class ScaleCubeCalculate : MonoBehaviour
{
    public float lerpDuration = 1.0f;
    public TextMeshProUGUI parameterDisplayText;
    public string unit = "%";
    public int decimalPlaces = 0;

    private float scaleValueY;
    private float currentDisplayValue; 

    public void ScaleInOneDirection(Vector3 scaleFactor, Vector3 direction, TextMeshProUGUI parameterText)
    {
        parameterDisplayText = parameterText;
        direction.Normalize();
        scaleValueY = scaleFactor.y;

        if (parameterDisplayText != null)
        {
            if (float.TryParse(parameterDisplayText.text.Split(' ')[0], out float displayValue))
            {
                currentDisplayValue = displayValue;
            }
            else
            {
                currentDisplayValue = 0f;
            }
        }
        else
        {
            currentDisplayValue = 0f;
        }

        StartCoroutine(ScaleOverTime(scaleFactor, direction));
    }

    private IEnumerator ScaleOverTime(Vector3 scaleFactor, Vector3 direction)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 originalPosition = transform.position;

        Vector3 newScale = originalScale + scaleFactor;
        Vector3 offset = Vector3.Scale(scaleFactor / 2.0f, direction);

        float elapsedTime = 0;

        // 设置目标值
        float targetValue = MapScaleToDisplayValue(scaleValueY);

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;
            transform.localScale = Vector3.Lerp(originalScale, newScale, t);
            transform.position = Vector3.Lerp(originalPosition, originalPosition + offset, t);

            float displayValue = Mathf.Lerp(currentDisplayValue, targetValue, t);

            if (parameterDisplayText != null)
            {
                string format = "F" + decimalPlaces;
                parameterDisplayText.text = $"{displayValue.ToString(format)} {unit}";
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = newScale;
        transform.position = originalPosition + offset;

        if (parameterDisplayText != null)
        {
            string format = "F" + decimalPlaces;
            parameterDisplayText.text = $"{targetValue.ToString(format)} {unit}";
        }
    }

    private float MapScaleToDisplayValue(float scaleValue)
    {
        if (Mathf.Approximately(scaleValue, 0.1f))
        {
            return 0.825f;
        }

        if (Mathf.Approximately(scaleValue, 0.05f))
        {
            return 0.8226f;
        }

        if (Mathf.Approximately(scaleValue, -0.05f))
        {
            return 0.515f;
        }

        if (Mathf.Approximately(scaleValue, 0.12f))
        {
            return 60f; 
        }

        if (Mathf.Approximately(scaleValue, 0.11f) || Mathf.Approximately(scaleValue,-0.01f) || Mathf.Approximately(scaleValue, 0f) || Mathf.Approximately(scaleValue, -0.15f) || Mathf.Approximately(scaleValue, -0.09f))
        {
            return 58f;
        }

        if (Mathf.Approximately(scaleValue, 0.15f))
        {
            return 440f;
        }

        if (Mathf.Approximately(scaleValue, 0.09f))
        {
            return 139f;
        }

        if (Mathf.Approximately(scaleValue, -0.02f))
        {
            return 41f;
        }
        return scaleValue;
    }
}
