using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectBall : MonoBehaviour
{
    public int c = 0, n = 0, o = 0, h = 0, fe = 0, allcount = 0;
    public int displayC = 0, displayN = 0, displayO = 0, displayH = 0, displayFe = 0;

    public TMP_Text cText;
    public TMP_Text nText;
    public TMP_Text oText;
    public TMP_Text hText;
    public TMP_Text feText;

    //引用其他程式碼
    private ElementalBallCounter elementalBallCounter;
    private ElementDisplay elementDisplay;
    private CanvasController_Unit3 canvasController;
    private AnimationController_Unit3 animationController;

    // 添加一個靜態事件來通知級別變化
    public static event System.Action<string> OnLevelChanged;
    public AudioManager audioManager;

    // 修改 CurrentLevel 為 property
    private string currentLevel;
    public string CurrentLevel
    {
        get => currentLevel;
        private set
        {
            if (currentLevel != value)
            {
                currentLevel = value;
                // 當 CurrentLevel 改變時觸發事件
                OnLevelChanged?.Invoke(currentLevel);
                //Debug.Log($"Level changed to: {currentLevel}"); // 用於調試
            }
        }
    }

    void Start()
    {
        elementalBallCounter = FindObjectOfType<ElementalBallCounter>(); // 获取 ElementalBallCounter 的引用     
        elementDisplay = FindObjectOfType<ElementDisplay>();
        canvasController = FindObjectOfType<CanvasController_Unit3>();
        animationController = FindObjectOfType<AnimationController_Unit3>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("(Clone)"))
        {
            RealBall realBall = other.GetComponent<RealBall>();
            Debug.Log(realBall);
            
            if (realBall != null && !realBall.isInsideTrigger)
            {
                realBall.isInsideTrigger = true;

                if (CurrentLevel == "part0")
                {
                    // 仅更新本地计数和显示计数
                    if (other.CompareTag("C"))
                    {
                        displayC--;
                        if (displayC < 0)
                        {
                            displayC = 0;
                            return;
                        }
                        c++;
                        allcount++;
                        cText.text = displayC.ToString();
                    }
                    else if (other.CompareTag("N"))
                    {
                        displayN--;
                        if (displayN < 0)
                        {
                            displayN = 0;
                            return;
                        }
                        n++;
                        allcount++;
                        nText.text = displayN.ToString();
                    }
                    else if (other.CompareTag("O"))
                    {
                        displayO--;
                        if (displayO < 0)
                        {
                            displayO = 0;
                            return;
                        }
                        o++;
                        allcount++;
                        oText.text = displayO.ToString();
                    }
                    else if (other.CompareTag("H"))
                    {
                        displayH--;
                        if (displayH < 0)
                        {
                            displayH = 0;
                            return;
                        }
                        h++;
                        allcount++;
                        hText.text = displayH.ToString();
                    }
                    else if (other.CompareTag("Fe"))
                    {
                        displayFe--;
                        if (displayFe < 0)
                        {
                            displayFe = 0;
                            return;
                        }
                        fe++;
                        allcount++;
                        feText.text = displayFe.ToString();
                    }
                }
                else
                {
                    // 在其他关卡中减少 ElementalBallCounter 的数量
                    if (other.CompareTag("C"))
                    {
                        c++;
                        allcount++;
                    }
                    else if (other.CompareTag("N"))
                    {
                        n++;
                        allcount++;
                    }
                    else if (other.CompareTag("O"))
                    {
                        o++;
                        allcount++;
                    }
                    else if (other.CompareTag("H"))
                    {
                        h++;
                        allcount++;
                    }
                    else if (other.CompareTag("Fe"))
                    {
                        fe++;
                        allcount++;
                    }
                    else
                    {
                        Debug.LogWarning("Unknown element tag: " + other.tag);
                    }
                }
                CheckQuantities();
            }
            else
            {
                Debug.Log("物體不符合條件，忽略觸發");
            }
        }
    }
    private void CheckQuantities()
    {
        //Debug.Log("allcount:" + allcount);
        //Debug.Log("GetRequiredBallCountForCurrentLevel:" + GetRequiredBallCountForCurrentLevel());
        // 如果数量足够，执行对应的逻辑
        if (allcount == GetRequiredBallCountForCurrentLevel())
        {
            //Debug.Log("CheckQuantities CurrentLevel:" + CurrentLevel);
            switch (CurrentLevel)
            {
                case "part0":
                    if (n == 4 && o == 2)
                    {
                        animationController.ActivateObjectsForLevel(CurrentLevel);
                        Debug.Log("教學關卡正確");
                    }
                    else
                    {
                        animationController.resetValue();
                        animationController.CloneRemover();
                        canvasController.Wrongsituation();
                    }
                    break;
                case "part1":
                    if (c == 1 && o == 2)
                    {
                        animationController.ActivateObjectsForLevel(CurrentLevel);
                        Debug.Log("第一關正確");
                    }
                    else
                    {
                        Debug.Log(c);
                        Debug.Log(o);
                        Debug.Log(allcount);
                        animationController.resetValue();
                        animationController.CloneRemover();
                        canvasController.Wrongsituation();
                        audioManager.Play("part1-withoutTheTrueAtom");
                    }
                    break;
                case "part2":
                    if (n == 2 && h == 2)
                    {
                        animationController.ActivateObjectsForLevel(CurrentLevel);
                        Debug.Log("第二關正確");
                    }
                    else
                    {
                        animationController.resetValue();
                        animationController.CloneRemover();
                        canvasController.Wrongsituation();
                        audioManager.Play("part2-withoutTheTrueAtom");
                    }
                    break;
                case "part3":
                    if (h == 1 && o == 2)
                    {
                        animationController.ActivateObjectsForLevel(CurrentLevel);
                        Debug.Log("第三關正確");
                    }
                    else
                    {
                        animationController.resetValue();
                        animationController.CloneRemover();
                        canvasController.Wrongsituation();
                        audioManager.Play("part3-withoutTheTrueAtom");
                    }
                    break;
                case "part4":
                    if (c == 3 && o == 2 && h == 8)
                    {
                        animationController.ActivateObjectsForLevel(CurrentLevel);
                        Debug.Log("第四關正確");
                    }
                    else
                    {
                        animationController.resetValue();
                        animationController.CloneRemover();
                        canvasController.Wrongsituation();
                        audioManager.Play("part4-withoutTheTrueAtom");
                    }
                    break;
                case "part5":
                    if (fe == 2 && o == 3 && c == 1)
                    {
                        animationController.ActivateObjectsForLevel(CurrentLevel);
                        Debug.Log("第五關正確");
                    }
                    else
                    {
                        animationController.resetValue();
                        animationController.CloneRemover();
                        canvasController.Wrongsituation();
                        audioManager.Play("part5-withoutTheTrueAtom");
                    }
                    break;
                default:
                    Debug.LogError("未匹配任何關卡條件: " + CurrentLevel);
                    break;
            }

            allcount = 0; // 重置计数
            c = 0;
            n = 0;
            o = 0;
            h = 0;
            fe = 0;
            }
    }

    private int GetRequiredBallCountForCurrentLevel()
    {
        switch (CurrentLevel)
        {
            case "part0": return 6;
            case "part1": return 3;
            case "part2": return 4;
            case "part3": return 3;
            case "part4": return 13;
            case "part5": return 6;
            default: return 0;
        }
    }

    public void CheckRequiredElementQuantities()
    {
        int requiredC = 0, requiredN = 0, requiredO = 0, requiredH = 0, requiredFe = 0;
        CurrentLevel = FindParts();
        //Debug.Log(CurrentLevel);
        
        // 确定当前关卡所需的元素数量
        switch (CurrentLevel)
        {
            case "part0":
                requiredN = 4; requiredO = 2;
                break;
            case "part1":
                requiredC = 1; requiredO = 2;
                break;
            case "part2":
                requiredN = 2; requiredH = 2;
                break;
            case "part3":
                requiredO = 2; requiredH = 1;
                break;
            case "part4":
                requiredC = 3; requiredO = 2; requiredH = 8;
                break;
            case "part5":
                requiredC = 1; requiredO = 3; requiredFe = 2;
                break;
            default:
                Debug.LogWarning("Unknown level: " + CurrentLevel);
                return;
        }
        Debug.Log("=========");
        Debug.Log("its checking");
        Debug.Log("=========");
        Debug.Log("c: " + elementalBallCounter.GetBallCount("c"));
        Debug.Log("n: " + elementalBallCounter.GetBallCount("n"));
        Debug.Log("o: " + elementalBallCounter.GetBallCount("o"));
        Debug.Log("h: " + elementalBallCounter.GetBallCount("h"));
        Debug.Log("fe: " + elementalBallCounter.GetBallCount("fe"));

        // 检查当前所拥有的元素数量
        if (elementalBallCounter.GetBallCount("c") < requiredC ||
            elementalBallCounter.GetBallCount("n") < requiredN ||
            elementalBallCounter.GetBallCount("o") < requiredO ||
            elementalBallCounter.GetBallCount("h") < requiredH ||
            elementalBallCounter.GetBallCount("fe") < requiredFe)
        {
            Debug.Log($"數量不足 {CurrentLevel}!");
            canvasController.NotEnoughQuantity();
            //這是放錯誤音效判斷
            if (CurrentLevel == "part0")
            {
                audioManager.Stop();
                audioManager.Play("part0-Wrong");
            }
            else if(CurrentLevel == "part1")
            {
                audioManager.Stop();
                audioManager.Play("part1-Wrong");
            }
            else if(CurrentLevel == "part2")
            {
                audioManager.Stop();
                audioManager.Play("part2-Wrong");
            }
            else if(CurrentLevel == "part3")
            {
                audioManager.Stop();
                audioManager.Play("part3-Wrong");
            }
            else if(CurrentLevel == "part4")
            {
                audioManager.Stop();
                audioManager.Play("part4-Wrong");
            }
            else if(CurrentLevel == "part5")
            {
                audioManager.Stop();
                audioManager.Play("part5-Wrong");
            }
            //return; 
        }
        else
        {
            Debug.Log("檢查目前所擁有的元素數量");
        }
    }

    public string FindParts()//查找關卡
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("part"))
            {
                if (obj.name == "part0")
                {
                    displayC = elementalBallCounter.GetBallCount("c");
                    displayN = elementalBallCounter.GetBallCount("n");
                    displayO = elementalBallCounter.GetBallCount("o");
                    displayH = elementalBallCounter.GetBallCount("h");
                    displayFe = elementalBallCounter.GetBallCount("fe");
                }
                CurrentLevel = obj.name; // 這裡會觸發事件
                return obj.name;
            }
        }
        CurrentLevel = "null";
        return "null";
    }
}
