using UnityEngine;

namespace DefaultNamespace
{
  public class LevelData : MonoBehaviour
  {
    [SerializeField] private Transform[] _startPoints;
    [SerializeField] private Transform[] _clientPoints;
    
    public Transform[] StartPoints
    {
      get { return _startPoints; }
    }

    public Transform[] ClientPoints => _clientPoints;
  }
}