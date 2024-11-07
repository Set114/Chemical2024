using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterandGlass : MonoBehaviour
{ 
    public string iceTag = "ice";    // 冰的標籤
    public string glassTag = "glass";  // 玻璃的標籤

    private bool isIceEntered = false;  // 記錄冰是否已進入
    private bool isGlassEnteredAfterIce = false;  // 記錄玻璃是否在冰進入後進入

    private void OnTriggerEnter(Collider other)
    {
        // 當 "冰" 進入時，標記 isIceEntered 為 true
        if (other.CompareTag(iceTag))
        {
            isIceEntered = true;
            
            Debug.Log("1");
        }

        // 當 "玻璃" 進入時，檢查冰是否已經進入
        if (other.CompareTag(glassTag) && isIceEntered)
        {
            isGlassEnteredAfterIce = true; // 標記玻璃在冰之後進入
            Debug.Log("2");
            // 找到 CollisionDetection1 並設定 a 為 true
            CollisionDetection1 collisionScript = FindObjectOfType<CollisionDetection1>();
            if (collisionScript != null && isGlassEnteredAfterIce)
            {
                Debug.Log("3");
                collisionScript.a = true;
            }
        }
    }
}
