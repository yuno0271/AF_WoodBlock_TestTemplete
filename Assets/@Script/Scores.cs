using System;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private int _currentScores;

    private void Start()
    {
        _currentScores = 0;
        UpdateScoreText();
    }

    private void OnEnable()
    {
        GameEvents.AddScores += AddScores;
    }

    private void OnDisable()
    {
        GameEvents.AddScores -= AddScores;
    }

    private void AddScores(int scores)
    {
        _currentScores += scores;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score\n" + _currentScores.ToString();
    }
}
