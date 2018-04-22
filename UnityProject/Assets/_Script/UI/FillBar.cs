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

      _animator.enabled = _warning;
      UpdateSprites();
    }

    public void SetValue(float value)
    {
      _value = Mathf.Clamp01(value);
    }

    public void SetWarning(bool warning)
    {
      _warning = warning;
      if (_animator)
        _animator.enabled = warning;
    }

    private void Update()
    {
      UpdateSprites();
    }

    private void UpdateSprites()
    {
      if (_spriteRenderer && !_warning)
      {
        var index = (int) ((_normalSprites.Length - 1) * _value);
        _spriteRenderer.sprite = _normalSprites[index];
      }
    }
  }
}