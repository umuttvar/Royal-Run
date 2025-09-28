using UnityEngine;
using TMPro;

public class Coin : Pickup
{
    ScoreManager scoreManager;

    public void Init(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    protected override void OnPickup()
    {
    if(scoreManager != null)
        {
            scoreManager.ScoreBoardProcess(10);
        }
    }
}

