using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnToStartPosition : XRGrabInteractable
{
    private Vector3 initialPosition; // 物体的初始位置
    private Quaternion initialRotation; // 物体的初始世界旋转

    protected override void Awake()
    {
        base.Awake();

        // 记录物体的初始位置和世界旋转
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // 订阅事件
        selectEntered.AddListener(OnSelectEnteredHandler);
        selectExited.AddListener(OnSelectExitedHandler);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // 取消订阅事件
        selectEntered.RemoveListener(OnSelectEnteredHandler);
        selectExited.RemoveListener(OnSelectExitedHandler);
    }

    private void OnSelectEnteredHandler(SelectEnterEventArgs args)
    {
        // 保持原始角度，不做任何改变
        Debug.Log("OnSelectEntered called");

        // 禁用XRGrabInteractable的旋转控制
        if (attachTransform != null)
        {
            attachTransform.rotation = initialRotation;
        }
        Debug.Log("OnSelectEnteredHandler");
    }

    private void OnSelectExitedHandler(SelectExitEventArgs args)
    {
        // 当手放开物体时，返回到初始位置和旋转
        Debug.Log("OnSelectExited called");
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
