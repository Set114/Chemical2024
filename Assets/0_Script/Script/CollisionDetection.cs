using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private GameObject tutorialObject;
    public string targetTag = "Object";
    // public GameObject canva;
    void OnTriggerEnter(Collider other)
    {
        //如果碰撞
        if (other.CompareTag(targetTag))
        {
            tutorialObject.SendMessage("PaperTouched");
        }
    }
}
