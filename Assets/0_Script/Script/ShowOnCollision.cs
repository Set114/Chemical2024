using UnityEngine;

public class ShowOnCollision : MonoBehaviour
{
    // 需要显示的 GameObject
    public GameObject objectToShow;

    // 在碰撞开始时调用
    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞的对象是否为名为 "CFXR" 的 GameObject
        if (collision.gameObject.CompareTag("CFXR"))
        {
            // 显示 objectToShow
            objectToShow.SetActive(true);
        }
    }

    // 在碰撞结束时调用（可选）
    // private void OnCollisionExit(Collision collision)
    // {
    //     // 在需要时执行某些操作
    // }

    // 在 Start 方法中将 cap 和 cap(1) 隐藏
    private void Start()
    {
        // 隐藏 cap
        gameObject.SetActive(false);

        // 隐藏 cap(1)
        GameObject cap1 = GameObject.Find("cap(1)");
        if (cap1 != null)
        {
            cap1.SetActive(false);
        }
    }
}
