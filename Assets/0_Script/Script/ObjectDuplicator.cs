using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectDuplicator : MonoBehaviour
{
    public XRGrabInteractable grabInteractable; // 用于拾取物体的 XRGrabInteractable 组件
    public GameObject objectPrefab; // 要生成的物体预制体

    private Vector3 originalPosition; // 物体的原始位置
    private GameObject spawnedObject;
    private int objectCount = 0; // 生成的物体计数器

    void Start()
    {
        originalPosition = transform.position; // 记录物体的原始位置
        // 添加事件监听器，当物体被拾取和放下时触发相应的方法
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    // 当物体被放下时触发
    void OnRelease(SelectExitEventArgs args)
    {
        if (spawnedObject == null)
        {
            // 生成一个新的物体
            objectCount = objectCount + 1;          
            spawnedObject = Instantiate(objectPrefab, originalPosition, transform.rotation);
            spawnedObject.name = objectPrefab.name + " " + objectCount; // 修改生成物体的名字，移除 (Clone) 并添加计数器
        }
    }
}