using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TextMapping  //���ܮج[�c
{
    public string chapter;
    [TextArea(3, 5)] public string content;
}

[System.Serializable]
public struct DialogMapping  //��ܮج[�c
{
    public string title;    //���D��r
    [TextArea(3, 5)]
    public string question; //���e��r
    [TextArea(3, 5)]
    public string warning;  //ĵ�i�T��
    public string voiceName;  //�����y��
}

[System.Serializable]
public struct QuestionMapping  //�ݵ���ܮج[�c
{
    [TextArea(3, 5)]
    public string question;    //�D�ؤ�r
    public string voiceQuestionName;  //�����y��
    [TextArea(3, 5)]
    public string answer;    //���פ�r
    public string voiceAnswerName;  //�����y��
    [TextArea(3, 5)]
    public string answer0Text;  //����1�����e��r
    public Sprite answer0Image; //答案1圖片
    public Sprite user0Icon;    //答案提出者1頭像
    [TextArea(3, 5)]
    public string answer1Text;  //����2�����e��r
    public Sprite answer1Image; //答案2圖片
    public Sprite user1Icon;    //答案提出者2頭像
    [TextArea(3, 5)]
    public string answer2Text;  //����3�����e��r
    public Sprite answer2Image; //答案3圖片
    public Sprite user2Icon;    //答案提出者3頭像

    public int answerNumber;    //���T����     
}

[System.Serializable]
public struct UIDailog  //��ܮج[�c
{
    public string title;    //���D��r
    [TextArea(3, 5)]
    public string question; //���e��r
    [TextArea(3, 5)]
    public string warning;  //ĵ�i�T��
    public string voiceName;  //�����y��

    public UIDailog(string Title, string Question, string Warning, string VoiceName)
    {
        title = Title;
        question = Question;
        warning = Warning;
        voiceName = VoiceName;
    }
}

[System.Serializable]
public struct QuestionDailog  //�ݵ���ܮج[�c
{
    [TextArea(3, 5)]
    public string question;    //�D�ؤ�r
    public string voiceQuestionName;  //�����y��
    [TextArea(3, 5)]
    public string answer;    //���פ�r
    public string voiceAnswerName;  //�����y��
    [TextArea(3, 5)]
    public string answer0Text;  //����1�����e��r
    public string user0Name;    //����1�����X��
    [TextArea(3, 5)]
    public string answer1Text;  //����2�����e��r
    public string user1Name;    //����2�����X��
    public int answerNumber;    //���T����        
}