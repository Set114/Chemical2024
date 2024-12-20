using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HammerIronInteraction : MonoBehaviour
{
    public GameObject[] ironObjects; // 铁块对象数组
    public AudioClip hitSound; // 敲击音效
    public Image countImg; // UI中的图像组件
    public Sprite[] countSprites;

    private int currentIronIndex = 0; // 当前铁块索引
    private bool canHit = true; // 是否可以敲击铁块
    private WaitForSeconds hitCooldown = new WaitForSeconds(1f); // 等待1秒的 WaitForSeconds 对象
    [Header("UI")]
    [SerializeField] GameObject imageUI;
    

    public FeAniAnimationController feAniAnimationController;
    public LevelEndSequence levelEndSequence;
    private LevelObjManager levelObjManager;

    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        // 隐藏所有的铁块对象
        foreach (GameObject ironObject in ironObjects)
        {
            ironObject.SetActive(false);
        }

        // 显示第一个铁块对象
        ironObjects[0].SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        // 碰到铁块时触发
        if (canHit && other.CompareTag("iron"))
        {
            StartCoroutine(HitIronCoroutine());
        }
    }

    IEnumerator HitIronCoroutine()
    {
        canHit = false; // 设置为不可敲击状态

        // 播放敲击音效
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        // 如果还有下一个铁块
        if (currentIronIndex < ironObjects.Length - 1)
        {
            // 隐藏当前铁块
            ironObjects[currentIronIndex].SetActive(false);
            //Debug.Log("Hiding iron object: " + ironObjects[currentIronIndex].name);

            // 隐藏与当前铁块相关的盒碰撞器
            ironObjects[currentIronIndex].GetComponent<BoxCollider>().enabled = false;

            // 移动到下一个铁块
            currentIronIndex++;

            // 显示下一个铁块
            ironObjects[currentIronIndex].SetActive(true);
            //Debug.Log("Showing iron object: " + ironObjects[currentIronIndex].name);

            if (currentIronIndex == 1)
            {
                feAniAnimationController.ResumeAnimation();
                countImg.sprite = countSprites[0];
            }
            else if (currentIronIndex == 2)
            {
                feAniAnimationController.ResumeAnimation();
                countImg.sprite = countSprites[1];
            }
            else if (currentIronIndex == 3)
            {
                feAniAnimationController.ResumeAnimation();
                countImg.sprite = countSprites[2];
            }

            if (currentIronIndex == 3)
            {
                //levelEndSequence.EndLevel(false,true, 1f, 6f, 1f, "1", () => { });
                levelObjManager.LevelClear("1", "");
            }
        }

        // 等待一秒后可以再次敲击
        yield return hitCooldown;
        canHit = true; // 恢复可以敲击状态
    }

    // 更新 UI 图像的方法
    void UpdateUIImage()
    {
        if (currentIronIndex >= 0 && currentIronIndex < countSprites.Length)
        {
            countImg.sprite = countSprites[currentIronIndex];
        }
    }
}
