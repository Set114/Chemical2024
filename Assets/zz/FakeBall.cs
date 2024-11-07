using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class FakeBall : MonoBehaviour
{
    public GameObject realBallPrefab;
    public GameObject whiteboard;

    private Vector3 originalPosition;
    private Rigidbody rb;

    public DetectBall detectBall;
    private ElementalBallCounter elementalBallCounter;
    private ElementDisplay elementDisplay;
    private string currentLevel;

    void Awake()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();

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
        string currentTag = gameObject.tag;

        //Debug.Log($"OnTriggerEnter - Current Level: {currentLevel}, Tag: {currentTag}");

        if (currentLevel == "part0")
        {
            HandlePart0Trigger(currentTag);
        }
        else
        {
            HandleOtherPartsTrigger(currentTag);
        }

        // 重置剛體約束
        rb.constraints = RigidbodyConstraints.None;
    }

    private void HandlePart0Trigger(string currentTag)
    {
        bool hasAvailableElement = false;
        switch (currentTag)
        {
            case "C":
                hasAvailableElement = detectBall.displayC > 0;
                break;
            case "N":
                hasAvailableElement = detectBall.displayN > 0;
                break;
            case "O":
                hasAvailableElement = detectBall.displayO > 0;
                break;
            case "H":
                hasAvailableElement = detectBall.displayH > 0;
                break;
            case "Fe":
                hasAvailableElement = detectBall.displayFe > 0;
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
        if (elementalBallCounter.GetBallCount(currentTag) >= 1)
        {
            SpawnRealBall();
            elementalBallCounter.RemoveBalls(currentTag, 1);
            elementDisplay.UpdateDisplay();
        }
        else
        {
            DisableGrabInteraction();
        }
    }

    private void SpawnRealBall()
    {
        Instantiate(realBallPrefab, transform.position, Quaternion.identity);
        ResetPosition();
        UpdateNumbers(1);
    }

    private void ResetPosition()
    {
        transform.position = originalPosition;
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