
using UnityEngine;

using UnityEngine.UI;



using DefaultNamespace;

namespace DefaultNamespace
{
	public class TimeManager : MonoBehaviour
	{
		private string scorePattern = "Time : {0}";
		private float _remainingTime;
		private int _previousRemainingTime;
		public int maxTime = 180;

		public Text TimeText;
		public AudioClip _bipAudioClip;

		private AudioSource _bipAudioSource;

		public void Start()
		{
			_bipAudioSource = gameObject.AddComponent<AudioSource>();
			_bipAudioSource.volume =  1.0f;
			_bipAudioSource.loop = false;
			_bipAudioSource.clip = _bipAudioClip;
			_bipAudioSource.playOnAwake = false;

			_remainingTime = (float)maxTime;
			TimeText.text = string.Format(scorePattern, _remainingTime.ToString());
		}
			
		void Update() {
			_remainingTime = _remainingTime - Time.deltaTime;
			int roundTime = (int)Mathf.Max(Mathf.Ceil(_remainingTime), 0);

			if (_previousRemainingTime != roundTime) {
				_previousRemainingTime = roundTime;

				if (roundTime <= 10 && roundTime > 0) {
					_bipAudioSource.Play();
					TimeText.color = Color.red;
				}
			}

			TimeText.text = string.Format(scorePattern, roundTime.ToString());
		}

		public bool IsGameOver() {
			return _remainingTime < 0;
		}

	}
}