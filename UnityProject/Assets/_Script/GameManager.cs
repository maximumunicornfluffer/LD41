using UnityEngine;

namespace DefaultNamespace
{
  public class GameManager : MonoBehaviour
  {
    #region Singleton

    private static GameManager s_instance;

    public static GameManager Instance
    {
      get { return s_instance; }
    }
    #endregion
    
    public Character _characterPrefab;
    
    void Awake()
    {
      s_instance = this;
    }
  }
}