using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    // Singleton instance
    private static UserDataManager instance;

    // 用於在場景間保存的資料
    public string currentPlayerID;
    public string currentPlayerName;
    public string currentSchID;
    // public string currentMail;
    // public string currentYear;
    public string currentClass;
    public int currentUid;
    public int chapterMode;
    public string startTime; // 2025.2.12 戴偉勝

    // 單例實例的取得方式
    public static UserDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 嘗試查找現有的 UserDataManager 實例
                instance = FindObjectOfType<UserDataManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("UserDataManager");
                    instance = obj.AddComponent<UserDataManager>();
                    DontDestroyOnLoad(obj); // 在場景切換時不銷毀
                    // Debug.Log("UserDataManager 初始化");
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 在場景切換時不銷毀
            // Debug.Log("UserDataManager 初始化");
        }
        else if (instance != this)
        {
            // Debug.LogWarning("檢測到重複的 UserDataManager 實例並銷毀");
            Destroy(gameObject); // 避免重複的 UserDataManager 實例
        }
    }
    // 2025.2.12 戴偉勝 更新開始時間
    public void UpdateStartTime()
    {
        startTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }
    // 2025.2.12 戴偉勝 取得開始時間
    public string GetStartTime()
    {
        return startTime;
    }
    // 更新玩家資料
    public void UpdatePlayerData(string playerName)
    {
        currentPlayerName = playerName;
        // Debug.Log($"Player name updated to: {currentPlayerName}");
    }

    // 更新下一個場景索引
    public void UpdateChapterMode(int levelIndex)
    {
        chapterMode = levelIndex;
        // Debug.Log($"Chapter mode updated to: {chapterMode}");
    }

    // 獲取當前玩家名稱
    public string GetCurrentPlayerName()
    {
        return currentPlayerName;
    }

    public void UpdateUid(int levelIndex)
    {
        currentUid = levelIndex;
        // Debug.Log($"UID updated to: {currentUid}");
    }

    public string GetPlayerID()
    {
        return currentPlayerID;
    }

    // public string GetMail()
    // {
    //     return currentMail;
    // }

    // public string GetYear()
    // {
    //     return currentYear;
    // }

    public string GetClass()
    {
        return currentClass;
    }

    public string GetSchID()
    {
        return currentSchID;
    }

    public int GetChapterMode()
    {
        return chapterMode;
    }

    public int GetUid()
    {
        return currentUid;
    }

    // 設置玩家資料
    public void SetPlayerData(string schoolID,string classData, string studentID, string studentName)
    {
        currentPlayerID = studentID; 
        currentPlayerName = studentName; 
        currentSchID = schoolID;
        currentClass = classData;
        // Debug.Log("Set player data in UserDataManager.");
    }
}
