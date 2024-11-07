using UnityEngine;

public class PlayAnimationOnPickup : MonoBehaviour
{
    // CFXR 粒子系统
    public ParticleSystem particleSystemToPlay;

    private bool pickedUp = false; // 是否已经拿起了火

    private void OnTriggerEnter(Collider other)
    {
        // 如果玩家触摸到 "fire" 物体并且还没有拿起火
        if (other.CompareTag("fire") && !pickedUp)
        {
            // 播放粒子系统
            particleSystemToPlay.Play();
            pickedUp = true;
        }
    }
}
