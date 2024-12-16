using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasController_Unit3 : MonoBehaviour
{
    public GameObject Buypage;
    public GameObject Correctpage;
    public GameObject Wrongpage;
    public GameObject Sussespage;
    public GameObject NotEnough;
    public GameObject Tutorial;
    public GameObject Ball;
    public GameObject Atom;
    public GameObject Screen;
    public GameObject Trash;


    public Button Correct;
    public Button Wrong;
    public Button Tutorialbtn;

    public DetectBall detectBall;
    //public Whiteboard whiteboard;
    public ElementDisplay elementDisplay;

    // 当前关卡的索引
    private int currentPartIndex = 1;
    // 关卡物件数组
    public GameObject[] questions;
    public GameObject[] answers;

    private int chapterMode = 0;
    // 假設關卡一開始是教學模式，0 = 教學 ，1 = 測驗
    private GameManager gameManager; // 讀取玩家資料
    private calculateManager calculate;

    void Start()
    {
        gameManager = GameManager.Instance;
        calculate = GetComponent<calculateManager>();
        chapterMode = gameManager.GetChapterMode();
        if (chapterMode == 0)
        {
            currentPartIndex = 0;
        }
        else
        {
            currentPartIndex = 1;
        }
        // 为按钮添加点击事件监听
        Tutorialbtn.onClick.AddListener(OnTutorialButtonClick);
        Correct.onClick.AddListener(OnCorrectButtonClick);
        Wrong.onClick.AddListener(OnWrongButtonClick);

        Screen.SetActive(false);
        Trash.SetActive(false);
        questions[currentPartIndex].SetActive(true);
    }

    public void Buy()
    {
        //detectBall.CheckRequiredElementQuantities();
        Atom.SetActive(true);
        Buypage.SetActive(false);
        Screen.SetActive(true);
        Ball.SetActive(true);
        Trash.SetActive(false);
        answers[currentPartIndex].SetActive(true);
    }
    public void BackToBuy()
    {
        Atom.SetActive(false);
        Tutorial.SetActive(false);
        Correctpage.SetActive(false);
        NotEnough.SetActive(false);
        Buypage.SetActive(true);
        Screen.SetActive(false);
        Ball.SetActive(false);
        questions[currentPartIndex].SetActive(true);
        for (int i = 0; i <= 5; i++)
        {
            // 關閉 parts[0] 到 parts[5]
            answers[i].SetActive(false);
        } 
    }
    public void ShowTutorial()
    {
        Trash.SetActive(true);
        Atom.SetActive(false);
        Tutorial.SetActive(true);
        questions[0].SetActive(false);
        answers[0].SetActive(false);
        elementDisplay.UpdateDisplay();
    }

    public void part1()
    {
        Trash.SetActive(true);
        Atom.SetActive(false);
        questions[1].SetActive(false);
        answers[1].SetActive(false);
        Correctpage.SetActive(true);
    }

    public void part2()
    {
        questions[2].SetActive(false);
        answers[2].SetActive(false);
        Correctpage.SetActive(true);
    }

    public void part3()
    {
        Atom.SetActive(false);
        questions[3].SetActive(false);
        answers[3].SetActive(false);
        Correctpage.SetActive(true);
    }

    public void part4()
    {
        Atom.SetActive(false);
        questions[4].SetActive(false);
        answers[4].SetActive(false);
        Correctpage.SetActive(true);
    }

    public void part5()
    {
        Atom.SetActive(false);
        questions[5].SetActive(false);
        answers[5].SetActive(false);
        Sussespage.SetActive(true);
    }

    public void NotEnoughQuantity()
    {
        Atom.SetActive(false);
        NotEnough.SetActive(true);
        Buypage.SetActive(true);
        Screen.SetActive(false);
        Ball.SetActive(false);
        for (int i = 0; i <= 5; i++)
        {
            // 關閉 parts[0] 到 parts[5]
            answers[i].SetActive(false);
        }
    }
    public void Wrongsituation()
    {
        Wrongpage.SetActive(true);
        Atom.SetActive(false);
    }

    // Tutorial按钮点击处理
    private void OnTutorialButtonClick()
    {
        currentPartIndex++; // 增加当前关卡索引
        calculate.ResetAllCounts();
        BackToBuy();
        //detectBall.CheckRequiredElementQuantities();
        StartCoroutine(b());
    }

    // Correct按钮点击处理
    private void OnCorrectButtonClick()
    {
        currentPartIndex++; // 增加当前关卡索引
        BackToBuy();
        StartCoroutine(b());
        //detectBall.CheckRequiredElementQuantities();
        //HideCurrentPartAndShowNext();
    }

    private void OnWrongButtonClick()
    {
        Wrongpage.SetActive(false);
        Atom.SetActive(true);
    }

    /*// 隐藏当前关卡并显示下一个关卡
    private void HideCurrentPartAndShowNext()
    {
        // 隐藏当前关卡
        questions[currentPartIndex].SetActive(false);
        answers[currentPartIndex].SetActive(false);
        currentPartIndex++; // 增加当前关卡索引
        // 显示下一个关卡
        questions[currentPartIndex].SetActive(true);
        answers[currentPartIndex].SetActive(true);
    }*/

    private IEnumerator b()
    {
        yield return new WaitForSeconds(0.1f);
        detectBall.FindParts();
    }
}
