
using UnityEngine;

using UnityEngine.UI;



using DefaultNamespace;

namespace DefaultNamespace
{
	public class TimeManager : MonoBehaviour
	{
		private string scorePattern = "Time : {0}";
		private float _remainingTime;
		public int maxTime = 180;

		public Text TimeText;


		public void Start()
		{
			_remainingTime = (float)maxTime;
			TimeText.text = string.Format(scorePattern, _remainingTime.ToString());
		}
			
		void Update() {
			_remainingTime = _remainingTime - Time.deltaTime;
			int roundTime = (int)Mathf.Ceil(_remainingTime);
			TimeText.text = string.Format(scorePattern, roundTime.ToString());

		}

		public bool IsGameOver() {
			return _remainingTime < 0;

		}

	}
}