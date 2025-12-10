using System;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;

    private int _currentScores;
    private int _bestScore = 0;
    public int CurrentScores => _currentScores;

    private void Start()
    {
        _currentScores = 0;
        _bestScore = PlayerPrefs.GetInt("BestScore");
        bestScoreText.text = $"Best Score\n{PlayerPrefs.GetInt("BestScore")}";
        UpdateScoreText();
    }

    private void OnEnable()
    {
        GameEvents.AddScores += AddScores;
        GameEvents.GameOver += SaveBestScore;
    }

    private void OnDisable()
    {
        GameEvents.AddScores -= AddScores;
        GameEvents.GameOver -= SaveBestScore;
    }

    private void AddScores(int scores)
    {
        _currentScores += scores;
        UpdateScoreText();
    }

    private void SaveBestScore(bool tem)
    {
        if(_currentScores > _bestScore)
        {
            PlayerPrefs.SetInt("BestScore", _currentScores);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score\n{_currentScores}";
        if (_currentScores > _bestScore)
        {
            bestScoreText.text = $"Best Score\n{_currentScores}";
        }
    }
}
