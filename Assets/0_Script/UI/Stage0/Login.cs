/*
 * 2025.3.1
 * 戴偉勝
 * 改為使用 內部xlsx來獲取資料
 * 使用者放置student.xlsx於Download資料夾下
 * 
 */
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml; // 引入 EPPlus 命名空間
using System.IO;
using System.ComponentModel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

public class Login : MonoBehaviour
{
    #region UI Elements and Variables
    [Header("Loading sign")]
    public GameObject Loading_sign;
    [Header("Login button")]
    public Button login_btn;
    [Header("Dropdowns")]
    public Dropdown DropdownCity;
    public Dropdown DropdownArea;
    public Dropdown DropdownSchool;
    public Dropdown DropdownYear;
    public Dropdown DropdownClass;
    public Dropdown DropdownStudent;
    [Header("UI Manager")]
    public MenuUIManager menuUIManager;
    // Google Apps Script 的 API URL
    //private string API_URL;
    private string sourcePath, filePath, logFilePath;
    private readonly string fileName = "student.xlsx";
    private readonly string logFileName = "log.txt";
    // 結構: 學校名稱 → 學年 → 班級 → 學生清單
    private readonly Dictionary<string, Dictionary<string, Dictionary<string, List<string[]>>>> schoolData = new();
    private readonly Dictionary<string, string> schoolIDMapping = new(); // 學校名稱對應 SchoolID
    private readonly Dictionary<string, string> studentIDMapping = new(); // 學生姓名對應 StudentID
    #endregion
    void Start()
    {   
        StartCoroutine(ReadExcel());
        DropdownCity.interactable = false; // 關閉城市選項
        DropdownArea.interactable = false; // 關閉區域選項
        // 設置下拉選單監聽
        DropdownSchool.onValueChanged.AddListener(delegate { PopulateYearDropdown(); });
        DropdownYear.onValueChanged.AddListener(delegate { PopulateClassDropdown(); });
        DropdownClass.onValueChanged.AddListener(delegate { PopulateStudentDropdown(); });
        DropdownStudent.onValueChanged.AddListener(delegate { DisplayStudentInfo(); });
        login_btn.onClick.AddListener(LoginButton);
    }
    private IEnumerator ReadExcel()
    {
#if UNITY_ANDROID
        sourcePath = Path.Combine(Application.streamingAssetsPath, fileName);
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        //string sdFilePath = "/sdcard/Download/" + fileName;
        logFilePath = Path.Combine(Application.persistentDataPath, logFileName);
#elif UNITY_STANDALONE_WIN
        filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        logFilePath = Path.Combine(Application.streamingAssetsPath, logFileName);
#endif  
        if (Application.platform == RuntimePlatform.Android && !File.Exists(filePath))
        {
            // Quest 2 (Android) 必須用 `UnityWebRequest` ，對應位置不存在時，複製檔案到指定位置
            using UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Get(sourcePath);
            yield return request.SendWebRequest();

            if (request.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {   
                File.AppendAllText(logFilePath, "讀取檔案失敗：" + request.error);
                yield break;
            }
            File.WriteAllBytes(filePath, request.downloadHandler.data);
        }
        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 設定 EPPlus 為非商業模式 
        File.AppendAllText(logFilePath, "\nReadExcel讀取檔案1 from:" + filePath);
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            if (package.Workbook.Worksheets.Count == 0)
            {
                File.AppendAllText(logFilePath, "Excel 文件沒有任何工作表！" + filePath);
                yield break;
            }
            ExcelWorksheet sheet = package.Workbook.Worksheets["工作表1"]; // 讀取工作表1
            int rowCount = sheet.Dimension.Rows; // 取得總行數
            File.AppendAllText(logFilePath, "\nReadExcel讀取檔案2-" + rowCount + "\nfrom:" + filePath);
            for (int i = 2; i <= rowCount; i++) // 跳過標題行（從第2行開始）
            {
                string schoolID = sheet.Cells[i, 1].Text.Trim();   // SchoolID
                string schoolName = sheet.Cells[i, 2].Text.Trim(); // SchoolName
                string year = sheet.Cells[i, 3].Text.Trim();       // Year (string)
                string classId = sheet.Cells[i, 4].Text.Trim();    // ClassId (string)
                string studentID = sheet.Cells[i, 5].Text.Trim();  // StudentID
                string studentName = sheet.Cells[i, 6].Text.Trim(); // StudentName

                // 存儲學校名稱與學校代碼對應關係
                if (!schoolIDMapping.ContainsKey(schoolName))
                    schoolIDMapping[schoolName] = schoolID;

                // 存儲學生姓名與學生代碼對應關係
                string studentKey = $"{studentName} ({studentID})";
                if (!studentIDMapping.ContainsKey(studentKey))
                    studentIDMapping[studentKey] = studentID;

                // 學校 → 學年 → 班級 → 學生
                if (!schoolData.ContainsKey(schoolName))
                    schoolData[schoolName] = new Dictionary<string, Dictionary<string, List<string[]>>>();

                if (!schoolData[schoolName].ContainsKey(year))
                    schoolData[schoolName][year] = new Dictionary<string, List<string[]>>();

                if (!schoolData[schoolName][year].ContainsKey(classId))
                    schoolData[schoolName][year][classId] = new List<string[]>();

                // 添加學生數據
                schoolData[schoolName][year][classId].Add(new string[] { schoolID, schoolName, year, classId, studentID, studentName });
                File.AppendAllText(logFilePath, "\nReadExcel讀取檔案3學生-" + studentName );
            }
        }
        PopulateSchoolDropdown();
        yield return null;
    }
    #region PopulateDropdowns
    void PopulateSchoolDropdown()
    {
        DropdownSchool.ClearOptions();
        DropdownSchool.AddOptions(new List<string>(schoolData.Keys));
        PopulateYearDropdown();
    }
    void PopulateYearDropdown()
    {
        DropdownYear.ClearOptions();
        string selectedSchool = DropdownSchool.options[DropdownSchool.value].text;
        if (schoolData.ContainsKey(selectedSchool))
        {
            DropdownYear.AddOptions(new List<string>(schoolData[selectedSchool].Keys));
        }
        PopulateClassDropdown();
    }

