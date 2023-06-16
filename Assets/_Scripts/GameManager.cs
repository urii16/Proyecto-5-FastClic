using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        inGame,
        gameOver
    }

    public GameState gameState;

    public List<GameObject> targetPrefabs;

    public float spawnRate = 1f;

    public TextMeshProUGUI scoreText;
    private int score;

    public TextMeshProUGUI gameOverText;

    public Button resetButton;

    public GameObject panelScreen;

    private int numberOfLives = 4;

    public List<GameObject> lives;


    public void Start()
    {
        ShowMaxScore();
    } 

    IEnumerator SpawnTarget()
    {
        while (gameState == GameState.inGame)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);
            Instantiate(targetPrefabs[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        if(score < 0)
        {
            score = 0;
        }
        scoreText.text = "Score: " + score;
    }

    public void ShowMaxScore()
    {
        int maxScore = PlayerPrefs.GetInt("MAX_SCORE", 0);
        scoreText.text = "Max score: "+maxScore;
    }

    private void SetMaxScore()
    {
        int maxScore = PlayerPrefs.GetInt("MAX_SCORE", 0);
        if (score < maxScore)
        {
            PlayerPrefs.SetInt("MAX_SCORE", score);
        }
    }

    public void StartGame(int difficulty)
    {
        gameState = GameState.inGame;
        spawnRate /= difficulty;
        numberOfLives -= difficulty;

        for (int i = 0; i < numberOfLives; i++)
        {
            lives[i].SetActive(true);
        }

        StartCoroutine(SpawnTarget());
        gameOverText.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        panelScreen.gameObject.SetActive(false);
        score = 0;
        UpdateScore(0);
    }

    public void GameOver()
    {
        numberOfLives--;
        if(numberOfLives >= 0)
        {
            Image heartImage = lives[numberOfLives].GetComponent<Image>();
            var tempColor = heartImage.color;
            tempColor.a = 0.3f;
            heartImage.color = tempColor;
        }
        

        if(numberOfLives <= 0)
        {
            SetMaxScore();
            resetButton.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);
            gameState = GameState.gameOver;
        }
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
