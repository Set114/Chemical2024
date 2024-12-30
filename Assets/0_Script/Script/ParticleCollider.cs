using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ParticleCollider : MonoBehaviour
{
    public List<GlucoseScaleCube> glucoseScaleCubeScripts = new List<GlucoseScaleCube>();
    public GameObject powder1;
    public GameObject powder2;
    public Vector3 startScale = Vector3.zero; // 起始縮放
    public Vector3 endScale = new Vector3(56.1727f, 55.89342f, 65.55539f); // 結束縮放
    public float duration = 2.0f; // 動畫持續時間

    private void OnParticleTrigger()
    {
        ParticleSystem ps = transform.GetComponent<ParticleSystem>();

        List<ParticleSystem.Particle> Enter = new List<ParticleSystem.Particle>();

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, Enter);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = Enter[i];
            p.remainingLifetime = 0.9f;
            Enter[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, Enter);

        if (numEnter > 0)
        {
            foreach (var glucoseScaleCubeScript in glucoseScaleCubeScripts)
            {
                if (glucoseScaleCubeScript != null)
                {
                    glucoseScaleCubeScript.isParticleTriggered = true;
                }
            }

            if (powder1 != null)
            {
                // 将 a1 对象缩小
                powder1.transform.localScale *= 0.9f; // 假设缩小 10%
                float elapsedTime = 0;
                StartCoroutine(ScaleOverTime(powder2, startScale, endScale, duration));

            }
        }
    }
    private IEnumerator ScaleOverTime(GameObject obj, Vector3 initialScale, Vector3 targetScale, float time)
    {

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            // 計算插值
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            // 等待下一幀
            yield return null;
        }

        // 確保縮放完全到達目標值
        obj.transform.localScale = targetScale;
    }
}
