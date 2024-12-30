using UnityEngine;

public class ShowCapOnFireCollision : MonoBehaviour
{
    // 帽子物体
    public GameObject capObject;
    public GameObject capObject1;

    private void Start()
    {
        // 开始时隐藏帽子物体
        capObject.SetActive(false);
        capObject1.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检测到火或特定类型的铁碰撞
        if (collision.gameObject.CompareTag("IronType1") || collision.gameObject.CompareTag("fire"))
        {
            Debug.Log("火或特定类型的铁碰撞！");

            // 显示帽子物体
            capObject.SetActive(true);
            capObject1.SetActive(true);
        }
        else
        {
            Debug.Log("碰撞物体标签：" + collision.gameObject.tag);
        }
    }
}
