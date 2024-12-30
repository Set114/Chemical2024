using UnityEngine;

public class ShowOnCFXR : MonoBehaviour
{
    public GameObject cap;
    public GameObject cap1;

    private Renderer renderer1;
    private Renderer renderer2;

    private void Start()
    {
        // 获取 cap 和 cap1 的渲染器组件
        renderer1 = cap1.GetComponent<Renderer>();
        renderer2 = cap.GetComponent<Renderer>();

        // 在开始时隐藏 cap 和 cap1
        if (renderer1 != null)
            renderer1.enabled = false;

        if (renderer2 != null)
            renderer2.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision occurred!");

        // 检查是否与火触发器发生碰撞
        if (collision.gameObject.CompareTag("fire"))
        {
            // 检查渲染器组件是否存在
            if (renderer1 != null)
                renderer1.enabled = true;

            if (renderer2 != null)
                renderer2.enabled = true;
        }
    }
}
