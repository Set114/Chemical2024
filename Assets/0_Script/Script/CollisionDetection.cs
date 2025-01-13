using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private GameObject tutorialObject;
    public string targetName = "Object";
    [SerializeField] private bool sendObj = false;

    void OnTriggerEnter(Collider other)
    {
        //如果碰撞
        if (other.gameObject.name == targetName)
        {
            if (sendObj)
            {
                tutorialObject.SendMessage("Reaction", gameObject, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                tutorialObject.SendMessage("Reaction", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        //如果碰撞
        if (other.gameObject.name == targetName)
        {
            if (sendObj)
            {
                tutorialObject.SendMessage("ReactionExit", gameObject, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                tutorialObject.SendMessage("ReactionExit", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        //如果碰撞
        if (other.gameObject.name == targetName)
        {
            if (sendObj)
            {
                tutorialObject.SendMessage("ReactionStay", gameObject, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                tutorialObject.SendMessage("ReactionStay",SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
