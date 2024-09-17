using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTransTest : MonoBehaviour
{
    public ParticleSystem skillParticle;
    private ParticleSystem.Particle[] particles; // 파티클 배열

    void Start()
    {
        // 파티클 배열 초기화
        particles = new ParticleSystem.Particle[skillParticle.main.maxParticles];
    }

    void Update()
    {
        // 파티클 데이터를 가져오기
        int particleCount = skillParticle.GetParticles(particles);

        // 첫 번째 파티클의 위치를 추적 (다른 파티클들도 반복문을 통해 조회 가능)
        if (particleCount > 0)
        {
            Vector3 firstParticlePosition = particles[0].position;            
            GetComponent<BoxCollider>().center = firstParticlePosition;
        }
    }
}
