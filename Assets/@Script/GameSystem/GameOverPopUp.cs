using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPopup;
    [SerializeField]
    private TextMeshProUGUI finalScoreText;
    [SerializeField]
    private Scores score;

    private void Start()
    {
        gameOverPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
    }

    private void OnGameOver(bool newBestScore)
    {
        AudioManager.Instance.PlayBestRecordClip();
        gameOverPopup.SetActive(true);
        finalScoreText.text = $"Á¡¼ö\n{score.CurrentScores}";
    }
}
