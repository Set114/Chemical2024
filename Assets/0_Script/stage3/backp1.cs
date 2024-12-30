using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class backp1 : MonoBehaviour
{
    public GameObject p1;        // The GameObject representing the initial UI (p1)
    public GameObject part0;        // The GameObject to hide when switching back to p1
    public GameObject ball;
    public GameObject show;
    public TextMeshProUGUI n2Text; // TextMeshPro component for N2 count
    public TextMeshProUGUI o2Text; // TextMeshPro component for O2 count
    public TextMeshProUGUI h2Text;
    public TextMeshProUGUI c2Text;
    public TextMeshProUGUI fe2Text;

    public calculateManager calcManager;

    // This method will be called when the button is pressed
    public void OnResetButtonClicked()
    {
        // Hide p2 and show p1
        if (part0 != null)
        {
            part0.SetActive(false);
        }
        if (p1 != null)
        {
            p1.SetActive(true);
        }
        if (ball != null)
        {
            ball.SetActive(false);
        }
        if (show != null)
        {
            show.SetActive(false);
        }

        // Reset all counters to zero
        ResetTextMeshProValue(n2Text);
        ResetTextMeshProValue(o2Text);
        ResetTextMeshProValue(c2Text);
        ResetTextMeshProValue(h2Text);
        ResetTextMeshProValue(fe2Text);

        if (calcManager != null)
        {
            calcManager.ResetAllCounts();
        }

        // Optionally, you can also reset any other game state or variables here
        HideAllSpheres();
    }

    private void ResetTextMeshProValue(TextMeshProUGUI textMeshPro)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = "0";
        }
    }

    private void HideAllSpheres()
    {
        // Find all GameObjects with "sphere" in their name
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("(sphere)"))
            {
                obj.SetActive(false);
                Debug.Log($"Hid {obj.name}");
            }
        }
    }
}
