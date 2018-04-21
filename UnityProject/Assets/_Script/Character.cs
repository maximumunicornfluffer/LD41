using Assets.Scripts.PlayerManagement;
using UnityEngine;

namespace DefaultNamespace
{
  public class Character : MonoBehaviour
  {
    [SerializeField]
    private float _moveForceRatio = 100;
    [SerializeField]
    private CharacterLogic m_logic;

    
    public PlayerInput _input;

    
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
      _rb = GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();
      _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
      if (_input == null)
        return;

      var stickVector = new Vector2(_input.H1(), _input.V1());

      if (stickVector.x > 0)
        _spriteRenderer.flipX = true;
      else if(stickVector.x<0)
        _spriteRenderer.flipX = false;
      
      if(stickVector.sqrMagnitude>0)
        _animator.SetBool("isWalking", true);
      else _animator.SetBool("isWalking",false);
      
      _rb.AddForce(stickVector * _moveForceRatio);

      if (_input.ADown())
      {
        m_logic.Act();
      }
    }
    
    
    
  }
}