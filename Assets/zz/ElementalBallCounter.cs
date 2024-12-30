using UnityEngine;

public class ElementalBallCounter : MonoBehaviour
{
    [SerializeField] private int carbonCount;
    [SerializeField] private int nitrogenCount;
    [SerializeField] private int oxygenCount;
    [SerializeField] private int hydrogenCount;
    [SerializeField] private int ironCount;
    private DetectBall detectBall;
    private void Start()
    {
        // ��l�ƦU�������ƶq
        carbonCount = 0;
        nitrogenCount = 0;
        oxygenCount = 0;
        hydrogenCount = 0;
        ironCount = 0;
        detectBall = FindObjectOfType<DetectBall>();
    }

    public void GetCounts(int carbon, int nitrogen, int oxygen, int hydrogen, int iron)
    {
        /*AddBalls("c", carbonCount);
        AddBalls("n", nitrogenCount);
        AddBalls("o", oxygenCount);
        AddBalls("h", hydrogenCount);
        AddBalls("fe", ironCount);
        */
        carbonCount = carbon - detectBall.c;
        nitrogenCount = nitrogen - detectBall.n;
        oxygenCount = oxygen - detectBall.o;
        hydrogenCount = hydrogen - detectBall.h;
        ironCount = iron - detectBall.fe;
    }

    // �W�[�y���ƶq
    public void AddBalls(string element, int amount)
    {
        switch (element.ToLower())
        {
            case "c":
                carbonCount += amount;
                Debug.Log($"Added {amount} Carbon balls. Total: {carbonCount}");
                break;
            case "n":
                nitrogenCount = amount;
                Debug.Log($"Added {amount} Nitrogen balls. Total: {nitrogenCount}");
                break;
            case "o":
                oxygenCount = amount;
                Debug.Log($"Added {amount} Oxygen balls. Total: {oxygenCount}");
                break;
            case "h":
                hydrogenCount = amount;
                Debug.Log($"Added {amount} Hydrogen balls. Total: {hydrogenCount}");
                break;
            case "fe":
                ironCount = amount;
                Debug.Log($"Added {amount} Iron balls. Total: {ironCount}");
                break;
            default:
                Debug.LogWarning("Unknown element: " + element);
                break;
        }
        detectBall.SpawnFakeBall();
    }

    // ��ֲy���ƶq
    public void RemoveBalls(string element, int amount)
    {
        switch (element.ToLower())
        {
            case "c":
                carbonCount -= amount;
                if (carbonCount < 0) carbonCount = 0;
                Debug.Log($"Removed {amount} Carbon balls. Total: {carbonCount}");
                break;
            case "n":
                nitrogenCount -= amount;
                if (nitrogenCount < 0) nitrogenCount = 0;
                Debug.Log($"Removed {amount} Nitrogen balls. Total: {nitrogenCount}");
                break;
            case "o":
                oxygenCount -= amount;
                if (oxygenCount < 0) oxygenCount = 0;
                Debug.Log($"Removed {amount} Oxygen balls. Total: {oxygenCount}");
                break;
            case "h":
                hydrogenCount -= amount;
                if (hydrogenCount < 0) hydrogenCount = 0;
                Debug.Log($"Removed {amount} Hydrogen balls. Total: {hydrogenCount}");
                break;
            case "fe":
                ironCount -= amount;
                if (ironCount < 0) ironCount = 0;
               Debug.Log($"Removed {amount} Iron balls. Total: {ironCount}");
                break;
            default:
                Debug.LogWarning("Unknown element: " + element);
                break;
        }
        detectBall.SpawnFakeBall();
    }

    // ������e�Y�ؤ������ƶq
    public int GetBallCount(string element)
    {
        switch (element.ToLower())
        {
            case "c": return carbonCount;
            case "n": return nitrogenCount;
            case "o": return oxygenCount;
            case "h": return hydrogenCount;
            case "fe": return ironCount;
            default:
                Debug.LogWarning("Unknown element: " + element);
                return 0;
        }
    }
}
