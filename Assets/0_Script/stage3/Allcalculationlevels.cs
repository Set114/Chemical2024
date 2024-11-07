using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Allcalculationlevels : MonoBehaviour
{
    public TMP_Text[] atoms;
    public GameObject[] objectPrefabs; // Array to hold the 5 prefabs
    public string objectName;

    private int objectCount = 0;
    private string currentElement = "";
    private Vector3 lastGrabPosition;
    private XRGrabInteractable[] grabInteractables;

    private Dictionary<string, int> elementCounts = new Dictionary<string, int>()
    {
        { "C", 0 },
        { "N", 0 },
        { "O", 0 },
        { "H", 0 },
        { "Fe", 0 }
    };

    public void GetCounts(int cCount, int nCount, int oCount, int hCount, int feCount)
    {
        elementCounts["C"] = cCount;
        elementCounts["N"] = nCount;
        elementCounts["O"] = oCount;
        elementCounts["H"] = hCount;
        elementCounts["Fe"] = feCount;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (atoms != null && atoms.Length >= 5)
        {
            if (atoms[0] != null) atoms[0].text = elementCounts["C"].ToString();
            if (atoms[1] != null) atoms[1].text = elementCounts["N"].ToString();
            if (atoms[2] != null) atoms[2].text = elementCounts["O"].ToString();
            if (atoms[3] != null) atoms[3].text = elementCounts["H"].ToString();
            if (atoms[4] != null) atoms[4].text = elementCounts["Fe"].ToString();
        }
        else
        {
            Debug.LogError("Atoms array is not initialized properly or does not have enough elements.");
        }
    }

    private void DecrementCount(string elementType)
    {
        if (elementCounts.ContainsKey(elementType) && elementCounts[elementType] > 0)
        {
            elementCounts[elementType]--;
        }

       //Debug.Log($"After decrement: C = {elementCounts["C"]}, N = {elementCounts["N"]}, O = {elementCounts["O"]}, H = {elementCounts["H"]}, Fe = {elementCounts["Fe"]}");
        UpdateDisplay();
    }

    void Start()
    {
        grabInteractables = FindObjectsOfType<XRGrabInteractable>();
        if (grabInteractables.Length > 0)
        {
            foreach (var grabInteractable in grabInteractables)
            {
                grabInteractable.selectEntered.RemoveListener(OnGrab);
                grabInteractable.selectEntered.AddListener(OnGrab);

                grabInteractable.selectExited.RemoveListener(OnRelease);
                grabInteractable.selectExited.AddListener(OnRelease);
                Debug.Log(grabInteractable);
            }
        }
        else
        {
            Debug.LogError("No XRGrabInteractable components found.");
        }

        if (objectPrefabs == null || objectPrefabs.Length != 5)
        {
            Debug.LogError("objectPrefabs is not assigned or does not have 5 elements.");
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        objectName = args.interactableObject.transform.name;
        //Debug.Log("Grabbed object: " + objectName);

        lastGrabPosition = args.interactableObject.transform.position;

        if (objectName.Contains("C(sphere)")) currentElement = "C";
        else if (objectName.Contains("N(sphere)")) currentElement = "N";
        else if (objectName.Contains("O(sphere)")) currentElement = "O";
        else if (objectName.Contains("H(sphere)")) currentElement = "H";
        else if (objectName.Contains("Fe(sphere)")) currentElement = "Fe";
        else currentElement = "";
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (string.IsNullOrEmpty(currentElement))
        {
            Debug.LogWarning("No element type selected.");
            return;
        }

        int elementCount = GetElementCount(currentElement);
        if (elementCount > 1)
        {
            DecrementCount(currentElement);
            GenerateObject(lastGrabPosition, currentElement);
        }
        else if (elementCount == 1)
        {
            DecrementCount(currentElement);
        }
        else
        {
            Debug.LogWarning($"Element count for {currentElement} is 0 or less.");
        }

        currentElement = "";
    }

    private int GetElementCount(string elementType)
    {
        if (elementCounts.ContainsKey(elementType))
        {
            return elementCounts[elementType];
        }
        return 0;
    }

    void GenerateObject(Vector3 position, string elementType)
    {
        if (objectPrefabs == null || objectPrefabs.Length != 5)
        {
            Debug.LogError("objectPrefabs is not assigned or does not have 5 elements.");
            return;
        }

        GameObject prefab = GetPrefabForElement(elementType);
        if (prefab == null)
        {
            Debug.LogError($"Prefab for element {elementType} is not found.");
            return;
        }

        GameObject spawnedObject = Instantiate(prefab, position, Quaternion.identity);
        Debug.Log(prefab);

        spawnedObject.name = $"{elementType}(sphere)_{elementType}_{objectCount}";

        XRGrabInteractable grabInteractable = spawnedObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = spawnedObject.AddComponent<XRGrabInteractable>();
        }

        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectEntered.AddListener(OnGrab);

        grabInteractable.selectExited.RemoveListener(OnRelease);
        grabInteractable.selectExited.AddListener(OnRelease);

        //Debug.Log(grabInteractable);
        

       // Debug.Log($"Generated object with name: {spawnedObject.name} at position: {position}");

        objectCount++;
    }

    private GameObject GetPrefabForElement(string elementType)
    {
        switch (elementType)
        {
            case "C":
                return objectPrefabs[0];
            case "N":
                return objectPrefabs[1];
            case "O":
                return objectPrefabs[2];
            case "H":
                return objectPrefabs[3];
            case "Fe":
                return objectPrefabs[4];
            default:
                return null;
        }
    }
}
