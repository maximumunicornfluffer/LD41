using States;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class SplashScreen : MonoBehaviour
    {

        public Animator m_poneyAnimator;

        void Awake()
        {
            m_poneyAnimator.speed = 0;
        }

        public void StartPoneyAnimation()
        {
            m_poneyAnimator.speed = 1;
        }

        public void OnAnimEnd()
        {
            FSM.Instance.GotoState<TitleState>();
        }
        
    }
}