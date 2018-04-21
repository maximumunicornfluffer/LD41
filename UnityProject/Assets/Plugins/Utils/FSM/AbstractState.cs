using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.Utils.FSM
{
  public abstract class AbstractState
  {

    private StateStatus _status = StateStatus.Waiting;
    private List<string> _dynamicScenesName;

    public abstract string[] GetStaticSceneName();
    
    public List<Scene> _loadedScenes = new List<Scene>();

    public StateStatus Status
    {
      get { return _status; }
    }

    public List<string> DynamicScenesName
    {
      get { return _dynamicScenesName; }
    }

    public void Init(List<string> dynamicSceneNames)
    {
      if(dynamicSceneNames==null)
        _dynamicScenesName = new List<string>();
      else
        _dynamicScenesName = dynamicSceneNames;
    }
    
    public virtual void Update()
    {}

    public virtual void OnEnter()
    {
      _status = StateStatus.Entering;
      List<IEnumerator> enumerators = new List<IEnumerator>();
      foreach (var sceneName in _dynamicScenesName)
      {
        enumerators.Add(LoadScene(sceneName));
      }

      var staticSceneName = GetStaticSceneName();
      if (staticSceneName != null)
      {
        foreach (var sceneName in GetStaticSceneName())
        {
          enumerators.Add(LoadScene(sceneName));
        }
      }

      CoroutineManager.Instance.StartCoroutines(enumerators,OnScenesLoaded);
    }
    
    public virtual void OnLeave() {
      _status = StateStatus.Leaving;
      List<IEnumerator> enumerators = new List<IEnumerator>();
      foreach (var scene in _loadedScenes)
      {
        enumerators.Add(UnloadScene(scene));
      }
      CoroutineManager.Instance.StartCoroutines(enumerators,OnScenesUnLoaded);
    }

    private IEnumerator LoadScene(string sceneName)
    {
      var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
      loadOperation.allowSceneActivation = false;

      while (loadOperation.progress<0.9f)
      {
        yield return null;
      }

      loadOperation.allowSceneActivation = true;
      yield return loadOperation;
      
      var scene = SceneManager.GetSceneByName(sceneName);

      CreateDisabledUniqueRoot(scene);
      _loadedScenes.Add(scene);
    }

    private void CreateDisabledUniqueRoot(Scene scene)
    {
      var rootGameObjects = scene.GetRootGameObjects();
      var root = new GameObject("Root_"+scene.name);
      root.SetActive(false);
      SceneManager.MoveGameObjectToScene(root,scene);
      foreach (var o in rootGameObjects)
      {
        o.transform.SetParent(root.transform,true);
      }
    }
    
    private void OnScenesLoaded()
    {

      foreach (var scene in _loadedScenes)
      {
        scene.GetRootGameObjects()[0].SetActive(true);
      }
      _status = StateStatus.Running;
    }

    private IEnumerator UnloadScene(Scene scene)
    {
      var unloadOperation = SceneManager.UnloadSceneAsync(scene);
      while (!unloadOperation.isDone)
      {
        yield return null;
      }

      _loadedScenes.Remove(scene);
    }
    
    private void OnScenesUnLoaded()
    {
      _status = StateStatus.LeaveEnded;
    }
  }


}