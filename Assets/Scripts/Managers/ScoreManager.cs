using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text scoreboardText;
    int score = 0;
    int maxScore = 1000;
    int minScore = 0;


    public void ScoreBoardProcess (int amount)
    {
        if (gameManager.Gameover) return;
        score += amount;
        int newScore = Mathf.Clamp(score, minScore, maxScore);
        scoreboardText.text =newScore.ToString("D4");
    }



}
