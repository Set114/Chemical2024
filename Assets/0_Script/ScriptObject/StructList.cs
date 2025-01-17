using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TextMapping  //提示框架構
{
    public string chapter;
    [TextArea(3, 5)] public string content;
}

[System.Serializable]
public struct DialogMapping  //對話框架構
{
    public string title;    //標題文字
    [TextArea(3, 5)]
    public string question; //內容文字
    [TextArea(3, 5)]
    public string warning;  //警告訊息
    public string voiceName;  //對應語音
}

[System.Serializable]
public struct QuestionMapping  //問答對話框架構
{
    [TextArea(3, 5)]
    public string question;    //題目文字
    public string voiceQuestionName;  //對應語音
    [TextArea(3, 5)]
    public string answer;    //答案文字
    public string voiceAnswerName;  //對應語音
    [TextArea(3, 5)]
    public string answer0Text;  //答案1的內容文字
    public string user0Name;    //答案1的提出者
    [TextArea(3, 5)]
    public string answer1Text;  //答案2的內容文字
    public string user1Name;    //答案2的提出者
    public int answerNumber;    //正確答案        
}

[System.Serializable]
public struct UIDailog  //對話框架構
{
    public string title;    //標題文字
    [TextArea(3, 5)]
    public string question; //內容文字
    [TextArea(3, 5)]
    public string warning;  //警告訊息
    public string voiceName;  //對應語音

    public UIDailog(string Title, string Question, string Warning, string VoiceName)
    {
        title = Title;
        question = Question;
        warning = Warning;
        voiceName = VoiceName;
    }
}

[System.Serializable]
public struct QuestionDailog  //問答對話框架構
{
    [TextArea(3, 5)]
    public string question;    //題目文字
    public string voiceQuestionName;  //對應語音
    [TextArea(3, 5)]
    public string answer;    //答案文字
    public string voiceAnswerName;  //對應語音
    [TextArea(3, 5)]
    public string answer0Text;  //答案1的內容文字
    public string user0Name;    //答案1的提出者
    [TextArea(3, 5)]
    public string answer1Text;  //答案2的內容文字
    public string user1Name;    //答案2的提出者
    public int answerNumber;    //正確答案        
}