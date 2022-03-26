using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public TextMeshProUGUI livesText, scoreText;
    int score, lives;
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = "Lives: " + lives.ToString();
        scoreText.text = "Score: " + score.ToString();
    }

    public void ReduceLife()
    {
        lives--;
        if (lives < 0)
        {
            lives = 0;
            //game over
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void AddScore()
    {
        score += 100;
    }

    public void GameWin()
    {
        gameWinUI.SetActive(true);
        Time.timeScale = 0;
    }
}
