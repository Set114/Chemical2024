/*
 * 增加其他單元，以及教學操作的次數
 * 紀錄各單元的教學與測驗資料，並將資料寫入Excel檔案。
 *
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OfficeOpenXml;
using System;
using Unity.VisualScripting;

public class DataInDevice : MonoBehaviour
{
    private string filePath;
    public List<string>[] SaveData = new List<string>[12];
    private readonly string[] sSheetName = { "單元一 教學", "單元一 測驗", "單元二 教學", "單元二 測驗", 
        "單元三 教學", "單元三 測驗", "單元四 教學", "單元四 測驗", "單元五 教學", "單元五 測驗", "單元六 教學", "單元六 測驗"};
    private readonly int[] iSheetUnit = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
    private readonly int[] iSheetSet = { 6, 5, 3, 5, 1, 5, 5, 5, 5, 1, 3, 5 };
    private void Awake()
    {
        // 初始化並清空每個 List<string>
        for (int i = 0; i < SaveData.Length; i++)
        {
            SaveData[i] = new List<string>(); // 初始化 List
            SaveData[i].Clear(); // 清除 List (確保為空)
        }
    }    
    // Start is called before the first frame update
    void Start()
    {
        string DataFileName = DateTime.Now.ToString("yyyyMMdd") +".xlsx";
        #if PLATFORM_ANDROID
                filePath = Path.Combine(Application.persistentDataPath, DataFileName);
        #elif PLATFORM_STANDALONE_WIN
                filePath = Path.Combine(Application.streamingAssetsPath, DataFileName);
        #endif
        WriteExcel();
    }
    public void WriteExcel()
    {
        //直接創新的(會覆寫)
        // 確保允許非商業用途的授權 (EPPlus 5+ 需要) 
        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 建立或開啟 Excel 檔案 
        if (!File.Exists(filePath))
        {
            FileInfo fileInfo = new(filePath);
            using ExcelPackage package = new(fileInfo);
            //新增工作表，建立所有資料表的第一列
            for (int i = 0; i < 12; i++) 
                GetWorksheetContent(package.Workbook.Worksheets.Add(sSheetName[i]),
                    iSheetUnit[i], iSheetSet[i], i % 2 == 0);

            FileInfo file = new(filePath);
            package.SaveAs(file);
            Debug.Log($"Excel 檔案已儲存於: {filePath}");
        }
    }
    private void GetWorksheetContent(ExcelWorksheet worksheet, int iUnit, int iSets, bool isTeach)
    {
        // 教學與測驗除學號與姓名之外標題不同，教學有開始時間、結束時間、次數，測驗有得分、答案。
        int headerCount = isTeach ? iSets * 3 + 2 : iSets * 2 + 2;
        //  客製化單元三測驗格式
        if (!isTeach && iUnit == 3)
        {
            headerCount = iSets * 3 + 2;
        }

        for (int i = 0; i < headerCount; i++)
            worksheet.Cells[1, i + 1].Value = GetHeaders(iUnit, i, isTeach);
    }
    private string GetHeaders(int iUnit, int headerIndex, bool isTeach)
    {
        if (headerIndex == 0)            return "學號";
        else if (headerIndex == 1)       return "姓名";
        else if (isTeach)
        {
            int iQuotient = (headerIndex - 2) / 3 + 1;
            int iRemainder = (headerIndex - 2) % 3;
            if (iRemainder == 0) return $"{iUnit}-{iQuotient}開始";
            else if (iRemainder == 1) return $"{iUnit}-{iQuotient}結束";
            else
            {
                if (iUnit == 2)
                {
                    return $"{iUnit}-{iQuotient}操作錯誤次數";
                }
                else
                {
                    return $"{iUnit}-{iQuotient}次數";
                }
            }
        }
        else
        {
            if(iUnit == 3)
            {
                //  客製化單元三測驗格式
                int iQuotient = (headerIndex - 2) / 3 + 1;
                int iRemainder = (headerIndex - 2) % 3;
                if (iRemainder == 0) return $"{iUnit}-{iQuotient}開始";
                else if (iRemainder == 1) return $"{iUnit}-{iQuotient}結束";
                else return $"{iUnit}-{iQuotient}次數";
            }
            else
            {
                int iQuotient = headerIndex / 2;
                if (headerIndex % 2 == 1) return $"{iUnit}-{iQuotient}得分";
                else return $"{iUnit}-{iQuotient}答案";
            }
        }
    }
    //在其他單元中，呼叫這個函式以增加資料(資料用字串陣列傳入)
    public void AddDataExcel(int tabIndex)
    {
        print("save");
        if (!File.Exists(filePath))            return;

        Debug.Log($"Excel 檔案存在: {filePath}");
        // 設定授權（EPPlus 5+ 必需） 
        // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // 開啟現有 Excel 檔案 
        using ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
        ExcelWorksheet worksheet = package.Workbook.Worksheets[sSheetName[tabIndex]]; // 取得第幾個工作表
        if (worksheet == null)            return;

        // 增加資料
        Debug.Log(tabIndex + ":" + worksheet.Dimension.End.Row + "+" + worksheet.Dimension.End.Column);
        // 計算新欄位的索引 最後一列+1
        int newRow = worksheet.Dimension.End.Row + 1; // 新的列數，必須在最後一列的基礎上+1
        for (int col = 0; col <= worksheet.Dimension.End.Column - 1; col++) // 總共有多少資料要填寫：學校+班級+姓名... 
        {
            //確認 col 沒有超出 SaveData[tabIndex] 的長度
            if (SaveData[tabIndex] != null && col >= 0 && col < SaveData[tabIndex].Count)
            {
                if (SaveData[tabIndex][col] != null)
                    worksheet.Cells[newRow, col + 1].Value = SaveData[tabIndex][col]; // 填入資料
            }
        }
        // 保存變更 
        package.Save();
        Debug.Log($"已更新 Excel 檔案: {filePath}");
    }
}
