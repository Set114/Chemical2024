using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LearnDataManager : MonoBehaviour
{
    //校方資訊
    private string schoolId;
    private string classId;
    //學生資訊
    private string studentId;
    //上傳資料
    private int getUId;
    private int getSId;
    private string startTime;
    private string completionTime;
    private string answer = "-1";

    private string folderName;
    private string sheetId;
    private string fileName;
    public GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        GetData();
    }

    private void GetData()
    {
        //資料查詢測試
        // schoolId = "173510";
        // classId = "173510_0001";
        // studentId = "a1110831001";
        // getUId = 1;   
        
        schoolId = gameManager.GetSchID();  
        classId = gameManager.GetClass();  
        getUId = gameManager.GetUid();   
        studentId = gameManager.GetPlayerID();
        
        // Debug.Log("schoolId = " + schoolId);
        // Debug.Log("classId = " + classId);
        // Debug.Log("getUId = " + getUId);
        // Debug.Log("studentId = " + studentId);

        folderName = schoolId;
        fileName = classId + "_Learn";
        sheetId = classId + "_" + getUId;

        StartCoroutine(LearnUploadData()); 
    }

    //獲取子單元資訊
    public void GetsId(int levelIndex)
    {
        getSId = levelIndex + 1;
        //Debug.Log("sId: " + getSId);
    }

    //紀錄關卡開始時間
    public void StartLevel()
    {
        startTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        // Debug.Log("Start Times: " + startTime);
    }

    // 紀錄關卡結束時間
    public void CompleteLevel()
    {
        completionTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //Debug.Log("Completion Times: " + completionTime);
    }

    public void EndLevelWithCallback(string levelAnswer, Action callback)
    {
        StartCoroutine(EndLevelCoroutine(levelAnswer, callback));
    }

    private IEnumerator EndLevelCoroutine(string levelAnswer, Action callback)
    {
        CompleteLevel();

        if (levelAnswer == "0")
        {
            answer = "0";
        }
        else if (levelAnswer == "1")
        {
            answer = "1";
        }

        //Loading_sign.SetActive(true);
        yield return StartCoroutine(LearnUploadData()); // 等待上传完成

        //Loading_sign.SetActive(false);
        callback?.Invoke(); // 执行回调
    }
    private IEnumerator LearnUploadData()
    {
    if (startTime != null && completionTime != null)
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "writeLearnData");
        form.AddField("folderName", folderName);
        form.AddField("fileName", fileName);
        form.AddField("sheetId", sheetId);
        form.AddField("studentid", studentId);
        form.AddField("sId", getSId);
        form.AddField("sTime", startTime);
        form.AddField("eTime", completionTime);
        form.AddField("getAnswer", answer);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxWveBEqj_fws4tqL-0vHVl1NitZEkHKizdiOZiOE8w5g-28KAauLcrHFJkUqYrnODSng/exec", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogWarning("網絡錯誤或HTTP錯誤: " + www.error);
            }
            else
            {
                Debug.Log("獲取到的檔案連結: " + www.downloadHandler.text);
            }
        }
    }
    else
    {
        // Debug.LogWarning("開始時間或完成時間為空，無法上傳資料。");
    }
    // InitializeUploadObject();
    }

    private void InitializeUploadObject()
    {
        startTime = string.Empty;
        completionTime = string.Empty;
        answer = "-1";

        //Debug.Log("初始化上傳物件完成");
    }
    
}
