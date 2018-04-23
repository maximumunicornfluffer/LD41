using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ScoreManager : MonoBehaviour
    {
        private string scorePattern = "score : {0}";
        private int _score;

        public Text ScoreText;

        public void Start()
        {
            _score = 0;
			ScoreText.text = GetScoreText();
        }

        public void Add(int points)
        {
            _score += points;
			ScoreText.text = GetScoreText();
        }

		public void Reset() {
			_score = 0;
		}

		public string GetScoreText() {
			return string.Format(scorePattern, _score.ToString());
		}
        
    }
}