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
    public GameObject W_part0;
    public GameObject W_part1;
    public GameObject W_part2;
    public GameObject W_part3;
    public GameObject W_part4;
    public GameObject W_part5;
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
    private GameObject[] parts;

    void Start()
    {
        // 初始化关卡物件数组
        parts = new GameObject[] { W_part0, W_part1, W_part2, W_part3, W_part4, W_part5 };

        // 为按钮添加点击事件监听
        Tutorialbtn.onClick.AddListener(OnTutorialButtonClick);
        Correct.onClick.AddListener(OnCorrectButtonClick);
        Wrong.onClick.AddListener(OnWrongButtonClick);

        Screen.SetActive(false);
        Trash.SetActive(false);
    }

    public void Buy()
    {
        //detectBall.CheckRequiredElementQuantities();
        Atom.SetActive(true);
        Buypage.SetActive(false);
        Screen.SetActive(true);
        Ball.SetActive(true);
        Trash.SetActive(false);
        parts[0].SetActive(true); // 显示第一关卡 (W_part0)
    }

    public void ShowTutorial()
    {
        Trash.SetActive(true);
        Atom.SetActive(false);
        Tutorial.SetActive(true);
        W_part0.SetActive(false);
        elementDisplay.UpdateDisplay();
    }

    public void part1()
    {
        Trash.SetActive(true);
        Atom.SetActive(false);
        W_part1.SetActive(false);
        Correctpage.SetActive(true);
    }

    public void part2()
    {
        W_part2.SetActive(false);
        Correctpage.SetActive(true);
    }

    public void part3()
    {
        Atom.SetActive(false);
        W_part3.SetActive(false);
        Correctpage.SetActive(true);
    }

    public void part4()
    {
        Atom.SetActive(false);
        W_part4.SetActive(false);
        Correctpage.SetActive(true);
    }

    public void part5()
    {
        Atom.SetActive(false);
        W_part5.SetActive(false);
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
            parts[i].SetActive(false); // 關閉 parts[0] 到 parts[5]
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
        Atom.SetActive(true);
        Tutorial.SetActive(false);
        W_part1.SetActive(true);
        detectBall.CheckRequiredElementQuantities();
        StartCoroutine(b());
    }

    // Correct按钮点击处理
    private void OnCorrectButtonClick()
    {
        Atom.SetActive(true);
        Correctpage.SetActive(false);
        StartCoroutine(b());
        detectBall.CheckRequiredElementQuantities();
        HideCurrentPartAndShowNext();
    }

    private void OnWrongButtonClick()
    {
        Wrongpage.SetActive(false);
        Atom.SetActive(true);
    }

    // 隐藏当前关卡并显示下一个关卡
    private void HideCurrentPartAndShowNext()
    {
        parts[currentPartIndex].SetActive(false); // 隐藏当前关卡
        currentPartIndex++; // 增加当前关卡索引
        parts[currentPartIndex].SetActive(true); // 显示下一个关卡
    }

    private IEnumerator b()
    {
        yield return new WaitForSeconds(0.1f);
        detectBall.FindParts();
    }
}
