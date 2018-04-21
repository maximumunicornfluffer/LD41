using Assets.Scripts.PlayerManagement;
using UnityEngine;

namespace DefaultNamespace
{
  public class Character : MonoBehaviour
  {
    [SerializeField]
    private float _moveForceRatio = 100;

    public Stuff _resource;
    
    public PlayerInput _input;
    
    private Rigidbody2D _rb;

    void Awake()
    {
      _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
      if (_input == null)
        return;

      var stickVector = new Vector2(_input.H1(), _input.V1());
      
      _rb.AddForce(stickVector * _moveForceRatio);
    }
  }
}