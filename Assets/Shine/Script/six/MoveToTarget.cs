using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public Transform target;     // �ؼЦ�m
    public float speed = 3f;     // ���ʳt��

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
