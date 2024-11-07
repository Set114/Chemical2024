using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    // 在Unity编辑器中将动画对象拖放到这里
    public Animation anim;

    void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞对象是否是铁
        if (collision.gameObject.CompareTag("Iron"))
        {
            // 播放动画
            anim.Play();
        }
    }
}
