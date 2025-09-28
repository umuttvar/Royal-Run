using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] float startTime = 25f;


    float timeLeft;
    bool gameOver = false;

    public bool Gameover => gameOver;


    void Start()
    {
        timeLeft = startTime;
    }

    void Update()
    {
        DecreaseTime();

    }

    void DecreaseTime() //süre bittiğinde zaman yavaşlayacak ve GameOver metodu çağırılacak
                                
    {
        if (gameOver) return;//gameOver true ise metoddan çıkacak
        
            timeLeft -= Time.deltaTime;
            timeText.text = timeLeft.ToString("F1"); // F1 ile sadece 1 ondalık kısmın gözükmesini sağlıyoruz 0.1 0.2 gibi

            if (timeLeft <= 0) //süre bittiğinde GameOver metodu çağırılır.
            {
                GameOver();
            }
    }

    void GameOver() //GameOver metodunda bool değişkenimiz gameOver true olarak işaretlendiği için
                    //if ile return edip DescreaseTime metodundan çıkacak.
    {
        gameOver = true;
        gameOverText.SetActive(true);
        Time.timeScale = .1f; // oyunu yavaşlatmak için kullanılır.
        playerController.enabled = false;
    }

    public void IncreaseTime (float amount)
    {
        timeLeft += amount;
    }
}
