using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections; 

public class ButtonInteraction6 : MonoBehaviour
{
    public Button button;
    public TMP_Text n2Text;
    public TMP_Text o2Text;
    public TMP_Text x2Text;  // 添加新的 TMP_Text 变量
    public GameObject hintPlane;
    public GameObject no2Object;
    public GameObject CO2Object;
    public GameObject eggObject;
    public GameObject cuObject;
    public Button closeButton;  // 添加关闭按钮

    public GameObject EndUI;
    public GameObject TESTUI;

    private List<GameObject> collidedObjects = new List<GameObject>(); // 保存碰撞的对象
    public Stage5_UIManager stage5_UIManager;
    public ControllerHaptics controllerHaptics;

    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick); // 监听关闭按钮点击事件
    }
    void OnButtonClick()
    {
        StartCoroutine(OnButtonClickCoroutine());
    }
    IEnumerator OnButtonClickCoroutine()
    {
        string FeString = n2Text.text.Trim();
        string O2String = o2Text.text.Trim();
        string cString = x2Text.text.Trim();  // 获取 X2 的文本值

        if (FeString.Equals("2") && O2String.Equals("3") && cString.Equals("1"))  // 更新条件
        {
            ShowNO2Object();
            ShowCO2Object();
            HideHintPlane();
            HideEggObject();
            HideCuObject();
            HideCollidedGasObjects();
            DestroyAllGasObjects();
            yield return new WaitForSeconds(2f);
        
            TESTUI.SetActive(true);
            EndUI.SetActive(true);
            
         
        }
        else
        {
            //level3AudioSet.leve1Error();
            controllerHaptics.TriggerHapticFeedback(true);
            HideNO2Object();
            ShowHintPlane();
           
           
        }
    }

    void OnCloseButtonClick()
    {
        HideHintPlane();
        ShowEggObject();
        HideCollidedGasObjects();
        ResetTextValues();
       
    }

    void ShowNO2Object()
    {
        if (no2Object != null)
            no2Object.SetActive(true);
    }
    void ShowCO2Object()
    {
        if (CO2Object != null)
            CO2Object.SetActive(true);
    }
    void HideNO2Object()
    {
        if (no2Object != null)
            no2Object.SetActive(false);
    }

    void ShowHintPlane()
    {
        if (hintPlane != null)
            hintPlane.SetActive(true);
    }

    void HideHintPlane()
    {
        if (hintPlane != null)
            hintPlane.SetActive(false);
    }

    void ShowEggObject()
    {
        if (eggObject != null)
            eggObject.SetActive(true);
    }

    void HideEggObject()
    {
        if (eggObject != null)
            eggObject.SetActive(false);
    }

    void HideCuObject()
    {
        if (cuObject != null)
            cuObject.SetActive(false);
    }

    void HideCollidedGasObjects()
    {
        GameObject[] gasObjects = GameObject.FindGameObjectsWithTag("gas");
        // 找到最后一个保留的 N2、O2 和 X2 对象并将其从列表中移除
        GameObject latestef = FindLatestGasObject(gasObjects, "ef");
        GameObject latestO2 = FindLatestGasObject(gasObjects, "O2");
        GameObject latestc = FindLatestGasObject(gasObjects, "c");

        List<GameObject> objectsToHide = new List<GameObject>();
        foreach (GameObject obj in gasObjects)
        {
            if (obj != latestef && obj != latestO2 && obj != latestc)
            {
                objectsToHide.Add(obj);
            }
        }

        // 隐藏所有其余碰撞的 gas 对象
        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }
    }
    void DestroyAllGasObjects()
    {
        GameObject[] gasObjects = GameObject.FindGameObjectsWithTag("gas");

        // 销毁所有气体对象
        foreach (GameObject obj in gasObjects)
        {
            Debug.Log("Destroying object: " + obj.name);
            Destroy(obj);
        }
    }

    GameObject FindLatestGasObject(GameObject[] gasObjects, string prefix)
    {
        GameObject latestGas = null;
        foreach (GameObject obj in gasObjects)
        {
            if (obj.name.StartsWith(prefix))
            {
                if (latestGas == null || obj.name.CompareTo(latestGas.name) > 0)
                {
                    latestGas = obj;
                }
            }
        }
        return latestGas;
    }

    void ResetTextValues()
    {
        if (n2Text != null)
            n2Text.text = "0";
        if (o2Text != null)
            o2Text.text = "0";
        if (x2Text != null)  // 重置 X2 的文本值
            x2Text.text = "0";
    }
}