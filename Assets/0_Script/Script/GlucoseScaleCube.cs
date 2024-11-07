using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


public class GlucoseScaleCube : MonoBehaviour
{
    public ScaleCubeCalculate scaleCubeScript;
    public GameObject hint1;
    public GameObject hint2;
    public Button Button;
    public TextMeshProUGUI parameterDisplayText;
    public Vector3 scale1 = new Vector3(0, 0.1f, 0);
    public Vector3 scale2 = new Vector3(0, 0.1f, 0);
    public int levelindex;

    public SwitchUI switchUI;
    
    

    private bool isMove = false;
    public bool a1 = false;
    public bool isParticleTriggered = false;
    
    public bool tempflag = true;

    void OnEnable()
    {
        levelindex = switchUI.GetLevelCount();
        Debug.Log("levelindex = " + levelindex);
        // if (levelindex == 2)
        // {
        //     Button.onClick.AddListener(OnButtonClicked);
        // }
    }

    private void Update()
    {
        if (levelindex == 2)
        {
            if (isParticleTriggered && !isMove)
            {
                UpdateScaleFactor(new ScaleFactorParameters(scale1,true));
                isMove = true;
                Debug.Log("Update is true");
            }
        }
    }

    [System.Serializable]
    public class ScaleFactorParameters
    {
        public Vector3 scale;
        public bool showHint;

        public ScaleFactorParameters(Vector3 scale, bool showHint = false)
        {
            this.scale = scale;
            this.showHint = showHint;
        }
    }


    public void UpdateScaleFactor(ScaleFactorParameters parameters)
    {
        Vector3 direction = Vector3.up;
        scaleCubeScript.ScaleInOneDirection(parameters.scale, direction, parameterDisplayText);
        
        if (parameters.showHint)
        {
            if (a1)
            {
                Debug.Log("A1 is true");
                StartCoroutine(ActivateHintWithDelay(4.5f, hint2));
            }
            else
            {
                Debug.Log("A1 is false");
                StartCoroutine(ActivateHintWithDelay(4.5f, hint1));
                a1 = true;
            }
        }
    }

   

    private IEnumerator ActivateHintWithDelay(float delay, GameObject hint)
    {
        Debug.Log("hint is true");
        yield return new WaitForSeconds(delay);
        hint.SetActive(true);
    }

    public void OnButtonClicked()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null)
        {
            Debug.Log("觸發此方法的按鈕是: " + clickedButton.name);
        }
        if (tempflag)
        {
            Debug.Log("目前物件的名稱是: " + gameObject.name);
            UpdateScaleFactor(new ScaleFactorParameters(scale2,true));
            Debug.Log("hint2 is ACTIVE");
        }
    }
    public void NotToClickAgain()
    {
        tempflag = false;
    }
}