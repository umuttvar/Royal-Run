using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem collisionParticleSystyem;
    [SerializeField] float shakeModifier = 10f;
    CinemachineImpulseSource cinemachineImpulseSource;

    float cooldownTimer = 0f;
    float VFXWaitTime = 1f;

    void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        ProcessImpulse();
        CollisionFX(other);
        ProcessCooldownVFX();
    }
    private void ProcessImpulse()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float shakeIntensity = (1f / distance) * shakeModifier;
        shakeIntensity = Mathf.Min(shakeIntensity, 1f);
        cinemachineImpulseSource.GenerateImpulse(shakeIntensity);
    }

    void CollisionFX(Collision other)
    {
        ContactPoint contactPoint = other.contacts[0];
        collisionParticleSystyem.transform.position = contactPoint.point;
        collisionParticleSystyem.Play();
    }
    
    void ProcessCooldownVFX()
    {
        if (cooldownTimer < VFXWaitTime) return;
        audioSource.Play();
        cooldownTimer = 0f;
        
    }

    
}
