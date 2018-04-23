using States;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class SplashScreen : MonoBehaviour
    {

        public Animator m_poneyAnimator;
		private AudioSource _mufAudio;

        void Awake()
        {
            m_poneyAnimator.speed = 0;
			_mufAudio = gameObject.GetComponent<AudioSource>();
        }

        public void StartPoneyAnimation()
        {
            m_poneyAnimator.speed = 1;
			_mufAudio.Play();
        }

        public void OnAnimEnd()
        {
            FSM.Instance.GotoState<TitleState>();
        }
        
    }
}