using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Card : MonoBehaviour
{
    public enum CardPattern
    {
        一, 二, 三, 四, 五, 六
    }

    public CardPattern cardPattern;
    public Exam_5_1 gameManager;

    private bool isFlipped = false; // 是否翻開

    int Status = 0;
    Quaternion targetAngle;
    Quaternion originAngle;
    float activeTime = 0.0f;
    float RotateCardTime = 0.4f;
    GameObject emptyParentForScale;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable; // 新增 XRGrabInteractable 來處理 VR 交互

    void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>(); // 獲取 Rigidbody

        this.transform.rotation = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 180.0f));

        // 監聽選擇退出事件
        //grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //待機

                break;
            case 1: //旋轉
                this.gameObject.transform.localRotation = Quaternion.Slerp(originAngle, targetAngle, (1.0f - ( activeTime - Time.time) / RotateCardTime));
                if (Time.time > activeTime)
                    Status = 0;
                break;
            case 2: //正確
                emptyParentForScale.transform.localScale = Mathf.Lerp(1.0f, 0.0f, (1.0f - (activeTime - Time.time) / RotateCardTime)) * Vector3.one;
                if (Time.time > activeTime)
                    Status = 3;
                break;
            case 3: //正確
                this.gameObject.SetActive(false);
                break;
        }
    }

    //翻牌
    public void FlipCard()
    {
        isFlipped = !isFlipped;
        // 顯示正面或背面
        UpdateCardVisual();
    }

    public void Matched()
    {
        activeTime = Time.time + RotateCardTime;
        emptyParentForScale = new GameObject("ScaleEmpty");
        emptyParentForScale.transform.position = this.transform.position;
        this.transform.parent = emptyParentForScale.transform;
        Status = 2;
    }

    public bool IsFlipped => isFlipped;
    private void UpdateCardVisual()
    {
        // 更新卡牌的顯示，例如顯示正面或背面
        // 可根據 isFlipped 和 isMatched 改變材質或顯示
        if (isFlipped)
            targetAngle = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f));
        else
            targetAngle = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 180.0f));
        originAngle = this.transform.localRotation;
        activeTime = Time.time + RotateCardTime;
        Status = 1;
    }

    public void OnCardClick()
    {
        gameManager.SendMessage("OnCardClicked", this);
    }

    /*
    void OnDestroy()
    {
        // 移除監聽事件
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        bool cardPlaced = false;

        foreach (GameObject detectItem in detectItems)
        {
            if (Vector3.Distance(transform.position, detectItem.transform.position) < 0.1f) // 調整檢測距離
            {
                MoveCardToDetectItemPosition(detectItem);
                cardPlaced = true;
                break; // 找到對應的 detectItem 就退出迴圈
            }
        }

        if (!cardPlaced)
        {
            // 如果卡牌沒有放置在任一 detectItem 上，則返回初始位置
            CloseCard();
            return; // 不做任何運算
        }

        // 碰撞後的其他處理
        HandleCollision();
    }

    private void MoveCardToDetectItemPosition(GameObject detectItem)
    {
        // 將卡牌位置設置為 detectitem 的位置加上一點高度
        transform.position = new Vector3(detectItem.transform.position.x, detectItem.transform.position.y + 0.01f, detectItem.transform.position.z);
        transform.rotation = detectItem.transform.rotation;

        // 凍結 Rigidbody 的所有運動
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void HandleCollision()
    {
        if (gameManager.cardComparison.Count == 2)
        {
            return; // 如果準備比對卡牌，則退出方法
        }
        // 將卡牌添加到比對清單中，進行比對
        gameManager.AddCardInCardComparison(this);
        gameManager.CompareCardsInList();
    }

    public void CloseCard()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    public void ChangeDetectItemsColor(Color newColor)
    {
        foreach (var item in detectItems)
        {
            var renderer = item.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
        }
    }
    */
}