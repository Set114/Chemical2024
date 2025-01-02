using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private GameObject tutorialObject;
    [SerializeField] private string targetName = "Object";
    [SerializeField] private bool sendObj = false;

    void OnTriggerEnter(Collider other)
    {
        //如果碰撞
        if (other.gameObject.name == targetName)
        {
            if (sendObj)
            {
                tutorialObject.SendMessage("Reaction", gameObject);
            }
            else
            {
                tutorialObject.SendMessage("Reaction");
            }
        }
    }
}
