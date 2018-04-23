using System;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class GameLogicExit : MonoBehaviour
    {
        private Animator m_animator;

        public event Action OnAnimCloseEnded;
        
        void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void PlayClose()
        {
            m_animator.Play("Close");
        }

        public void OnAnimEnded()
        {
            OnAnimCloseEnded?.Invoke();
        }
        
    }
}