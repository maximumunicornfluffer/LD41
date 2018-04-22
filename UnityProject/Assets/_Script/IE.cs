using DefaultNamespace;
using UnityEngine;

public enum IEType {
    Resource,
    Machine,
}

    public abstract class IE: MonoBehaviour
    {
        public SpriteRenderer m_spriteRenderer;
        public GameObject m_selectedIE;

        private int _mActivatorCount = -1;
        
        public abstract IEType Type { get; }

        protected abstract void Highlight(bool highlight);

        protected virtual void Awake()
        {
            GameManager.Instance.IEManager.Register(this);
            ActivatorCount = 0;
        }

        private void OnDestroy()
        {
            GameManager.Instance.IEManager.Unregister(this);
        }

        private int ActivatorCount
        {
            get { return _mActivatorCount; }
            set
            {
                if (value == _mActivatorCount)
                    return;
                _mActivatorCount = value;
                SetSelectedIE(_mActivatorCount != 0);
            }
        }
        
        public void SetSelectedIE(bool selected)
        {
            m_selectedIE.SetActive(selected);
        }
        
        void OnTriggerEnter2D(Collider2D other) {
            var characterLogic = other.GetComponent<CharacterLogic>();
            if (characterLogic)
            {
                characterLogic.Register(this);
                ActivatorCount = ActivatorCount + 1;
                Highlight(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var characterLogic = other.GetComponent<CharacterLogic>();
            if (characterLogic)
            {
                characterLogic.Unregister(this);
                ActivatorCount = ActivatorCount - 1;
                Highlight(false);
            }
        }

        public abstract void UpdateLoop();

        private void Update()
        {
            var pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x, pos.y, -pos.y);
        }
    }
