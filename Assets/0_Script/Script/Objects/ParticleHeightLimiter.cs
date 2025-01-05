using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHeightLimiter : MonoBehaviour
{
    public ParticleSystem particleSystem; // ﹚采╰参
    public float maxHeight = 5f;          // 程蔼

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
            // 浪琩采蔼
            if (particles[i].position.y >= maxHeight)
            {
                // 盢采逞緇ネ㏑丁砞 0琵ウミア
                particles[i].position = new Vector3(particles[i].position.x, maxHeight, particles[i].position.z );
                //particles[i].remainingLifetime = 0.0f;
            }
        }

        // 穝采╰参
        particleSystem.SetParticles(particles, particleCount);
    }
}
