using System;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TMP_Text sessionHighScore;
        [SerializeField] private TMP_Text currentScore;
        
        [Header("Restart UI")]
        [SerializeField] private CanvasGroup restartUi;
        [SerializeField] private Button restartButton;

        private int score;
        private int highScore;

        public event Action OnRestartPressed;

        private void Awake() => restartButton.onClick.AddListener(RestartGame);
        
        private void Start() => ResetPoints();

        private void RestartGame()
        {
            score = 0;
            ResetPoints();
            OnRestartPressed?.Invoke();
            restartUi.ShowCanvasGroup(false);
        }
        
        private void ResetPoints()
        {
            sessionHighScore.SetText(highScore.ToString());
            currentScore.SetText(score.ToString());
        }

        public void AddPoint()
        {
            score++;
            currentScore.SetText(score.ToString());
        }

        public void ShowRestartScreen()
        {
            restartUi.ShowCanvasGroup(true);
            if (score > highScore) highScore = score;
            sessionHighScore.SetText(highScore.ToString());
        }
    }
}