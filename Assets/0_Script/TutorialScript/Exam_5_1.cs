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

    private int totalPairs; // 配對的總數
    private int matchedPairs = 0; // 已配對的數量


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
    private QuestionManager questionManager;    //管理題目介面
    private AudioManager audioManager;
    private TestDataManager testDataManager;
    private ControllerHaptics hapticsController;

    // Start is called before the first frame update
    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        questionManager = FindObjectOfType<QuestionManager>();
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

    //初始化卡牌
    void ShuffleCards()
    {
        // 將卡牌隨機排列
        foreach (GameObject card in cards)
        {
            card.SetActive(true);
        }
        /*
        for (int i = 0; i < cards.Length; i++)
        {
            int randomIndex = Random.Range(0, cards.Length);
            var temp = cards[i].transform.position;
            cards[i].transform.position = cards[randomIndex].transform.position;
            cards[randomIndex].transform.position = temp;
        }
        */
    }

    //當卡片被翻開
    public void OnCardClicked(Card clickedCard)
    {
        if (cardComparison.Count >= 2 || clickedCard.IsFlipped) return;

        // 翻開卡牌
        clickedCard.FlipCard();
        cardComparison.Add(clickedCard);

        // 檢查是否滿足配對條件
         if (cardComparison.Count >= 2)
        {
            if (cardComparison[0].cardPattern == cardComparison[1].cardPattern)
            {
                StartCoroutine(HandleMatchedCards());

                audioManager.PlaySound(2);
            }
            else
            {
                StartCoroutine(MissMatchCards());

                audioManager.PlaySound(3);
            }
        }
    }


    IEnumerator HandleMatchedCards()
    {
        yield return new WaitForSeconds(1f);

        foreach (Card card in cardComparison)
        {
            card.Matched();
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
        yield return new WaitForSeconds(2f);

        questionManager.TriggerHapticFeedback();
        foreach (Card card in cardComparison)
        {
            card.FlipCard();
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
        // 初始化卡牌
        ShuffleCards();
        matchedCardsCount = 0;
        
        timer = 0f; // 重置计时器值
        Status = 1;
    }

    public void OnEndButtonClicked()
    {
        testDataManager.Getanswer(timerText.text.ToString());
        levelObjManager.LevelClear(2);
    }
}
