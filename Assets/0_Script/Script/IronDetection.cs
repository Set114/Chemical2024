using UnityEngine;

public class IronDetection : MonoBehaviour
{
    // 帽子物体
    public GameObject capObject;
    // 帽子固定位置
    public Vector3 capFixedPosition = new Vector3(0f, 2f, 0f); // 帽子的固定位置，示例中为 (0, 2, 0)

    private void OnCollisionEnter(Collision collision)
    {
        // 检测到火碰撞到铁
        if (collision.gameObject.CompareTag("iron(4)"))
        {
            // 显示帽子物体
            capObject.SetActive(true);
            // 设置帽子的位置为固定位置
            capObject.transform.position = transform.position + capFixedPosition;
        }
    }
}
