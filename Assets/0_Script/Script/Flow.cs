using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using SoftKitty.LiquidContainer;

public class Flow : MonoBehaviour
{
    public AnimationController AnimationController;
    public GameObject hint;
    public GameObject percentagescript;
    public GlucoseScaleCube GlucoseScaleCube1;
    public GlucoseScaleCube GlucoseScaleCube2;
    public Vector3 scale1 = new Vector3(0, 0.1f, 0);
    public Vector3 scale2 = new Vector3(0, 0.1f, 0);
    private XRGrabInteractable grabInteractable;

    private LiquidControl myLiquid;
    public LiquidControl targetLiquid;
    private float originalAmount;
    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnGrab);
        myLiquid = GetComponent<LiquidControl>();
        if (targetLiquid != null)
        {
            originalAmount = targetLiquid.GetCurrentTotalVolumn();
        }
    }

    public void OnGrab(SelectExitEventArgs args)
    {
        if (targetLiquid != null)
        {
            if (targetLiquid.GetCurrentTotalVolumn() > originalAmount)
            {
                targetLiquid.SetWaterLine(1f);
                StartCoroutine(DelayedAction());
            }
        }
    }

    private IEnumerator DelayedAction()
    {
        percentagescript.SendMessage("flow");

        GlucoseScaleCube1.SendMessage("UpdateScaleFactor", new GlucoseScaleCube.ScaleFactorParameters(scale1, false));
        GlucoseScaleCube2.SendMessage("UpdateScaleFactor", new GlucoseScaleCube.ScaleFactorParameters(scale2, false));

        yield return new WaitForSeconds(3f); 
      
        DoSomething();
    }

    private void DoSomething()
    {
        hint.SetActive(true);
    }
}
