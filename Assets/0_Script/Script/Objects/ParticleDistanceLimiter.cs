using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDistanceLimiter : MonoBehaviour
{
    public ParticleSystem particleSystem; // ���w�A���ɤl�t��
    public float maxDistance = 5f;          // �̤j����

    private ParticleSystem.Particle[] particles;

    void Update()
    {
        if (particleSystem == null) return;

        int maxParticles = particleSystem.main.maxParticles;
        if (particles == null || particles.Length < maxParticles)
        {
            particles = new ParticleSystem.Particle[maxParticles];
        }

        int particleCount = particleSystem.GetParticles(particles);

        for (int i = 0; i < particleCount; i++)
        {
            // �ˬd�ɤl������
            if ( Vector3.Distance( particles[i].position, this.transform.position ) > maxDistance )
            {
                // �N�ɤl���Ѿl�ͩR�ɶ��]�� 0�A�����ߧY����
                particles[i].velocity = Vector3.zero;// position = new Vector3(particles[i].position.x, maxDistance, particles[i].position.z);
                //particles[i].remainingLifetime = 0.0f;
            }
        }

        // ��s�ɤl�t��
        particleSystem.SetParticles(particles, particleCount);
    }
}
