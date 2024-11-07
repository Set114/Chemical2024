using UnityEngine;

public class ChangeMaterial0Collision : MonoBehaviour
{
    public Material colorIFie; // 颜色 fie 的材质
    public float scaleAmount = 0.5f; // 缩小的比例

    private Renderer rend;
    private Vector3 originalScale;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalScale = transform.localScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("fire"))
        {
            // 改变材质颜色
            rend.material = colorIFie;

            // 缩小物体
            transform.localScale *= scaleAmount;
        }
    }
}

