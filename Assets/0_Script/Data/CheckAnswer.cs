using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAnswer : MonoBehaviour
{
    public TestDataManager testDataManager;

    public void CorrectAnswerData(int Index)
    {
        string answerData = "";
        int score = 0;
        if(Index == 0 || Index == 1 || Index == 3)
        {
            answerData = "物理";
        }
        else if(Index == 2 || Index == 4)
        {
            answerData = "化學";
        }

        if(Index == 0 || Index == 1 || Index == 2)
        {
            score = 33 ;
        }
        else if(Index == 3 || Index == 4)
        {

            score = 50 ;
        }
        testDataManager.Getanswer(answerData);
        testDataManager.Getscore(score);

    }

    public void WrongAnswerData(int Index)
    {
        string answerData = "";
        if(Index == 0 || Index == 1 || Index == 3)
        {
            answerData = "化學";

        }
        else if(Index == 2 || Index == 4)
        {
            answerData = "物理";

        }

        testDataManager.Getanswer(answerData);
        testDataManager.Getscore(0);
    }

    public void Stage5ScoreCount(float Index)
    {
        int score = 100;
        
        if (Index > 18)
        {
            // 計算超過的秒數，每多 2 秒扣 2 分
            float extraSeconds = Index - 18;
            int deduction = Mathf.FloorToInt(extraSeconds / 2) * 2;
            score -= deduction;
        }

        score = Mathf.Max(score, 0);
        // Debug.Log("分數: " + score);
        testDataManager.Getscore(score);
    }
}
