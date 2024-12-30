using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Card : MonoBehaviour
{
    public GameObject[] detectItems; // 更改為陣列以處理多個 detectitem
    public CardPattern cardPattern;
    public MyGameManager gameManager;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable; // 新增 XRGrabInteractable 來處理 VR 交互

    // 用於紀錄卡牌初始位置和旋轉
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MyGameManager>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>(); // 獲取 Rigidbody

        // 紀錄初始位置和旋轉
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // 監聽選擇退出事件
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

  

    void OnDestroy()
    {
        // 移除監聽事件
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectExited(SelectExitEventArgs args)
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
        if (gameManager.ReadyToCompareCards)
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
}

public enum CardPattern
{
    一, 二, 三, 四, 五, 六
}