using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem impactEffect;   // 觸碰後生成的新Particle System
    [SerializeField] private int maxSpawnedEffects = 10;    // 限制最大數量
    [SerializeField] private float lifeTime = 1f;           // 生命週期
    [SerializeField] private string targetName = "";        // 限制碰撞目標名稱
    private List<GameObject> spawnedEffects = new List<GameObject>();
    private ParticleSystem myParticleSystem;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (targetName != "" && other.name != targetName)
            return;


        if (spawnedEffects.Count >= maxSpawnedEffects)
            return; // 如果超過最大數量則不生成

        int numCollisionEvents = myParticleSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 collisionPoint = collisionEvents[i].intersection; // 取得接觸點
            Quaternion rotation = Quaternion.LookRotation(collisionEvents[i].normal); // 讓粒子面對接觸面

            // 生成新的 Particle System
            GameObject newEffect = Instantiate(impactEffect.gameObject, collisionPoint, rotation);
            spawnedEffects.Add(newEffect);

            // 啟動協程：等待 X 秒後刪除 & 從 List 移除
            StartCoroutine(DestroyEffectAfterTime(newEffect, lifeTime));
        }
    }

    private IEnumerator DestroyEffectAfterTime(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 先從 List 移除
        spawnedEffects.Remove(effect);

        // 再刪除物件
        Destroy(effect);
    }
}
