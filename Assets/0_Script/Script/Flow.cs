using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnGrab);
    }

    public void OnGrab(SelectExitEventArgs args)
    {
        StartCoroutine(DelayedAction());
    }

    private IEnumerator DelayedAction()
    {
        percentagescript.SendMessage("flow");

        GlucoseScaleCube1.SendMessage("UpdateScaleFactor", new GlucoseScaleCube.ScaleFactorParameters(scale1, false));
        GlucoseScaleCube2.SendMessage("UpdateScaleFactor", new GlucoseScaleCube.ScaleFactorParameters(scale2, false));

        yield return new WaitForSeconds(7.5f); 
      
        DoSomething();
    }

    private void DoSomething()
    {
        hint.SetActive(true);
    }
}
