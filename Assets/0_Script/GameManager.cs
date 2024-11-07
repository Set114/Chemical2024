using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;

    // 用於在場景間保存的資料
    public string currentPlayerID;
    public string currentPlayerName;
    public string currentSchID;
    // public string currentMail;
    // public string currentYear;
    public string currentClass;
    public int currentUid;
    public int chapterMode;

    // 單例實例的取得方式
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 嘗試查找現有的 GameManager 實例
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj); // 在場景切換時不銷毀
                    // Debug.Log("GameManager 初始化");
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
            // Debug.Log("GameManager 初始化");
        }
        else if (instance != this)
        {
            // Debug.LogWarning("檢測到重複的 GameManager 實例並銷毀");
            Destroy(gameObject); // 避免重複的 GameManager 實例
        }
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
        // Debug.Log("Set player data in GameManager.");
    }
}
