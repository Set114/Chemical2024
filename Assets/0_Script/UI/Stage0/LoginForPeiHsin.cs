using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
public class LoginForPeiHsin : MonoBehaviour
{
    [Header("LoadingUI")]
    [SerializeField] GameObject LoadingUI;
    [Header("Button")]
    [SerializeField] Button login_btn;
    [Header("LogInDrop")]
    [SerializeField] Dropdown city_drop;
    [SerializeField] Dropdown region_drop;
    [SerializeField] Dropdown school_drop;
    [SerializeField] Dropdown yearDrop;
    [SerializeField] Dropdown classDrop;
    [SerializeField] Dropdown studentDrop;
    //DATA
    private string cityData = "";
    private string regionData = "";
    private string schoolData = "";
    private string yearData = "";
    private string classData = "";
    private string studentNameData = "";
    private string studentIDData = "";
    
    private string SchoolDataID = "";
    private string ClassID = "";
    private string selectedStudentID = "";
    private string selectedStudentName = "";

    // Google Apps Script 的 API URL
    private string API_URL;

    // 學年資料
    private List<ClassData> classDataList = new List<ClassData>();
    // 學年對應的班級資料
    private Dictionary<string, List<string>> classesByYear = new Dictionary<string, List<string>>();
    // 學年和班級對應的 classCode（ID）
    private Dictionary<string, Dictionary<string, string>> classCodeByYearGrade = new Dictionary<string, Dictionary<string, string>>();
    private Dictionary<string, string> studentData = new Dictionary<string, string>();

    [System.Serializable]
    public class ClassData
    {
        public string year;
        public string grade;
        public string className;
        public string classCode;
    }

    [System.Serializable]
    public class ClassDataListWrapper
    {
        public List<ClassData> classDataList;
    }


    public MenuUIManager menuUIManager;
    public GameManager gameManager;

    void Start()
    {
        // 初始化所有 Dropdown
        InitializeDropdown(city_drop);
        InitializeDropdown(region_drop);
        InitializeDropdown(school_drop);
        InitializeDropdown(yearDrop);
        InitializeDropdown(classDrop);
        InitializeDropdown(studentDrop);

        yearDrop.onValueChanged.AddListener(delegate {
            YearDropdownValueChanged(yearDrop);
        });
        classDrop.onValueChanged.AddListener(delegate {
            ClassDropdownValueChanged(classDrop);
        });
        studentDrop.onValueChanged.AddListener(delegate {
            StudentDropdownValueChanged(studentDrop);
        });
        LoadingUI.SetActive(true);
        // 設置初始值
        SetDropdownValue(city_drop, new List<string> { "台中市" });
        cityData = "台中市";

        SetRegionDropdownValue();

        login_btn.onClick.AddListener(LoginButton);

    }
    // 初始化 Dropdown，將選項設置為空
    void InitializeDropdown(Dropdown dropdown)
    {
        dropdown.ClearOptions();
        List<string> emptyOptions = new List<string> { "" };
        dropdown.AddOptions(emptyOptions);
    }

    void SetDropdownValue(Dropdown dropdown, List<string> values)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(values);
        dropdown.value = 0; // 設置第一個選項為選中值

