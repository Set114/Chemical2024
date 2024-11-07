using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerHaptics : MonoBehaviour
{
    public XRBaseController leftController;
    public XRBaseController rightController;
    public float amplitude = 0.5f; // 震動強度
    public float duration = 0.5f; // 震動持續時間

    public void TriggerHapticFeedback(bool isShock)
    {
        if (isShock)
        {
            StartCoroutine(TriggerHaptic(leftController));
            StartCoroutine(TriggerHaptic(rightController));
        }
    }

    private IEnumerator TriggerHaptic(XRBaseController controller)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude, duration);
            yield return new WaitForSeconds(duration);
        }
    }
}
