using UnityEngine;

namespace DefaultNamespace
{
    public class ScoreManager : MonoBehaviour
    {
        private int _score;

        public int Score => _score;

        public void Start()
        {
            _score = 0;
        }

        public void Add(int points)
        {
            _score += points;
        }
        
    }
}