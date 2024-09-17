using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTransTest : MonoBehaviour
{
    public ParticleSystem skillParticle;
    private ParticleSystem.Particle[] particles; // ��ƼŬ �迭

    void Start()
    {
        // ��ƼŬ �迭 �ʱ�ȭ
        particles = new ParticleSystem.Particle[skillParticle.main.maxParticles];
    }

    void Update()
    {
        // ��ƼŬ �����͸� ��������
        int particleCount = skillParticle.GetParticles(particles);

        // ù ��° ��ƼŬ�� ��ġ�� ���� (�ٸ� ��ƼŬ�鵵 �ݺ����� ���� ��ȸ ����)
        if (particleCount > 0)
        {
            Vector3 firstParticlePosition = particles[0].position;            
            GetComponent<BoxCollider>().center = firstParticlePosition;
        }
    }
}
