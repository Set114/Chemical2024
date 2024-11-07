using System.Collections;
using UnityEngine;

public class LevelEndSequence : MonoBehaviour
{
    public CameraController cameraController; // 相機控制器
    public SwitchUI switchUI; // 關卡 UI 管理器
    public CheckImage checkImage; // 圖像檢查
    public ELFStatus elfStatus; // ELF 狀態控制

    [Header("時間設定")]
    private bool haveAni = false; // 判斷是否需要動畫
    private float showELFDelay = 2f; // 顯示 ELF 的延遲時間
    private float cameraZoomDelay = 0f; // 縮放鏡頭前的延遲時間
    private float levelChangeDelay = 5f; // 關卡變更的延遲時間
    private float nextUIShowDelay = 1f; // 顯示下一個 UI 的延遲時間
    private string answer; 
    //[Header("levelCount")]
    private int levelCount = 1; 
    [Header("END")]
    private bool showEndUI = false; 
    [Header("EndUI")]
    [SerializeField] GameObject learnEndUI;
    [SerializeField] GameObject testEndUI;
    [Header("LoadingSign")]
    [SerializeField] GameObject loading_sign;

    [Header("UI")]
    [SerializeField] GameObject learnUI;
    [Header("TeachEndUI")]
    [SerializeField] GameObject testUI;
    private int chapterMode = 0;

    public LearnDataManager learnDataManager;
    public TestDataManager testDataManager;
    public PlaySpeechAudio playSpeechAudio;
    public SwitchT switchT;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        loading_sign.SetActive(false);
    }

    public void EndLevel(bool showEndUIBool,bool haveAniBool,float showELFDelayTime,float levelChangeDelayTime,float nextUIShowDelayTime,string answerData, System.Action callback = null)
    {
        showEndUI = showEndUIBool;
        haveAni = haveAniBool;

        showELFDelay = showELFDelayTime;// 延遲小精靈的顯示時間
        levelChangeDelay = levelChangeDelayTime;// 小精靈的說話時間
        nextUIShowDelay = nextUIShowDelayTime;// 到下一關前的緩衝時間
        answer = answerData;// 答案

        levelCount = switchUI.GetLevelCount();

        playSpeechAudio.SetCurrentLevel(levelCount);
        StartCoroutine(ShowELFAndThenZoomIn(callback)); // 傳入 callback
    }

    // 先顯示 ELF，再進行鏡頭縮放
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

    // 延遲關卡變更
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
        switchUI.CompletedState(levelCount);
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
