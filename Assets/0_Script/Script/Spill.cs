using UnityEngine;
using System.Collections;

public class Spill : MonoBehaviour
{
    public GameObject myParticleSystem;
    public GameObject a1;
    public float checkInterval = 0.1f; // 检查间隔时间

    void Start()
    {
        // 开始协程来检测旋转角度
        StartCoroutine(CheckRotation());
    }

    private IEnumerator CheckRotation()
    {
        while (true)
        {
            // 获取 a1 的 z 轴旋转角度
            float rotationZ = a1.transform.eulerAngles.z;

            // 如果 z 轴旋转角度超过 90 度，则播放粒子系统
            if (rotationZ > 90f && rotationZ < 180f)
            {
                myParticleSystem.SetActive(true);
            }
            else
            {
                myParticleSystem.SetActive(false);
            }

            // 等待一段时间后再进行下一次检查
            yield return new WaitForSeconds(checkInterval);
        }
    }
}
