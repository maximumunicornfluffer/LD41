using UnityEngine;

namespace DefaultNamespace.UI
{
  public class FillBar : MonoBehaviour
  {
    [SerializeField]
    private Sprite[] _normalSprites;
    
    private Animator _animator;

    private bool _warning=false;
    private float _value;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
      _animator = GetComponent<Animator>();
      _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetValue(float value)
    {
      _value = value;
    }

    public void SetWarning(bool warning)
    {
      _warning = warning;
      _animator.enabled = warning;
    }

    private void Update()
    {
      if (!_warning)
      {
        var index = (int)((_normalSprites.Length-1)* _value);
        _spriteRenderer.sprite = _normalSprites[index];
      }
    }
  }
}