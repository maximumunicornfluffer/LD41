using System;
using UnityEngine;

namespace DefaultNamespace
{
  public class StuffInHandDisplayer : MonoBehaviour
  {
    [SerializeField] private Sprite _waterSprite;
    [SerializeField] private Sprite _potatoesSprite;
    [SerializeField] private Sprite _friesSprite;
    [SerializeField] private Sprite _corpsSprite;
    [SerializeField] private Sprite _cinderSprite;

    [SerializeField] private SpriteRenderer _icon;

    public void SetStuffInHand(StuffType type)
    {
      switch (type)
      {
        case StuffType.None:
          gameObject.SetActive(false);
          break;
        case StuffType.Water:
          _icon.sprite = _waterSprite;
          gameObject.SetActive(true);
          break;
        case StuffType.Potatoes:
          _icon.sprite = _potatoesSprite;
          gameObject.SetActive(true);
          break;
        case StuffType.Fries:
          _icon.sprite = _friesSprite;
          gameObject.SetActive(true);
          break;
        case StuffType.Corpes:
          _icon.sprite = _corpsSprite;
          gameObject.SetActive(true);
          break;
        case StuffType.Cinder:
          _icon.sprite = _cinderSprite;
          gameObject.SetActive(true);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(type), type, null);
      }
      
    }
    
    public SpriteRenderer Renderer
    {
      get { return _icon; }
    }
  }
}