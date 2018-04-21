using DefaultNamespace;
using UnityEngine;

public enum IEType {
    Resource,
    Machine,
}

    public abstract class IE: MonoBehaviour
    {
        public SpriteRenderer m_spriteRenderer;
        
        public abstract IEType Type { get; }

        protected abstract void Highlight(bool highlight);

        private void Awake()
        {
            GameManager.Instance.IEManager.Register(this);
        }

        private void OnDestroy()
        {
            GameManager.Instance.IEManager.Unregister(this);
        }

        void OnTriggerEnter2D(Collider2D other) {
            Highlight(true);
            var characterLogic = other.GetComponent<CharacterLogic>();
            if (characterLogic)
                characterLogic.Register(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Highlight(false);
            var characterLogic = other.GetComponent<CharacterLogic>();
            if (characterLogic)
                characterLogic.Unregister(this);
        }

        public abstract void UpdateLoop();
    }
