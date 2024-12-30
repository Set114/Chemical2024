using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool[] levelCompleted;

    public LevelManager(int numberOfLevels)
    {
        levelCompleted = new bool[numberOfLevels];
    }

    // 標記某個關卡為通關
    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelCompleted.Length)
        {
            levelCompleted[levelIndex] = true;
        }
        else
        {
            Debug.LogError("Invalid level index");
        }
    }

    // 檢查某個關卡是否已通關
    public bool IsLevelCompleted(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelCompleted.Length)
        {
            return levelCompleted[levelIndex];
        }
        else
        {
            Debug.LogError("Invalid level index");
            return false;
        }
    }
}
