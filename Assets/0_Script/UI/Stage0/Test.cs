using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Test : MonoBehaviour
{
    // Student Dropdown
    public Dropdown studentDropdown;

    // Dictionary to store student ID and name
    private Dictionary<string, string> studentData = new Dictionary<string, string>();
    private string selectedStudentID;

    void OnEnable()
    {
        StartCoroutine(showStudentsData());
        studentDropdown.onValueChanged.AddListener(delegate {
            StudentDropdownValueChanged(studentDropdown);
        });
    }

    IEnumerator showStudentsData()
    {
        // Test data
        string folderName = "173510";
        string fileName = "173510_0001" + "_Student";
        string classDataID = "173510_0001";

        // Form to send to Google Apps Script
        WWWForm form = new WWWForm();
        form.AddField("method", "readStuBtnData");
        form.AddField("folderName", folderName);
        form.AddField("fileName", fileName);
        form.AddField("classDataID", classDataID);

        string url = "https://script.google.com/macros/s/AKfycbySw3nsep50fZXgUzkwgXckAx23qFMlgwtadLh-jpYsx2_jpT5tvWQZFsIdzKyqiZ4g/exec";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Network or HTTP error: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Received data: " + responseText);

                // Parse the JSON into the dictionary
                studentData = ParseJsonToDictionary(responseText);

                // Fill dropdown with student names
                FillStudentDropdown(studentData);
            }
        }
    }

    // Method to parse JSON response into a dictionary
    private Dictionary<string, string> ParseJsonToDictionary(string json)
    {
        var dict = new Dictionary<string, string>();
        string pattern = "\"([^\"]+)\":\"([^\"]+)\"";
        MatchCollection matches = Regex.Matches(json, pattern);

        foreach (Match match in matches)
        {
            string key = match.Groups[1].Value;
            string value = match.Groups[2].Value;
            dict[key] = value;
        }

        return dict;
    }

    // Method to fill the dropdown with student names
    private void FillStudentDropdown(Dictionary<string, string> students)
    {
        studentDropdown.ClearOptions();
        List<string> studentNames = students.Values.ToList();
        studentDropdown.AddOptions(studentNames);
    }

    // Handle student dropdown value change
    private void StudentDropdownValueChanged(Dropdown dropdown)
    {
        string selectedStudentName = dropdown.options[dropdown.value].text;
        selectedStudentID = studentData.FirstOrDefault(x => x.Value == selectedStudentName).Key;

        Debug.Log($"Selected Student: {selectedStudentName}, ID: {selectedStudentID}");
    }
}
