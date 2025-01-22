using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exam_5_1 : MonoBehaviour
{
    [Header("卡牌")]
    public GameObject[] cards;
    [Header("比對卡牌的清單")]
    public List<Card> cardComparison = new List<Card>();
    [Header("已配對的卡牌數量")]
    public int matchedCardsCount = 0;

    [Header("UI")]
    [Tooltip("開始介面")]
    [SerializeField] private GameObject startMenu;
    [Tooltip("結束介面")]
    [SerializeField] private GameObject endMenu;
    [Tooltip("計時器文字")]
    [SerializeField] private Text timerText;

    private float timer = 0f;
    private int Status = 0;

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;
    private TestDataManager testDataManager;
    private ControllerHaptics hapticsController;

    // Start is called before the first frame update
    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        testDataManager = FindObjectOfType<TestDataManager>();
        hapticsController = FindObjectOfType<ControllerHaptics>();
        audioManager.PlayVoice("E5_1_1");
        startMenu.SetActive(true);
        endMenu.SetActive(false);
        timerText.text = "0.0"; // 初始时显示计时器为0
    }

    private void Update()
    {
        switch (Status)
        {
            case 0://待機
                break;
            case 1://開始遊戲
                timer += Time.deltaTime;
                break;
            case 2://結束
                break;
        }
        timerText.text = timer.ToString("0.0"); // 更新显示的文本内容
    }

    // 將卡牌添加到比對清單中，進行比對
    public void AddCardInCardComparison(Card card)
    {
        testDataManager.StartLevel();
        testDataManager.GetsId(0);
        cardComparison.Add(card);
    }

    public void CompareCardsInList()
    {
        if (cardComparison.Count == 2)
        {
            if (cardComparison[0].cardPattern == cardComparison[1].cardPattern)
            {
                StartCoroutine(HandleMatchedCards());
            }
            else
            {
                StartCoroutine(MissMatchCards());
            }
        }
    }

    IEnumerator HandleMatchedCards()
    {
        foreach (var card in cardComparison)
        {
            card.ChangeDetectItemsColor(Color.green);
        }

        yield return new WaitForSeconds(1f);

        foreach (var card in cardComparison)
        {
            card.ChangeDetectItemsColor(Color.white);
            Destroy(card.gameObject);
        }
        cardComparison.Clear();

        matchedCardsCount += 2;

        if (matchedCardsCount >= 12) // 假設場景中總共有12張卡牌
        {
            GameOver();
        }
    }

    IEnumerator MissMatchCards()
    {
        hapticsController.TriggerHapticFeedback(true);
        foreach (var card in cardComparison)
        {
            card.ChangeDetectItemsColor(Color.red);
        }
        yield return new WaitForSeconds(1f);

        foreach (var card in cardComparison)
        {
            card.ChangeDetectItemsColor(Color.white);
            card.CloseCard();
        }
        cardComparison.Clear();
    }

    //結束
    private void GameOver()
    {
        endMenu.SetActive(true);
        audioManager.PlayVoice("E5_1_2");
        Status = 2;
    }

    public void OnStartButtonClicked()
    {
        startMenu.SetActive(false);
        foreach(GameObject card in cards)
        {
            card.SetActive(true);
        }
        timer = 0f; // 重置计时器值
        Status = 1;
    }

    public void OnEndButtonClicked()
    {
        testDataManager.Getanswer(timerText.text.ToString());
        levelObjManager.LevelClear(2);
    }
}
