using States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
  public class MainInitializer : MonoBehaviour
  {

    private void Start()
    {
      SceneManager.LoadSceneAsync("Default",LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
      if (CoroutineManager.Instance == null)
        return;

      if (FSM.Instance == null)
        return;

      if (InputsManager.Instance == null)
        return;


			FSM.Instance.GotoState<TitleState>();

      Destroy(this);
      

    }
  }
}