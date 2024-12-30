using UnityEngine;

public class AnimationEventMediator2 : MonoBehaviour
{
    public GameObject IceBlockCollision2; // 具有 CollisionDetection 脚本的对象

    // 动画事件将会调用这个方法
    public void OnAnimationEnd()
    {
        // 向 collisionDetectionObject 发送消息
        IceBlockCollision2.SendMessage("Q3question");
    }
}
