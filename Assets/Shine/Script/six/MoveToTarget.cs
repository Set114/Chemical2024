using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public Transform target;     // 目標位置
    public float speed = 3f;     // 移動速度

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
