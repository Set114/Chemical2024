using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour
{
    // Google Apps Script 的 API URL
    private string API_URL;
    //資料
    private string CityDataID = "";
    private string AreaDataID = "";
    private string SchoolDataID = "";
    private string ClassID = "";
    private string selectedStudentID = "";
    private string selectedStudentName = "";
    
    //中文資料
    private string CityData;
    private string AreaData;
    private string SchoolData;


    [Header("Loading")]
    public GameObject Loading_sign;
    [Header("Button")]
    [SerializeField] Button login_btn;
    [Header("SignUp")]
    public Dropdown cityDropdown;
    public Dropdown areaDropdown;
    public Dropdown schoolDropdown;
    [Header("Class Dropdowns")]
    public Dropdown yearDrop;
    public Dropdown classDrop;
    [Header("Student Dropdowns")]
    public Dropdown studentDropdown;


    // SheetsList 類別，用於解析 JSON 數據
    [System.Serializable]
    public class SheetsWrapper
    {
        public SheetsList sheets;
    }

    [System.Serializable]
    public class SheetsList
    {
        public List<string> sheets;
    }

    [System.Serializable]
    public class ClassDataListWrapper
    {
        public List<ClassData> classDataList;
    }
    // 學年資料
    private List<ClassData> classDataList = new List<ClassData>();
    // 用於存儲不重複的資料配對
    private Dictionary<string, string> cityData = new Dictionary<string, string>();
    private Dictionary<string, string> areaData = new Dictionary<string, string>();
    private Dictionary<string, string> schoolData = new Dictionary<string, string>();
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


    public MenuUIManager menuUIManager;
    public GameManager gameManager;

    void OnEnable()
    {
        StartCoroutine(GetCityData());
        // 設定下拉選單的選擇變更事件
        cityDropdown.onValueChanged.AddListener(delegate {
            CityDropdownValueChanged(cityDropdown);
        });
        areaDropdown.onValueChanged.AddListener(delegate {
            AreaDropdownValueChanged(areaDropdown);
        });
        schoolDropdown.onValueChanged.AddListener(delegate {
            SchoolDropdownValueChanged(schoolDropdown);
        });
        yearDrop.onValueChanged.AddListener(delegate {
            YearDropdownValueChanged(yearDrop);
        });
        classDrop.onValueChanged.AddListener(delegate {
            ClassDropdownValueChanged(classDrop);
        });
        studentDropdown.onValueChanged.AddListener(delegate {
            StudentDropdownValueChanged(studentDropdown);
        });
        login_btn.onClick.AddListener(LoginButton);
    }
    #region City
    // 獲取城市資料的協程// 先獲取所有 sheet 名稱
    public IEnumerator GetCityData()
    {
        Loading_sign.SetActive(true);
        API_URL = "https://script.google.com/macros/s/AKfycbwseWfABq5wX0WhdhbBmtHRJSS16VogH8NB3pcXWw7Xk7a1tVt5RE-XfGUUN70EhZusrA/exec";
        WWWForm form = new WWWForm();
        form.AddField("method", "cityListSheets");

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
                SheetsList sheetNames = null;

                try
                {
                    // check sheetname
                    // Debug.Log("{\"sheets\":" + responseText + "}");
                    SheetsWrapper sheetWrapper = JsonUtility.FromJson<SheetsWrapper>("{\"sheets\":" + responseText + "}");
                    sheetNames = sheetWrapper.sheets;
                }
                catch (System.Exception e)
                {
                    Debug.Log("JSON 解析錯誤: " + e.Message);
                    yield break;  // 結束協程，避免進一步錯誤
                }

                
                // 逐一讀取每個 sheet 的資料
                foreach (string sheetName in sheetNames.sheets)
                {
                    yield return StartCoroutine(ReadCityDataFromSheet(sheetName));
                }

                // 顯示結果
                // foreach (var entry in cityData)
                // {
                //     Debug.Log($"代號: {entry.Key}, 城市: {entry.Value}");
                // }
                
                CityFillDropdown();
                CityDropdownValueChanged(cityDropdown);
            }
        }
    }

    // 從單個 sheet 中讀取城市資料
    private IEnumerator ReadCityDataFromSheet(string sheetName)
    {
        API_URL = "https://script.google.com/macros/s/AKfycbwseWfABq5wX0WhdhbBmtHRJSS16VogH8NB3pcXWw7Xk7a1tVt5RE-XfGUUN70EhZusrA/exec";
        WWWForm form = new WWWForm();
        form.AddField("method", "cityReadSheet");
        form.AddField("sheetName", sheetName);

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
                // 假設返回的資料是以逗號分隔的字符串格式，如 "B,台中市"
                string[] data = responseText.Split(',');

                if (data.Length >= 2)
                {
                    string code = data[0].Trim();  // 代號
                    string city = data[1].Trim();  // 城市名稱
                    // check id and cityname is right
                    // Debug.Log("code" + code + "city" + city);

                    // 如果該代號尚未記錄過，則添加到字典中
                    if (!cityData.ContainsKey(code))
                    {
                        cityData[code] = city;
                    }
                }
            }
        }
    }

    private void CityFillDropdown()
    {
        cityDropdown.ClearOptions();

        // 從 cityData 字典中獲取所有城市名稱
        List<string> cityNames = cityData.Values.ToList();
        // 將城市名稱加入下拉選單選項中
        cityDropdown.AddOptions(cityNames);

    }

    private void CityDropdownValueChanged(Dropdown dropdown)
    {
        Loading_sign.SetActive(true);
        // 獲取選中的城市名稱
        string selectedCity = dropdown.options[dropdown.value].text;
        // 根據選中的城市名稱找到對應的代號
        string key = cityData.FirstOrDefault(x => x.Value == selectedCity).Key;
        // 顯示選中的城市及其代號
        // Debug.Log($"選擇的城市: {selectedCity}, 代號: {key}");
        CityDataID = key;
        CityData = selectedCity;
        StartCoroutine(GetAreaData());
    }

    #endregion
    #region Area

    private IEnumerator GetAreaData()
    {
        API_URL = "https://script.google.com/macros/s/AKfycbzbG9biT7taipSCbSLUVAbsyJna_40ekqC1d10KO1GQOUw9aMELpWZQ6oyH8fJ8quvSbg/exec";
        WWWForm form = new WWWForm();
        form.AddField("method", "areaReadSheet");
        form.AddField("cityData", CityDataID);

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
                // 手動解析 JSON 字符串
                areaData = ParseJsonToDictionary(responseText);

                // 顯示解析的結果
                // foreach (var entry in areaData)
                // {
                //     Debug.Log($"代號: {entry.Key}, 區域: {entry.Value}");
                // }
                AreaFillDropdown(areaData);
                AreaDropdownValueChanged(areaDropdown);
            }
        }
    }

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

    private void AreaFillDropdown(Dictionary<string, string> areaData)
    {
        areaDropdown.ClearOptions();
        List<string> areaNames = areaData.Values.ToList();
        areaDropdown.AddOptions(areaNames);
    }

    private void AreaDropdownValueChanged(Dropdown dropdown)
    {
        Loading_sign.SetActive(true);
        string selectedArea = dropdown.options[dropdown.value].text;
        string key = areaData.FirstOrDefault(x => x.Value == selectedArea).Key;  
        // Debug.Log($"選擇的區域: {selectedArea}, 代號: {key}");
        AreaDataID = key;
        AreaData = selectedArea;
        StartCoroutine(GetSchoolData());
    }

    #endregion
    #region School
    private IEnumerator GetSchoolData()
    {
        API_URL = "https://script.google.com/macros/s/AKfycbwy9hDMJHYrc5lmZ1mfU-R7oMvGFCwovC2gdGGqqVEYOnN13snWqYIMKQJHkDwBQlFtxA/exec";
        WWWForm form = new WWWForm();
        form.AddField("method", "schoolReadSheet");
        form.AddField("areaData", AreaDataID);

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
                // 手動解析 JSON 字符串
                schoolData = ParseJsonToDictionary(responseText);

                // 顯示解析的結果
                // foreach (var entry in schoolData)
                // {
                //     Debug.Log($"代號: {entry.Key}, 學校: {entry.Value}");
                // }
                SchoolFillDropdown(schoolData);
                SchoolDropdownValueChanged(schoolDropdown);
            }
        }
        
    }
    private void SchoolFillDropdown(Dictionary<string, string> schoolData)
    {
        schoolDropdown.ClearOptions();
        List<string> schoolsNames = schoolData.Values.ToList();
        schoolDropdown.AddOptions(schoolsNames);
    }

    private void SchoolDropdownValueChanged(Dropdown dropdown)
    {
        Loading_sign.SetActive(true);
        string selectedSchool = dropdown.options[dropdown.value].text;
        string key = schoolData.FirstOrDefault(x => x.Value == selectedSchool).Key;  // 修正代碼
        // Debug.Log($"選擇的學校: {selectedSchool}, 代號: {key}");
        SchoolDataID = key;
        SchoolData = selectedSchool;
        
        StartCoroutine(GetClassData());
    }
    #endregion
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
        // 獲取選擇的學年和班級
        string selectedYear = yearDrop.options[yearDrop.value].text.Split(' ')[0]; // 提取學年
        string selectedClass = dropdown.options[dropdown.value].text; // 選中的班級（格式為 "幾年 幾班"）

        // Debug.Log($"選擇的班級: {selectedClass} in year {selectedYear}");

        // 找到對應的 ClassID 或 ID
        if (classCodeByYearGrade.ContainsKey(selectedYear) && classCodeByYearGrade[selectedYear].ContainsKey(selectedClass))
        {
            ClassID = classCodeByYearGrade[selectedYear][selectedClass];
            // Debug.Log($"對應的班級代號 (Class ID): {ClassID}");
            // 在這裡執行後續邏輯，例如根據選中的班級 ID 進行操作
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
        studentDropdown.ClearOptions();
        List<string> studentNames = students.Values.ToList();
        studentDropdown.AddOptions(studentNames);
        Loading_sign.SetActive(false);
        StudentDropdownValueChanged(studentDropdown);
    }

    private void StudentDropdownValueChanged(Dropdown dropdown)
    {
        selectedStudentName = dropdown.options[dropdown.value].text;
        selectedStudentID = studentData.FirstOrDefault(x => x.Value == selectedStudentName).Key;

        // Debug.Log($"Selected Student: {selectedStudentName}, ID: {selectedStudentID}");
        
        Loading_sign.SetActive(false);
    }
    #endregion
    
    #region UploadDataToGameManager

    void LoginButton()
    {
        Loading_sign.SetActive(true);
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
        Loading_sign.SetActive(false);
    }

    void UploadDataToGameManager()
    {
        // 在這裡調用 GameManager 的 SetPlayerData 方法來上傳資料
        GameManager.Instance.SetPlayerData(SchoolDataID, ClassID, selectedStudentID, selectedStudentName);
    }
    #endregion
}
