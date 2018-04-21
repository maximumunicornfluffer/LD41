using UnityEngine;

    public class IE: MonoBehaviour
    {
        public SpriteRenderer m_spriteRenderer;
        public BoxCollider2D m_collider;
        
        void OnTriggerEnter2D(Collider2D other) {
              m_spriteRenderer.color = Color.red;  
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            m_spriteRenderer.color = Color.white;
        }
    }
