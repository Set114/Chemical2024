using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

public class FakeBall : MonoBehaviour
{
    public GameObject realBallPrefab;
    public GameObject whiteboard;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    public DetectBall detectBall;
    private ElementalBallCounter elementalBallCounter;
    private ElementDisplay elementDisplay;
    private string currentLevel;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnGrab);

        elementalBallCounter = FindObjectOfType<ElementalBallCounter>();
        elementDisplay = FindObjectOfType<ElementDisplay>();

        DetectBall detectBallComponent = FindObjectOfType<DetectBall>();
        if (detectBallComponent != null)
        {
            currentLevel = detectBallComponent.CurrentLevel;
        }
        else
        {
            Debug.LogWarning("DetectBall component not found!");
        }
    }

    public void OnGrab(SelectExitEventArgs args)
    {
        // 重置剛體約束
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnEnable()
    {
        DetectBall.OnLevelChanged += HandleLevelChanged;
    }

    private void OnDisable()
    {
        DetectBall.OnLevelChanged -= HandleLevelChanged;
    }

    private void HandleLevelChanged(string newLevel)//查找關卡
    {
        currentLevel = newLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name== "Atomicdetection")
        {
            string currentTag = gameObject.tag;

            //Debug.Log($"OnTriggerEnter - Current Level: {currentLevel}, Tag: {currentTag}");

            /*
            if (currentLevel == "part0")
            {
                //HandlePart0Trigger(currentTag);
            }*/
            HandleOtherPartsTrigger(currentTag);  

            // 重置剛體約束
            //rb.constraints = RigidbodyConstraints.None;
        }else if(other.gameObject.tag == "TrashBin")
        {
            ResetPosition();
        }
    }

    private void HandlePart0Trigger(string currentTag)
    {
        bool hasAvailableElement = false;
        switch (currentTag)
        {
            case "C":
                hasAvailableElement = elementalBallCounter.GetBallCount("c") > 1;
                break;
            case "N":
                hasAvailableElement = elementalBallCounter.GetBallCount("n") > 1;
                break;
            case "O":
                hasAvailableElement = elementalBallCounter.GetBallCount("o") > 1;
                break;
            case "H":
                hasAvailableElement = elementalBallCounter.GetBallCount("h") > 1;
                break;
            case "Fe":
                hasAvailableElement = elementalBallCounter.GetBallCount("fe") > 1;
                break;
            default:
                Debug.LogWarning($"Unknown element type: {currentTag}");
                break;
        }

        if (hasAvailableElement)
        {
            SpawnRealBall();
        }
        else
        {
            DisableGrabInteraction();
        }
    }

    private void HandleOtherPartsTrigger(string currentTag)
    {
        /*if (elementalBallCounter.GetBallCount(currentTag) > 1)
        {
            SpawnRealBall();
        }
        else
        {
            DisableGrabInteraction();
        }*/
        SpawnRealBall();
        elementalBallCounter.RemoveBalls(currentTag, 1);
        elementDisplay.UpdateDisplay();
    }

    private void SpawnRealBall()
    {
        Instantiate(realBallPrefab, transform.position, Quaternion.identity, transform.parent);
        ResetPosition();
        UpdateNumbers(1);
    }

    private void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    private void DisableGrabInteraction()
    {
        var grabInteractable = GetComponent<XRGrabInteractable>();
        Debug.Log("DisableGrabInteraction:" + grabInteractable);
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            Debug.Log($"Disabled grab interaction for {gameObject.name}");
        }
    }

    private void UpdateNumbers(int whiteboardChange)
    {
        whiteboard.GetComponent<Whiteboard>().ChangeValue(whiteboardChange);
    }
}