    void PopulateClassDropdown()
    {
        DropdownClass.ClearOptions();
        string selectedSchool = DropdownSchool.options[DropdownSchool.value].text;
        string selectedYear = DropdownYear.options[DropdownYear.value].text;

        if (schoolData.ContainsKey(selectedSchool) && schoolData[selectedSchool].ContainsKey(selectedYear))
        {
            DropdownClass.AddOptions(new List<string>(schoolData[selectedSchool][selectedYear].Keys));
        }
        PopulateStudentDropdown();
    }
    void PopulateStudentDropdown()
    {
        DropdownStudent.ClearOptions();
        string selectedSchool = DropdownSchool.options[DropdownSchool.value].text;
        string selectedYear = DropdownYear.options[DropdownYear.value].text;
        string selectedClass = DropdownClass.options[DropdownClass.value].text;

        if (schoolData.ContainsKey(selectedSchool) &&
            schoolData[selectedSchool].ContainsKey(selectedYear) &&
            schoolData[selectedSchool][selectedYear].ContainsKey(selectedClass))
        {
            List<string[]> students = schoolData[selectedSchool][selectedYear][selectedClass];
            List<string> studentOptions = new();

            foreach (var student in students)
            {
                studentOptions.Add($"{student[5]} ({student[4]})"); // 姓名 (學號)
            }

            DropdownStudent.AddOptions(studentOptions);
        }
    }
    #endregion
    #region keyboard login
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoginButton();
        }
    }
    #endregion
    #region UploadDataToGameManager
    void LoginButton()
    {
        if (DropdownStudent.options.Count == 0) return;
        Loading_sign.SetActive(true);
        StartCoroutine(UploadDataAndLogin());
    }
    void DisplayStudentInfo()
    {
        int studentIndex = DropdownStudent.value;
        string schoolName = DropdownSchool.options[DropdownSchool.value].text;
        string year = DropdownYear.options[DropdownYear.value].text;
        string classID = DropdownClass.options[DropdownClass.value].text;
        string[] selectedStudent = schoolData[schoolName][year][classID][studentIndex];
        string studentID = selectedStudent[4];
        string studentName = selectedStudent[5];
        Debug.Log($"學號：{studentID}-{studentName}");
    }
    IEnumerator UploadDataAndLogin()
    {
        string schoolName = DropdownSchool.options[DropdownSchool.value].text;
        string schoolID = schoolIDMapping.ContainsKey(schoolName) ? schoolIDMapping[schoolName] : "未知";
        string year = DropdownYear.options[DropdownYear.value].text;
        string classID = DropdownClass.options[DropdownClass.value].text;
        int studentIndex = DropdownStudent.value;

        if (schoolData.ContainsKey(schoolName) &&
            schoolData[schoolName].ContainsKey(year) &&
            schoolData[schoolName][year].ContainsKey(classID))
        {
            List<string[]> students = schoolData[schoolName][year][classID];

            if (studentIndex < students.Count)
            {
                string[] selectedStudent = schoolData[schoolName][year][classID][studentIndex];
                string studentID = selectedStudent[4];
                string studentName = selectedStudent[5];
                string TextResult = $"學校名稱: {schoolName}\n" +
                                  $"學校代碼: {schoolID}\n" +
                                  $"學年: {year}\n" +
                                  $"班級: {classID}\n" +
                                  $"學號: {studentID}\n" +
                                  $"姓名: {studentName}";
                Debug.Log(TextResult);
                // 用 GameManager方法來上傳資料
                UserDataManager.Instance.SetPlayerData(schoolID, classID, studentID, studentName, schoolName, year);
            }
        }
        // 等待一帧，確保資料已經處理完成
        yield return null;

        // 執行登入操作
        menuUIManager.Login();

        // 隱藏 LoadingUI
        Loading_sign.SetActive(false);
    }
    #endregion 
}