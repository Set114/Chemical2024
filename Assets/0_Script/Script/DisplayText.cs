using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DisplayText : MonoBehaviour
{
    public GlucoseScaleCube glucoseScaleCubeScript;
    public TMP_Text text;
    public Text temperature_text;
    public string displayText1 = "0%";
    public string displayText2 = "0%";
    public string displayText3 = "0%";
    public string displayText4 = "0%";
    public string displayText5 = "0%";
    public Vector3 scale = new Vector3(0, 0.1f, 0);
    public int levelindex;

    private bool a1 = false;
    // private AnimationController animationController;
    public SwitchUI switchUI;


    void OnEnable()
    {
        levelindex = switchUI.GetLevelCount();
        Debug.Log("levelindex" + levelindex);
        if (levelindex == 2)
        {
            if (text != null)
            {
                text.text = displayText1;
            }
        }

        //Button.onClick.AddListener(OnButtonClicked);

        // animationController = FindObjectOfType<AnimationController>();
        // if (animationController == null)
        // {
        //     Debug.LogError("AnimationController not found in the scene.");
        // }
    }

    private void Update()
    {
        if (levelindex == 2)
        {
            if (glucoseScaleCubeScript.isParticleTriggered && !a1)
            {
                StartCoroutine(AnimateText(displayText2));
                a1 = true;
            }
        }
    }

    public void ignition()
    {
        float index = 50;
        StartCoroutine(AnimateText(displayText3));
        StartCoroutine(AnimateTemperature(index));
    }

    public void flow()
    {
        StartCoroutine(AnimateText(displayText4));
        
        StartCoroutine(DelayedAction());
    }

    private IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(7.5f);
        StartCoroutine(AnimateText(displayText5));
        glucoseScaleCubeScript.SendMessage("UpdateScaleFactor", new GlucoseScaleCube.ScaleFactorParameters(scale, false));
    }

    private IEnumerator AnimateText(string targetText)
    {
        float currentValue = float.Parse(text.text.Replace("%", ""));
        float targetValue = float.Parse(targetText.Replace("%", ""));
        float duration = 4.0f; //  ʵe    ɶ  ]   ^
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newValue = Mathf.Lerp(currentValue, targetValue, elapsed / duration);
            text.text = Mathf.RoundToInt(newValue).ToString() + "%";
            yield return null;
        }

        text.text = targetValue.ToString() + "%"; //  T O ̲׭ȥ  T ] m
    }

    private IEnumerator AnimateTemperature(float targetTemperature)
    {
        // 檢查溫度字串是否為空
        if (string.IsNullOrEmpty(temperature_text.text))
        {
            Debug.LogError("Temperature text is empty or null.");
            yield break; // 直接退出 Coroutine，避免解析錯誤
        }

        // 嘗試解析當前溫度
        float currentTemperature;
        if (!float.TryParse(temperature_text.text.Replace("°C", ""), out currentTemperature))
        {
            Debug.LogError("Failed to parse temperature: " + temperature_text.text);
            yield break; // 如果解析失敗，直接退出 Coroutine
        }

        float duration = 4.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newTemperature = Mathf.Lerp(currentTemperature, targetTemperature, elapsed / duration);
            temperature_text.text = Mathf.RoundToInt(newTemperature).ToString();
            yield return null;
        }

        temperature_text.text = targetTemperature.ToString();
    }
}