        // 根據 dropdown 來設置對應的 data
        if (dropdown == city_drop)
        {
            cityData = values[0];
        }
        else if (dropdown == region_drop)
        {
            regionData = values[0];
        }
        else if (dropdown == school_drop)
        {
            schoolData = values[0];
        }
        // else if (dropdown == year_drop)
        // {
        //     yearData = values[0];
        // }
        // else if (dropdown == classDrop)
        // {
        //     classData = values[0];
        // }
        // else if (dropdown == studentID_drop)
        // {
        //     if (dropdown.options.Count > 1) // 檢查是否有選項
        //     {
        //         studentNameData = values[0]; // 記錄選擇的學生姓名
        //         UpdateStudentIDData(studentID_drop.value); // 更新學生ID資料
        //     }
        // }

    }
    // 設置區域 Dropdown 的初始值
    void SetRegionDropdownValue()
    {
        SetDropdownValue(region_drop, new List<string> { "北屯區" });
        regionData = "北屯區";
        SetSchoolDropdownValue();
    }

    // 設置學校 Dropdown 的初始值
    void SetSchoolDropdownValue()
    {
        SetDropdownValue(school_drop, new List<string> { "北新國中" });
        schoolData = "北新國中";
        SchoolDataID = "193506";
        StartCoroutine(GetClassData());
        // SetYearDropdownValue();
    }
    #region Class
    private IEnumerator GetClassData()
    {
        //class
        API_URL = "https://script.google.com/macros/s/AKfycbw24_cKIaIOlRuosTJPcJueQVJumqXC1pWyNpGK-ekBBIUE8Df2O1HgYysR_LkTUmZj/exec";
        WWWForm form = new WWWForm();
        form.AddField("method", "findSchoolClass");
        form.AddField("schoolID", SchoolDataID);
        // form.AddField("schoolID", "173510");

        using (UnityWebRequest www = UnityWebRequest.Post(API_URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                // 處理返回的資料
                // Debug.Log("檢查結果: " + responseText);
                ClassParseJsonData(responseText);
            }
        }
    }
        

    void ClassParseJsonData(string jsonData)
    {
        // Debug.Log($"Raw JSON Data: {jsonData}");

        // 使用 JsonUtility 解析 JSON 字符串
        ClassDataListWrapper wrapper = JsonUtility.FromJson<ClassDataListWrapper>("{\"classDataList\":" + jsonData + "}");

        // 確保列表不為空
        if (wrapper != null && wrapper.classDataList != null)
        {
            classDataList = wrapper.classDataList;

            // 清空現有的選項
            yearDrop.ClearOptions();
            classDrop.ClearOptions();

            // 用來存儲唯一的學年和班級資料
            List<string> yearsList = new List<string>();
            classesByYear.Clear();
            classCodeByYearGrade.Clear(); // 用於存儲 year, grade 和 class 對應的 ID

            foreach (ClassData data in classDataList)
            {
                // Debug.Log($"Year: {data.year}, Grade: {data.grade}, Class: {data.className}, Class Code: {data.classCode}");

                // 添加學年到 yearsList，顯示格式： "學年 (Year)"
                string yearDisplay = $"{data.year} 學年";
                if (!yearsList.Contains(yearDisplay))
                {
                    yearsList.Add(yearDisplay);
                }

                string classInfo = $"{data.grade}年 {data.className}班";
                if (!classesByYear.ContainsKey(data.year))
                {
                    classesByYear[data.year] = new List<string>();
                }
                if (!classesByYear[data.year].Contains(classInfo))
                {
                    classesByYear[data.year].Add(classInfo);
                }

                // 儲存 year, grade, class 對應的 classCode
                if (!classCodeByYearGrade.ContainsKey(data.year))
                {
                    classCodeByYearGrade[data.year] = new Dictionary<string, string>();
                }

                string gradeClassKey = $"{data.grade}年 {data.className}班";
                classCodeByYearGrade[data.year][gradeClassKey] = data.classCode;
            }

            // 將學年資料填充到 yearDrop
            yearDrop.AddOptions(yearsList);

            // 預設選擇第一個學年
            if (yearsList.Count > 0)
            {
                yearDrop.value = 0;
                YearDropdownValueChanged(yearDrop);
            }
        }
        else
        {
            Debug.LogError("Failed to parse JSON data.");
        }
    }
            
    private void YearDropdownValueChanged(Dropdown dropdown)
    {
        LoadingUI.SetActive(true);
        string selectedYear = dropdown.options[dropdown.value].text.Split(' ')[0]; // 提取學年
        // Debug.Log($"選擇的學年: {selectedYear}");

        // 更新 classDrop 選項
        if (classesByYear.ContainsKey(selectedYear))
        {
            List<string> classList = classesByYear[selectedYear];
            classDrop.ClearOptions();
            classDrop.AddOptions(classList);

            // 預設選擇第一個班級
            if (classList.Count > 0)
            {
                classDrop.value = 0;
                ClassDropdownValueChanged(classDrop);
            }
        }
        else
        {
            classDrop.ClearOptions();
        }
    }
    
    private void ClassDropdownValueChanged(Dropdown dropdown)
    {
        LoadingUI.SetActive(true);
        // 獲取選擇的學年和班級
        string selectedYear = yearDrop.options[yearDrop.value].text.Split(' ')[0]; // 提取學年
        string selectedClass = dropdown.options[dropdown.value].text; // 選中的班級（格式為 "幾年 幾班"）

        // Debug.Log($"選擇的班級: {selectedClass} in year {selectedYear}");

        // 找到對應的 ClassID 或 ID
        if (classCodeByYearGrade.ContainsKey(selectedYear) && classCodeByYearGrade[selectedYear].ContainsKey(selectedClass))
        {
            ClassID = classCodeByYearGrade[selectedYear][selectedClass];
        }
        StartCoroutine(showStudentsData());
    }
    #endregion

    #region Student
    IEnumerator showStudentsData()
    {
        string folderName = SchoolDataID;
        string fileName = ClassID + "_Student";
        string classDataID = ClassID;
        // Debug.Log("檢查" + folderName);

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
                // Debug.Log("Received data: " + responseText);

                // Parse the JSON into the dictionary
                studentData = StudentParseJsonToDictionary(responseText);

                // Fill dropdown with student names
                FillStudentDropdown(studentData);
            }
        }
    }

    private Dictionary<string, string> StudentParseJsonToDictionary(string json)
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

    private void FillStudentDropdown(Dictionary<string, string> students)
    {
        studentDrop.ClearOptions();
        List<string> studentNames = students.Values.ToList();
        studentDrop.AddOptions(studentNames);
        LoadingUI.SetActive(false);
        StudentDropdownValueChanged(studentDrop);
    }

    private void StudentDropdownValueChanged(Dropdown dropdown)
    {
        LoadingUI.SetActive(true);
        selectedStudentName = dropdown.options[dropdown.value].text;
        selectedStudentID = studentData.FirstOrDefault(x => x.Value == selectedStudentName).Key;

        // Debug.Log($"Selected Student: {selectedStudentName}, ID: {selectedStudentID}");
        
        LoadingUI.SetActive(false);
    }
    #endregion    
    #region UploadDataToGameManager

    void LoginButton()
    {
        LoadingUI.SetActive(true);
        StartCoroutine(UploadDataAndLogin());
    }

    IEnumerator UploadDataAndLogin()
    {
        // 上傳資料到 GameManager
        UploadDataToGameManager();

        // 等待一帧，確保資料已經處理完成
        yield return null;

        // 執行登入操作
        menuUIManager.Login();

        // 隱藏 LoadingUI
        LoadingUI.SetActive(false);
    }

    void UploadDataToGameManager()
    {
        // 在這裡調用 GameManager 的 SetPlayerData 方法來上傳資料
        GameManager.Instance.SetPlayerData(SchoolDataID, ClassID, selectedStudentID, selectedStudentName);
    }
    #endregion
}
