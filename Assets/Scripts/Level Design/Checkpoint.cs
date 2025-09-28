using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float increaseTime = 5f;
    GameManager gameManager;
    const string playerString = "Player";

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerString))
        {
            if(gameManager != null)
            {
                gameManager.IncreaseTime(increaseTime);
            }
        }
        
    }
}
