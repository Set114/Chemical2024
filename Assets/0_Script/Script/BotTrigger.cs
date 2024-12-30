using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BotTrigger : MonoBehaviour
{
    public Transform bot;
    public GameObject warning;
    public Transform originalPosition;
    public CTrigger[] colliderTriggers;
    private XRGrabInteractable grabInteractable;
    
    
    void Start()
    {
        // 获取所有拥有 CTrigger 脚本的物体
        colliderTriggers = FindObjectsOfType<CTrigger>();

        if (colliderTriggers.Length == 0)
        {
            //Debug.LogError("No objects with CTrigger script found!");
        }
    }
    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectExited.AddListener(OnPutDown);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bot"))
        {
            if (Vector3.Distance(other.transform.position, bot.position) < 0.05f)
            {
                AttachToTargetPoint(bot);

                // 设置所有对象的 botPlaced 属性为 true
                foreach (CTrigger trigger in colliderTriggers)
                {
                    trigger.botPlaced = true;
                }
                warning.SetActive(false);
            }
        }
    }

    void AttachToTargetPoint(Transform targetPoint)
    {
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
    }

    public void OnPutDown(SelectExitEventArgs args)
    {
        if (Vector3.Distance(transform.position, originalPosition.position) > 0.05f)
        {
            bool isAtTargetPoint = false;

            if (Vector3.Distance(transform.position, bot.position) < 0.05f)
            {
                isAtTargetPoint = true;
            }

            if (!isAtTargetPoint)
            {
                transform.position = originalPosition.position;
                transform.rotation = originalPosition.rotation;
            }
        }
    }
}
