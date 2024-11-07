using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyGameManager : MonoBehaviour
{
    [Header("比對卡牌的清單")]
    public List<Card> cardComparison = new List<Card>();

    [Header("已配對的卡牌數量")]
    public int matchedCardsCount = 0;
    public TMP_Text TimerCount;

    public GameObject Canvas;
    public Timer Timer;
    public CheckAnswer checkAnswer;
    public ControllerHaptics hapticsController;
    public LevelEndSequence levelEndSequence;
    public TestDataManager testDataManager;

    private bool a1 = false;
    public void AddCardInCardComparison(Card card)
    {
        testDataManager.StartLevel();
        testDataManager.GetsId(0);
        cardComparison.Add(card);
    }

    public bool ReadyToCompareCards
    {
        get
        {
            return cardComparison.Count == 2;
        }
    }

    public void CompareCardsInList()
    {
        if (ReadyToCompareCards)
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
        ClearCardComparison();
        
        matchedCardsCount += 2;

        if (matchedCardsCount >= 12) // 假設場景中總共有12張卡牌
        {
            ShowEndCanvas();
        }
    }

    void ClearCardComparison()
    {
        cardComparison.Clear();
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
        ClearCardComparison();
    }
    
    //結束
    // void ShowEndCanvas()
    // {
    //     Canvas.SetActive(true);
    //     Timer.isCounting = false;
    //     checkAnswer.Stage5ScoreCount(TimerCount.text);
    //     testDataManager.Getanswer(TimerCount.text.ToString());
    //     levelEndSequence.EndLevel(true, false, 2f, 0f, 5f, 0f,"1");
    // }
    public void ShowEndCanvas()
    {
        Canvas.SetActive(true);
        Timer.isCounting = false;
        StartCoroutine(ExecuteWithDelay());
    }

    private IEnumerator ExecuteWithDelay()
    {
        checkAnswer.Stage5ScoreCount(float.Parse(TimerCount.text));
        testDataManager.Getanswer(TimerCount.text.ToString());

        // 確保兩者執行完成後再執行 EndLevel
        yield return null;
    
        levelEndSequence.EndLevel(true,false, 1f, 4f, 1f, "1", () => { });
    }


}
