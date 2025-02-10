using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OfficeOpenXml;
using System;

public class DataInDevice : MonoBehaviour
{
    string DataPath;
    public List<string> SaveData4Teach;
    public List<string> SaveData4Test;
    public List<string> SaveData6Teach;
    public List<string> SaveData6Test;

    private void Awake()
    {
        Clear();
    }
    public void Clear() {
        SaveData4Teach.Clear();
        SaveData4Test.Clear();
        SaveData6Teach.Clear();
        SaveData6Test.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        DataPath = DateTime.Now.ToString("yyyyMMdd") +".xlsx";

        WriteExcel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteExcel()
    {
#if PLATFORM_ANDROID
string P = Application.persistentDataPath;
#elif PLATFORM_STANDALONE_WIN
        string P = Application.streamingAssetsPath;
#endif
        //直接創新的(會覆寫)
        string filePath = Path.Combine(P, DataPath); 
        // 確保允許非商業用途的授權 (EPPlus 5+ 需要) 
        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 建立或開啟 Excel 檔案 
        if (!File.Exists(filePath))
        {
            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                //新增工作表 
                //建立所有資料表的第一列
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("單元四 教學"); // 寫入資料 
                worksheet.Cells[1, 1].Value = "學號";
                worksheet.Cells[1, 2].Value = "姓名";
                worksheet.Cells[1, 3].Value = "4-1開始時間";
                worksheet.Cells[1, 4].Value = "4-1結束時間";
                worksheet.Cells[1, 5].Value = "4-2開始時間";
                worksheet.Cells[1, 6].Value = "4-2結束時間";
                worksheet.Cells[1, 7].Value = "4-3開始時間";
                worksheet.Cells[1, 8].Value = "4-3結束時間";
                worksheet.Cells[1, 9].Value = "4-4開始時間";
                worksheet.Cells[1, 10].Value = "4-4結束時間";
                worksheet.Cells[1, 11].Value = "4-5開始時間";
                worksheet.Cells[1, 12].Value = "4-5結束時間";

                ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("單元四 測驗"); // 寫入資料 
                worksheet2.Cells[1, 1].Value = "學號";
                worksheet2.Cells[1, 2].Value = "姓名";
                worksheet2.Cells[1, 3].Value = "第1題答案";
                worksheet2.Cells[1, 4].Value = "第2題答案";
                worksheet2.Cells[1, 5].Value = "第3題答案";
                worksheet2.Cells[1, 6].Value = "第4題答案";
                worksheet2.Cells[1, 7].Value = "第5題答案";

                ExcelWorksheet worksheet3 = package.Workbook.Worksheets.Add("單元六 教學"); // 寫入資料 
                worksheet3.Cells[1, 1].Value = "學號";
                worksheet3.Cells[1, 2].Value = "姓名";
                worksheet3.Cells[1, 3].Value = "6-1開始時間";
                worksheet3.Cells[1, 4].Value = "6-1結束時間";
                worksheet3.Cells[1, 5].Value = "6-2開始時間";
                worksheet3.Cells[1, 6].Value = "6-2結束時間";
                worksheet3.Cells[1, 7].Value = "6-3開始時間";
                worksheet3.Cells[1, 8].Value = "6-3結束時間";

                ExcelWorksheet worksheet4 = package.Workbook.Worksheets.Add("單元六 測驗"); // 寫入資料 
                worksheet4.Cells[1, 1].Value = "學號";
                worksheet4.Cells[1, 2].Value = "姓名";
                worksheet4.Cells[1, 3].Value = "第1題答案";
                worksheet4.Cells[1, 4].Value = "第2題答案";
                worksheet4.Cells[1, 5].Value = "第3題答案";
                worksheet4.Cells[1, 6].Value = "第4題答案";
                worksheet4.Cells[1, 7].Value = "第5題答案";

                FileInfo file = new FileInfo(filePath);
                package.SaveAs(file);
                Debug.Log($"Excel 檔案已儲存於: {filePath}");
            }
        }
    }

    //在其他單元中，呼叫這個函式以增加資料(資料用字串陣列傳入)
    public void AddDataTeach(int Unit)
    {
#if PLATFORM_ANDROID
string P = Application.persistentDataPath;
#elif PLATFORM_STANDALONE_WIN
        string P = Application.streamingAssetsPath;
#endif
        // 設定檔案路徑 
        string filePath = Path.Combine(P, DataPath);
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Excel 檔案不存在: {filePath}");
            return;
        }
        // 設定授權（EPPlus 5+ 必需） 
        // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // 開啟現有 Excel 檔案 
        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelWorksheet worksheet = null;
            switch (Unit)
            {
                case 0:
                     worksheet = package.Workbook.Worksheets["單元四 教學"]; // 取得第幾個工作表 
                    break;
                case 1:
                     worksheet = package.Workbook.Worksheets["單元四 測驗"]; // 取得第幾個工作表 
                    break;
                case 2:
                     worksheet = package.Workbook.Worksheets["單元六 教學"]; // 取得第幾個工作表 
                    break;
                case 3:
                     worksheet = package.Workbook.Worksheets["單元六 測驗"]; // 取得第幾個工作表 
                    break;
            }
            if (worksheet == null)
            {
                Debug.LogError("無法找到工作表！");
                return;
            }
            // 增加資料
            Debug.Log(Unit + ":" + worksheet.Dimension.End.Row + "+"+worksheet.Dimension.End.Column);
            int newRowIndex = worksheet.Dimension.End.Row + 1; // 計算新欄位的索引 最後一列+1
            for (int col = 0; col <= worksheet.Dimension.End.Column-1; col++) // 總共有多少資料要填寫：學校+班級+姓名... 
            {
                switch (Unit) {
                    case 0:
                        worksheet.Cells[newRowIndex, col+1].Value = SaveData4Teach[col]; // 填入資料 
                        break;
                    case 1:
                        worksheet.Cells[newRowIndex, col+1].Value = SaveData4Test[col]; // 填入資料 
                        break;
                    case 2:
                        worksheet.Cells[newRowIndex, col+1].Value = SaveData6Teach[col]; // 填入資料 
                        break;
                    case 3:
                        worksheet.Cells[newRowIndex, col+1].Value = SaveData6Test[col]; // 填入資料 
                        break;
                }
            }
            // 保存變更 
            package.Save();
            Debug.Log($"已更新 Excel 檔案: {filePath}");
        }
    }
}
