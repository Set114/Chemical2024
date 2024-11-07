using UnityEngine;
using TMPro;

public class WaterScaleCube : MonoBehaviour
{
    public ScaleCubeCalculate scaleCubeScript;
    public GameObject hint;
    public GameObject cover;
    public GameObject liquid;
    public GameObject h2o;
    public GameObject H2ONo;
    public TextMeshProUGUI parameterDisplayText;
    public bool isMove = false;
    public Vector3 scale0 = new Vector3(0, 0, 0);
    public Vector3 scale1 = new Vector3(0, 0.1f, 0);
    public Vector3 scale2 = new Vector3(0, 0.1f, 0);

    private Vector3 previousCoverPosition;
    private float targetScaleY = 0.5f;
    private float scaleSpeed = 0.3f; 

    public int levelindex;
    public bool flag1 = false; 
    public bool flag2 = false; 

    public SwitchUI switchUI;

    void OnEnable()
    {
        levelindex = switchUI.GetLevelCount();
        if (!flag1)
        {
            previousCoverPosition = cover.transform.position;
            UpdateScaleFactor(scale1);
            H2ONo.SetActive(false);  
            flag1 = true;
        }          
    }

    private void Update()
    {
        if (levelindex == 1 && !flag2)
        {
            if (Vector3.Distance(cover.transform.position, previousCoverPosition) > 0.001f && !isMove)
            {
                UpdateScaleFactor(scale2);
                isMove = true;
                h2o.SetActive(false);
                H2ONo.SetActive(true);
            }

            if (isMove)
            {
                UpdateLiquidScale();
            }
        }   

    }

    private void UpdateScaleFactor(Vector3 scale)
    {
        Vector3 direction = Vector3.up;
        scaleCubeScript.ScaleInOneDirection(scale, direction , parameterDisplayText);
    }

    private void UpdateLiquidScale()
    {
        Vector3 currentScale = liquid.transform.localScale;

        currentScale.y = Mathf.Lerp(currentScale.y, targetScaleY, scaleSpeed * Time.deltaTime);

        liquid.transform.localScale = currentScale;

        if (Mathf.Abs(currentScale.y - targetScaleY) < 0.01f)
        {
            currentScale.y = targetScaleY;
            liquid.transform.localScale = currentScale;
        }
    }

}
