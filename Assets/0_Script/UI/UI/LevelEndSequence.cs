using System.Collections;
using UnityEngine;

public class LevelEndSequence : MonoBehaviour
{
    [Header("時間設定")]
    private bool haveAni = false; // 判斷是否需要動畫
    private float showELFDelay = 2f; // 顯示 ELF 的延遲時間
    private float cameraZoomDelay = 0f; // 縮放鏡頭前的延遲時間
    private float levelChangeDelay = 5f; // 關卡變更的延遲時間
    private float nextUIShowDelay = 1f; // 顯示下一個 UI 的延遲時間
    private bool showEndUI = false; 
    
    private string answer; 
    private int levelCount = 1; 

    [Header("EndUI")]
    [SerializeField] GameObject learnEndUI;
    [SerializeField] GameObject testEndUI;
    [Header("LoadingSign")]
    [SerializeField] GameObject loading_sign;

    [Header("Canvas_LearnI")]
    [SerializeField] GameObject learnUI;
    [Header("Canvas_Test")]
    [SerializeField] GameObject testUI;
    
    private int chapterMode = 0;
    // 假設關卡一開始是教學模式，0 = 教學 ，1 = 測驗

    public SwitchUI switchUI; // 關卡 UI 
    public SwitchT switchT; // 精靈提示文字 UI 
    public CheckImage checkImage; // 圖像檢查
    public ELFStatus elfStatus; // ELF 狀態控制
    public LearnDataManager learnDataManager; // 紀錄
    public TestDataManager testDataManager; // 紀錄
    public PlaySpeechAudio playSpeechAudio; // 語音撥放
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        loading_sign.SetActive(false);
    }

    public void EndLevel(bool showEndUIBool,bool haveAniBool,float showELFDelayTime,float levelChangeDelayTime,float nextUIShowDelayTime,string answerData, System.Action callback = null)
    {
        showEndUI = showEndUIBool; // 是否是最後一關
        haveAni = haveAniBool; // 是否有動畫

        showELFDelay = showELFDelayTime; // 延遲小精靈的顯示時間
        levelChangeDelay = levelChangeDelayTime; // 小精靈的說話時間
        nextUIShowDelay = nextUIShowDelayTime; // 到下一關前的緩衝時間
        answer = answerData; // 答案

        levelCount = switchUI.GetLevelCount();

        playSpeechAudio.SetCurrentLevel(levelCount);
        StartCoroutine(ShowELFAndThenZoomIn(callback)); // 傳入 callback
    }

    // 如果需顯示 ELF 顯示 ELF，否則顯示結束UI或下一關
    IEnumerator ShowELFAndThenZoomIn(System.Action callback)
    {
        if (haveAni == true){
            yield return ShowELFWithDelay(showELFDelay); // 等待指定時間後顯示 ELF
        }
        
        StartCoroutine(DelayedLevelChange(callback)); // 開始延遲關卡變更，傳入 callback
    }

    // 延遲顯示 ELF
    IEnumerator ShowELFWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        elfStatus.ShowELF(); // 顯示 ELF
        if (switchT != null)
        {
           switchT.ChangeLevel(levelCount);
        }
    }

    // 延遲關卡變更，使小精靈講完話
    IEnumerator DelayedLevelChange(System.Action callback)
    {
        yield return new WaitForSeconds(levelChangeDelay);
        elfStatus.HideELF(); // 隱藏 ELF
        chapterMode = gameManager.GetChapterMode();
        loading_sign.SetActive(true);
        if (switchT != null)
        {
           switchT.CloseT();
        }

        //結束
        if (chapterMode == 0)
        {
            learnDataManager.EndLevelWithCallback(answer, () => StartCoroutine(ShowNextUIAfterDelay(nextUIShowDelay, callback)));
        }
        else if (chapterMode == 1)
        {
            testDataManager.EndLevelWithCallback(() => StartCoroutine(ShowNextUIAfterDelay(nextUIShowDelay, callback)));
        }
    }

    // 延遲顯示下一個 UI
    IEnumerator ShowNextUIAfterDelay(float delay, System.Action callback)
    {
        chapterMode = gameManager.GetChapterMode();
        yield return new WaitForSeconds(delay); // 等待指定時間
        switchUI.CompletedState(levelCount); // 紀錄關卡已完成
        checkImage.SwitchImage(levelCount); // 切換圖像
        
        if(showEndUI == false)
        {
            switchUI.ShowNextUI(); // 顯示下一個 UI
        }
        else 
        {
            if(chapterMode == 0)
            {
                learnUI.SetActive(true);
                learnEndUI.SetActive(true);
            }
            else
            {
                testUI.SetActive(true);
                testEndUI.SetActive(true);
            }
        }
        loading_sign.SetActive(false);
        
        // 執行 callback 以回報完成
        callback?.Invoke();
    }
}
