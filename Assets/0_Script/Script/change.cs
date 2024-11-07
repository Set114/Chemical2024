using UnityEngine;

public class hammer : MonoBehaviour {

    public GameObject fire ;
    public Color 变色;

    // 当物体与其他碰撞器碰撞时调用
    public void OnCollisionEnter(Collision collision) {
        // 获取碰撞到的物体
        GameObject otherObject = collision.gameObject;

        // 输出碰撞到的物体的标签，用于调试
        Debug.Log("碰撞发生：" + otherObject.tag);

        // 检查是否是你想要改变颜色的物体
        if (otherObject.CompareTag("iron(2)")) {
            // 获取物体的 MeshRenderer 组件
            MeshRenderer renderer = otherObject.GetComponent<MeshRenderer>();
            
            // 如果物体有 MeshRenderer 组件
            if (renderer != null) {
                // 修改物体的颜色
                renderer.material.color = 变色;

                // 输出日志，用于调试
                Debug.Log("碰撞到了 'iron(2)'，已改变颜色。");
            } else {
                Debug.Log("碰撞到了 'iron(2)'，但物体没有 MeshRenderer 组件。");
            }
        }
    }
}
