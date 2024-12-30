using UnityEngine;

public class Ignition : MonoBehaviour
{
    public GameObject cover;
    public GameObject fire; 
    public GameObject hint; 
    public GameObject percentagescript;
    public GlucoseScaleCube GlucoseScaleCube1;
    public GlucoseScaleCube GlucoseScaleCube2;
    private Vector3 previousCoverPosition;
    private bool isMove = false;
    public Animator a1;
    public Vector3 scale1 = new Vector3(0, 0.1f, 0);
    public Vector3 scale2 = new Vector3(0, 0.1f, 0);

    void Start()
    {
        previousCoverPosition = cover.transform.position;
    }
    private void Update()
    {
        if (Vector3.Distance(cover.transform.position, previousCoverPosition) > 0.001f && !isMove)
        {
            a1.SetBool("matche1", true);
            isMove = true;
        }
    }

    public void OnAnimationMiddle()
    {
        fire.SetActive(true);
        percentagescript.SendMessage("ignition");
        GlucoseScaleCube1.SendMessage("UpdateScaleFactor", new GlucoseScaleCube.ScaleFactorParameters(scale1, false));
        GlucoseScaleCube2.SendMessage("UpdateScaleFactor", new GlucoseScaleCube.ScaleFactorParameters(scale2, false));     
    }

    public void OnAnimationEnd()
    {
        hint.SetActive(true);
    }
}
