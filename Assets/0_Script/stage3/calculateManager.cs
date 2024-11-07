using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class calculateManager : MonoBehaviour
{
    public Button buybutton;

    public TMP_Text totalText;
    public TMP_Text cText;
    public TMP_Text nText;
    public TMP_Text oText;
    public TMP_Text hText;
    public TMP_Text feText;

    public Text cMText; // C(?)m的文本
    public Text nMText; 
    public Text oMText; 
    public Text hMText; 
    public Text feMText; 
    private int cCount = 0;
    private int nCount = 0;
    private int oCount = 0;
    private int hCount = 0;
    private int feCount = 0;
    private float cPrice;
    private float nPrice;
    private float oPrice;
    private float hPrice;
    private float fePrice;
    private float totalPrice;
    public static calculateManager Instance;
    private CanvasController_Unit3 canvasController;

    public float initialTotalPrice = 300f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log(gameObject);
        }
    }
     void Start()
     {
        buybutton.onClick.AddListener(Onbuybutton);

        cPrice = float.Parse(cMText.text);
        nPrice = float.Parse(nMText.text);
        oPrice = float.Parse(oMText.text);
        hPrice = float.Parse(hMText.text);
        fePrice = float.Parse(feMText.text);
        totalPrice = initialTotalPrice;

        canvasController = FindObjectOfType < CanvasController_Unit3>();

        UpdateTotalPrice(0);

        UpdateCText();
        UpdateNText();
        UpdateOText();
        UpdateHText();
        UpdateFeText();
     }

    public void OnCPlusClicked()
    {
        cCount++;
        UpdateCText();
        UpdateTotalPrice(-cPrice);
    }
    public void OnCMinusClicked()
    {
        if (cCount > 0)
        { 
        cCount--;
        UpdateCText();
        UpdateTotalPrice(cPrice);
        }
    }
    public void OnNPlusClicked()
    {
        nCount++;
        UpdateNText();
        UpdateTotalPrice(-nPrice);
    }
    public void OnNMinusClicked()
    {
        if (nCount > 0)
        {
            nCount--;
            UpdateNText();
            UpdateTotalPrice(nPrice);
        }
    }
    public void OnHPlusClicked()
    {
        hCount++;
        UpdateHText();
        UpdateTotalPrice(-hPrice);
    }
    public void OnHMinusClicked()
    {
        if (hCount > 0)
        {
            hCount--;
            UpdateHText();
            UpdateTotalPrice(hPrice);
        }
    }
    public void OnOPlusClicked()
    {
        oCount++;
        UpdateOText();
        UpdateTotalPrice(-oPrice);
    }
    public void OnOMinusClicked()
    {
        if (oCount > 0) oCount--;
        UpdateOText();
        UpdateTotalPrice(oPrice);
    }
    public void OnFePlusClicked()
    {
        feCount++;
        UpdateFeText();
        UpdateTotalPrice(-fePrice);
    }
    public void OnFeMinusClicked()
    {
        if (feCount > 0)
        { 
            feCount--;
        UpdateFeText();
        UpdateTotalPrice(fePrice);}
        
    }
    private void UpdateCText()
    {
        cText.text = cCount.ToString();
    }
    private void UpdateNText()
    {
        nText.text = nCount.ToString();
    }
    private void UpdateOText()
    {
        oText.text = oCount.ToString();
    }
    private void UpdateHText()
    {
        hText.text = hCount.ToString();
    }
    private void UpdateFeText()
    {
        feText.text = feCount.ToString();
    }

    private void UpdateTotalPrice(float priceChange)
    {
        totalPrice += priceChange;
        totalText.text = totalPrice.ToString("F0"); // Display with 2 decimal places
    }

    public void Onbuybutton()
    {
        canvasController.Buy();
        StartCoroutine(BuyProcess());
    }
    private IEnumerator BuyProcess()
    {
        ElementalBallCounter aa = FindObjectOfType<ElementalBallCounter>();
        ElementDisplay elementDisplay = FindObjectOfType<ElementDisplay>();
        DetectBall detectBall = FindObjectOfType<DetectBall>();

        if (aa != null)
        {
            aa.GetCounts(cCount, nCount, oCount, hCount, feCount);

            // 延迟 
            yield return new WaitForSeconds(0.2f);

            detectBall.FindParts(); // 更新关卡
            detectBall.CheckRequiredElementQuantities();
            elementDisplay.UpdateDisplay(); // 更新显示
        }
    }

        public void ResetAllCounts()
    {
        cCount = 0;
        nCount = 0;
        oCount = 0;
        hCount = 0;
        feCount = 0;
        initialTotalPrice = 300f;
    }
}