using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AtomicDetection : MonoBehaviour
{
    public TextMeshProUGUI[] part0Texts;  // For part0
    public TextMeshProUGUI[] part1Texts;  // For part1
    public TextMeshProUGUI[] part2Texts;  // For part2
    public TextMeshProUGUI[] part3Texts;  // For part3
    public TextMeshProUGUI[] part4Texts;  // For part4
    public TextMeshProUGUI[] part5Texts;  // For part5

    public GameObject abc;
    public GameObject atomicDetection;
    public GameObject no2;
    public GameObject co2;
    public GameObject co22;
    public GameObject nh3;
    public GameObject h2o;
    public GameObject fe;
    public GameObject p0run1;
    public GameObject plane;
    public GameObject show;
    public AudioClip no222;
    private AudioSource audioSource;
    public GameObject part0;

    private int n2Count = 0;
    private int o2Count = 0;
    private int c2Count = 0;
    private int h2Count = 0;
    private int fe2Count = 0;
    private int allCount = 0;
    private string currentLevel = "";

    private Dictionary<string, int> sphereCounts = new Dictionary<string, int>()
    {
        { "N(sphere)", 0 },
        { "O(sphere)", 0 },
        { "C(sphere)", 0 },
        { "H(sphere)", 0 },
        { "Fe(sphere)", 0 }
    };

    private Dictionary<string, List<GameObject>> detectedSpheres = new Dictionary<string, List<GameObject>>()
    {
        { "N(sphere)", new List<GameObject>() },
        { "O(sphere)", new List<GameObject>() },
        { "C(sphere)", new List<GameObject>() },
        { "H(sphere)", new List<GameObject>() },
        { "Fe(sphere)", new List<GameObject>() }
    };

    
    public DataWrite dataWrite;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        currentLevel = GetCurrentLevel();

        // Debug.Log($"Trigger Entered by: {other.gameObject.name}");

        foreach (var key in detectedSpheres.Keys)
        {
            if (other.gameObject.name.Contains(key) && !detectedSpheres[key].Contains(other.gameObject))
            {
                detectedSpheres[key].Add(other.gameObject);
                sphereCounts[key]++;
                allCount++;
                // Debug.Log($"Added {other.gameObject.name} to detectedSpheres[{key}]");

                // Update the specific counters
                if (key == "N(sphere)")
                    n2Count++;
                else if (key == "O(sphere)")
                    o2Count++;
                else if (key == "C(sphere)")
                    c2Count++;
                else if (key == "H(sphere)")
                    h2Count++;
                else if (key == "Fe(sphere)")
                    fe2Count++;
                break;  // Exit the loop after the first match
            }
        }

        // Debug.Log($"N: {sphereCounts["N(sphere)"]} 、 O: {sphereCounts["O(sphere)"]} 、 C: {sphereCounts["C(sphere)"]} 、H: {sphereCounts["H(sphere)"]} 、Fe: {sphereCounts["Fe(sphere)"]}");

        // Update TextMeshPro based on the current level
        UpdateTextMeshPro();
    }

    private void UpdateTextMeshPro()
    {
        TextMeshProUGUI[] texts = GetCurrentLevelTexts();

        foreach (var textMesh in texts)
        {
            if (textMesh != null)
            {
                // Debug.Log($"Updating TextMeshPro for {textMesh.name}");
                if (textMesh.name.Contains("n2"))
                    textMesh.text = sphereCounts["N(sphere)"].ToString();
                else if (textMesh.name.Contains("o2"))
                    textMesh.text = sphereCounts["O(sphere)"].ToString();
                else if (textMesh.name.Contains("C2"))
                    textMesh.text = sphereCounts["C(sphere)"].ToString();
                else if (textMesh.name.Contains("H2"))
                    textMesh.text = sphereCounts["H(sphere)"].ToString();
                else if (textMesh.name.Contains("Fe2"))
                    textMesh.text = sphereCounts["Fe(sphere)"].ToString();
            }
        }

        // Check if the number of detected balls matches the required count
        if (allCount == GetRequiredBallCountForCurrentLevel())
        {
            HandleSphereDetection();
            // Debug.Log("");
        }
    }

    private int GetRequiredBallCountForCurrentLevel()
    {
        switch (currentLevel)
        {
            case "part0":
            case "part5":
                return 6;
            case "part1":
            case "part3":
                return 3;
            case "part2":
                return 4;
            case "part4":
                return 13;
            default:
                return 0;
        }
    }

    private TextMeshProUGUI[] GetCurrentLevelTexts()
    {
        switch (currentLevel)
        {
            case "part0":
                return part0Texts;
            case "part1":
                return part1Texts;
            case "part2":
                return part2Texts;
            case "part3":
                return part3Texts;
            case "part4":
                return part4Texts;
            case "part5":
                return part5Texts;
            default:
                return new TextMeshProUGUI[0]; // Return an empty array
        }
    }

    private void HandleSphereDetection()
    {
        if (allCount == GetRequiredBallCountForCurrentLevel())
        {
            // Debug.Log($"Checking conditions: N2: {n2Count}, O2: {o2Count}, C2: {c2Count}, H2: {h2Count}, Fe2: {fe2Count}, Level: {currentLevel}");

            if (currentLevel == "part0")
            {
                if (n2Count == 4 && o2Count == 2 && c2Count == 0 && h2Count == 0 && fe2Count == 0)
                {
                    ToggleObjects(false);
                    DisableSpheres();
                    no2.SetActive(true);
                    dataWrite.DataWriting();
                    StartCoroutine(ShowP0Run1AfterDelay(3f));
                }
                else
                {
                    ShowIncorrectSetup();
                }
            }
            // Other levels' checking logic can be added here
        }
    }

    private void ShowIncorrectSetup()
    {
        if (plane != null)
        {
            part0.SetActive(false);
            show.SetActive(true);
            if (audioSource != null && no222 != null)
            {
                audioSource.clip = no222;
                audioSource.Play();
            }
        }
    }

    private string GetCurrentLevel()
    {
        // Find all GameObjects with names "part1", "part2", etc.
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("part") && obj.activeInHierarchy)
            {
                // Debug.Log($"Current Level: {obj.name}");
                return obj.name;
            }
        }
        return "";
    }

    private void OnTriggerExit(Collider other)
    {
        TextMeshProUGUI[] texts = GetCurrentLevelTexts();

        if (other.gameObject.name.Contains("N(sphere)") && detectedSpheres["N(sphere)"].Contains(other.gameObject))
        {
            detectedSpheres["N(sphere)"].Remove(other.gameObject);
            sphereCounts["N(sphere)"] = Mathf.Max(0, sphereCounts["N(sphere)"] - 1); // Ensure count is not negative

            foreach (var textMesh in texts)
            {
                if (textMesh != null && textMesh.name.Contains("N2"))
                {
                    textMesh.text = sphereCounts["N(sphere)"].ToString();
                }
            }
        }
        else if (other.gameObject.name.Contains("O(sphere)") && detectedSpheres["O(sphere)"].Contains(other.gameObject))
        {
            detectedSpheres["O(sphere)"].Remove(other.gameObject);
            sphereCounts["O(sphere)"] = Mathf.Max(0, sphereCounts["O(sphere)"] - 1); // Ensure count is not negative

            foreach (var textMesh in texts)
            {
                if (textMesh != null && textMesh.name.Contains("O2"))
                {
                    textMesh.text = sphereCounts["O(sphere)"].ToString();
                }
            }
        }
    }

    private void ToggleObjects(bool state)
    {
        if (abc != null) abc.SetActive(state);
        if (atomicDetection != null) atomicDetection.SetActive(state);
    }

    private void DisableSpheres()
    {
        foreach (GameObject sphere in detectedSpheres["N(sphere)"])
        {
            if (sphere != null)
            {
                sphere.SetActive(false);
                // Debug.Log($"Deactivated sphere: {sphere.name}");
            }
        }
    }

    private IEnumerator ShowP0Run1AfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (p0run1 != null) p0run1.SetActive(true);
    }
}
