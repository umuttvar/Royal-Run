using System.Collections;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    
    [SerializeField] Animator animator;
    [SerializeField] float stumbleWaitTime = 1f;
    [SerializeField] float adjustChangeMoveSpeedAmount = -2f;
    float cooldownTimer = 0f;
    const string stumbleAnimaton = "Hit";

    LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = FindAnyObjectByType<LevelGenerator>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (cooldownTimer < stumbleWaitTime) return;
        levelGenerator.ChangeChunkMovespeed(adjustChangeMoveSpeedAmount);
        animator.SetTrigger(stumbleAnimaton);
        cooldownTimer = 0f;
    }

   

}
