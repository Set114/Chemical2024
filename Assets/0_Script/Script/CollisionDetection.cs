using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    public Animator ani1;
    public Animator ani2;
    public Animator ani3;
    public string targetTag = "Object";
    // public GameObject canva;
    void OnTriggerEnter(Collider other)
    {
        //如果碰撞
        if (other.CompareTag(targetTag))
        {
            ani1.SetBool("move", true);
            ani2.SetBool("move", true);
            ani3.SetBool("move1", true);
            // Debug.Log("aniStart");
        }
    }
    // 确保该方法可以被消息系统调用
    // public void OnAnimationEnd()
    // {
    //     canva.SetActive(true);

    // }
}
