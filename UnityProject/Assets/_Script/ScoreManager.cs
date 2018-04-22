using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ScoreManager : MonoBehaviour
    {
        private int _score;

        public Text ScoreText;

        public void Start()
        {
            _score = 0;
            ScoreText.text = _score.ToString();
        }

        public void Add(int points)
        {
            _score += points;
            ScoreText.text = _score.ToString();
        }
        
    }
}