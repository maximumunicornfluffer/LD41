using UnityEngine;

namespace DefaultNamespace
{
  public class LevelData : MonoBehaviour
  {
    [SerializeField] private Transform[] _startPoints;
    
    public Transform[] StartPoints
    {
      get { return _startPoints; }
    }
  }
}