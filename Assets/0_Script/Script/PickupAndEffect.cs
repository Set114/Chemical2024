using UnityEngine;

public class PickupAndEffect : MonoBehaviour
{
    public GameObject effectPrefab; // 特效的预制体
    private bool isPickedUp = false; // 是否已经拾取物体

    void Update()
    {
        if (isPickedUp) // 检查是否已拾取物体
        {
            PlayEffect();
            isPickedUp = false; // 重置拾取标志
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果触发器碰到了玩家的 "fire" 物体
        if (other.CompareTag("Fire"))
        {
            isPickedUp = true; // 设置物体已被拾取
        }
    }

    private void PlayEffect()
    {
        if (effectPrefab != null) // 检查特效预制体是否存在
        {
            // 在物体位置播放特效
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("特效预制体未设置！");
        }
    }
}
