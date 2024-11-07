using UnityEngine;

public class RealBall : MonoBehaviour
{
    public GameObject whiteboard;

    private ElementalBallCounter elementalBallCounter;
    private ElementDisplay elementDisplay;
    private DetectBall detectBall;

    public bool isInsideTrigger = false;

    void Awake()
    {
        elementalBallCounter = FindObjectOfType<ElementalBallCounter>();
        elementDisplay = FindObjectOfType<ElementDisplay>();
        detectBall = FindObjectOfType<DetectBall>();
    }

    void OnTriggerEnter(Collider other)
    {
        string currentTag = gameObject.tag;
        if (other.CompareTag("TrashBin"))
        {
            // 碰到垃圾盒!isInsideTrigger &&
            elementalBallCounter.AddBalls(currentTag, 1);
            elementDisplay.UpdateDisplay();
            detectBall.allcount--;
            switch (currentTag)
            {
                case "C":
                    //Debug.Log($"Before decrement: detectBall.c = {detectBall.c}");
                    detectBall.c--;
                    //Debug.Log($"After decrement: detectBall.c = {detectBall.c}");
                    break;
                case "N":
                    detectBall.n--;
                    break;
                case "O":
                    detectBall.o--;
                    break;
                case "H":
                    detectBall.h--;
                    break;
                case "Fe":
                    detectBall.fe--;
                    break;
                default:
                    Debug.LogWarning($"Unknown element type: {currentTag}");
                    break;
            }
            UpdateNumbers(-1);
            // 更新原料盒和白板数字
            Destroy(gameObject); // 銷毀真球

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TrashBin"))
        {
            // 進入收集盆
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 0), Time.deltaTime * 5); // 假設收集盆在原點
        }
    }

    void UpdateNumbers(int whiteboardChange)
    {
        whiteboard.GetComponent<Whiteboard>().ChangeValue(whiteboardChange);
    }
}