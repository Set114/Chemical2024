using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections; 

public class ButtonInteraction2 : MonoBehaviour
{
    public Button button;
    public TMP_Text n2Text;
    public TMP_Text o2Text;
    public GameObject hintPlane;
    public GameObject no2Object;
    public GameObject eggObject;
    public GameObject cuObject;
    public Button closeButton;  // 添加关闭按钮
    public ControllerHaptics controllerHaptics;

    public Stage5_UIManager stage5_UIManager;
    private List<GameObject> collidedObjects = new List<GameObject>(); // 保存碰撞的对象

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
        string cString = n2Text.text.Trim();
        string o2String = o2Text.text.Trim();
        if (cString.Equals("1") && o2String.Equals("2"))
        {
            ShowNO2Object();
            HideHintPlane();
            HideeggObject();
            HidecuObject();
            HideCollidedGasObjects();
            DestroyAllGasObjects();
            
            yield return new WaitForSeconds(2f);
            
            stage5_UIManager.ShowTestNextUI();
        }
        else
        {
            controllerHaptics.TriggerHapticFeedback(true);
            HideNO2Object();
            ShowHintPlane();
           
        }
    }

    
    void OnCloseButtonClick()
    {
        HideHintPlane();
        ShoweggObject();
        HideCollidedGasObjects();
        ResetTextValues();
        
    }

    void ShowNO2Object()
    {
        if (no2Object != null)
            no2Object.SetActive(true);
    }
    void ShowcuObject()
    {
        if (cuObject != null)
            cuObject.SetActive(true);
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

    void ShoweggObject()
    {
        if (eggObject != null)
            eggObject.SetActive(true);
    }
    void HidecuObject()
    {
        if (cuObject != null)
            cuObject.SetActive(false);
    }

    void HideeggObject()
    {
        if (eggObject != null)
            eggObject.SetActive(false);
    }

    void HideCollidedGasObjects()
    {
        GameObject[] gasObjects = GameObject.FindGameObjectsWithTag("gas");
        // 找到最后一个保留的 N2 和 O2 对象并将其从列表中移除
        GameObject latestC = FindLatestGasObject(gasObjects, "c(sphere)");
        GameObject latestO2 = FindLatestGasObject(gasObjects, "O2");

        List<GameObject> objectsToHide = new List<GameObject>();
        foreach (GameObject obj in gasObjects)
        {
            if (obj != latestC && obj != latestO2)
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
    }
}
