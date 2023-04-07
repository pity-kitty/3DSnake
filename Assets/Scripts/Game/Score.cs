using TMPro;
using UnityEngine;

namespace Game
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TMP_Text sessionHighScore;
        [SerializeField] private TMP_Text currentScore;

        private int score;
        private int highScore;
        
        private void Start()
        {
            sessionHighScore.SetText(highScore.ToString());
            currentScore.SetText(score.ToString());
        }

        public void AddPoint()
        {
            score++;
            currentScore.SetText(score.ToString());
        }

        public void ResetPoints()
        {
            if (score > highScore) highScore = score;
            score = 0;
            sessionHighScore.SetText(highScore.ToString());
            currentScore.SetText(score.ToString());
        }
    }
}