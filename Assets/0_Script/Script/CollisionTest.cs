using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    // 当碰撞发生时调用
    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞对象的标签是否为"Player"
        if (collision.gameObject.CompareTag("hammer"))
        {
            Debug.Log("Collision with player detected!");
        }
    }
}
