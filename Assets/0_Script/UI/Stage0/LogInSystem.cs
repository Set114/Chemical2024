using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInSystem : MonoBehaviour
{
    [Header("LoadingUI")]
    [SerializeField] GameObject LoadingUI;
    [Header("Button")]
    [SerializeField] Button login_btn;
    [Header("LogInDrop")]
    [SerializeField] Dropdown city_drop;
    [SerializeField] Dropdown region_drop;
    [SerializeField] Dropdown school_drop;
    [SerializeField] Dropdown year_drop;
    [SerializeField] Dropdown class_drop;
    [SerializeField] Dropdown studentID_drop;
    //DATA
    private string cityData = "";
    private string regionData = "";
    private string schoolData = "";
    private string yearData = "";
    private string classData = "";
    private string studentNameData = "";
    private string studentIDData = "";

    public MenuUIManager menuUIManager;
    public GameManager gameManager;

    // 預設的學生ID選項
    private Dictionary<string, List<string>> classToStudents = new Dictionary<string, List<string>>()
    {
        // { "一年一班", new List<string> { "王強", "偉杰"} },
        // { "二年一班", new List<string> { "林小美", "宋依雨" } }
        { "一年一班", new List<string> { "王強", "偉杰"} }
    };

    // 學生ID對應表
    private Dictionary<string, string> studentData = new Dictionary<string, string>()
    {
        { "王強", "a1110831001" },
        { "偉杰", "a1110831002" }
        // { "偉杰", "a1110831012" },
        // { "宋依雨", "a1110831007" }
    };

    // Start 在第一次 frame 更新之前被調用
    void Start()
    {
        // 初始化所有 Dropdown
        InitializeDropdown(city_drop);
        InitializeDropdown(region_drop);
        InitializeDropdown(school_drop);
        InitializeDropdown(year_drop);
        InitializeDropdown(class_drop);
        InitializeDropdown(studentID_drop);
        
        // 設置初始值
        SetDropdownValue(city_drop, new List<string> { "基隆市" });
        cityData = "基隆市";
        SetRegionDropdownValue();

        // 設置班級 Dropdown 的初始值
        SetClassDropdownValue();

        // 顯示所有 data
        DisplayAllData();
        
        login_btn.onClick.AddListener(Login);

        // 添加學生ID Dropdown 的值改變事件監聽
        studentID_drop.onValueChanged.AddListener(delegate {
            UpdateStudentIDData(studentID_drop.value);
            DisplayAllData();
        });
    }

    // 初始化 Dropdown，將選項設置為空
    void InitializeDropdown(Dropdown dropdown)
    {
        dropdown.ClearOptions();
        List<string> emptyOptions = new List<string> { "" };
        dropdown.AddOptions(emptyOptions);
    }

    // 設置 Dropdown 的初始值
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
        else if (dropdown == year_drop)
        {
            yearData = values[0];
        }
        else if (dropdown == class_drop)
        {
            classData = values[0];
        }
        else if (dropdown == studentID_drop)
        {
            if (dropdown.options.Count > 1) // 檢查是否有選項
            {
                studentNameData = values[0]; // 記錄選擇的學生姓名
                UpdateStudentIDData(studentID_drop.value); // 更新學生ID資料
            }
        }
    }

    // 設置區域 Dropdown 的初始值
    void SetRegionDropdownValue()
    {
        SetDropdownValue(region_drop, new List<string> { "中正區" });
        regionData = "中正區";
        SetSchoolDropdownValue();
    }

    // 設置學校 Dropdown 的初始值
    void SetSchoolDropdownValue()
    {
        SetDropdownValue(school_drop, new List<string> { "正濱國中" });
        schoolData = "正濱國中";
        SetYearDropdownValue();
    }

    // 設置學年 Dropdown 的初始值
    void SetYearDropdownValue()
    {
        List<string> yearOptions = new List<string> { "111學年" };
        yearData = yearOptions[0]; // 預設為第一個選項
        SetDropdownValue(year_drop, yearOptions);

        // 添加學年 Dropdown 的值改變事件監聽
        year_drop.onValueChanged.AddListener(delegate {
            OnYearDropdownValueChanged(year_drop.value);
        });
    }

    // 當學年 Dropdown 的值改變時觸發
    void OnYearDropdownValueChanged(int index)
    {
        if (index >= 0 && index < year_drop.options.Count)
        {
            yearData = year_drop.options[index].text;
            DisplayAllData();
        }
    }

    // 設置班級 Dropdown 的初始值，並添加事件監聽
    void SetClassDropdownValue()
    {
        // List<string> classOptions = new List<string> { "一年一班", "二年一班" };
        List<string> classOptions = new List<string> { "一年一班" };
        classData = classOptions[0]; // 預設為第一個選項
        SetDropdownValue(class_drop, classOptions);

        // 添加班級 Dropdown 的值改變事件監聽
        class_drop.onValueChanged.AddListener(delegate {
            OnClassDropdownValueChanged(class_drop.value);
        });

        // 初始化學生ID Dropdown
        OnClassDropdownValueChanged(class_drop.value);
    }

    // 當班級 Dropdown 的值改變時觸發
    void OnClassDropdownValueChanged(int index)
    {
        if (index >= 0 && index < class_drop.options.Count)
        {
            classData = class_drop.options[index].text;
            if (classToStudents.ContainsKey(classData))
            {
                SetDropdownValue(studentID_drop, classToStudents[classData]);
                UpdateStudentIDData(studentID_drop.value);
                DisplayAllData();
            }
        }
    }

    // 更新學生ID Data
    void UpdateStudentIDData(int index)
    {
        if (index >= 0 && index < studentID_drop.options.Count)
        {
            string studentName = studentID_drop.options[index].text;
            if (studentData.ContainsKey(studentName))
            {
                studentIDData = studentData[studentName];
                studentNameData = studentName; // 更新選擇的學生姓名
            }
        }
    }

    // 顯示所有 data
    void DisplayAllData()
    {
        // Debug.Log("City: " + cityData);
        // Debug.Log("Region: " + regionData);
        // Debug.Log("School: " + schoolData);
        // Debug.Log("Year: " + yearData);
        // Debug.Log("Class: " + classData);
        // Debug.Log("Student Name: " + studentNameData);
        // Debug.Log("Student ID: " + studentIDData);
    }

    void Login()
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
        
        string schoolId = "173510";
        string classId = "173510_0001";
        GameManager.Instance.SetPlayerData(schoolId, classId, studentIDData, studentNameData);
    }
}
