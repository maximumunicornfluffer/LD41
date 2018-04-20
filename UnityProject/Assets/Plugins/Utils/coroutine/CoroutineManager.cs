using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{


    private static CoroutineManager s_instance = null;
    
    public static CoroutineManager Instance
    {
        get { return s_instance; }
    }

    public delegate void CoroutineFinishedCallback();

    void Start()
    {
        s_instance = this;
        Debug.Log("CoroutineManager Start");
    }

    public void StartCoroutine(IEnumerator coroutine, CoroutineFinishedCallback finishedCallBack)
    {
        StartCoroutine(InnerCoroutine(coroutine, finishedCallBack));
    }

    private IEnumerator InnerCoroutine(IEnumerator coroutine, CoroutineFinishedCallback finishedCallBack)
    {
        yield return StartCoroutine(coroutine);

        // use to wait one frame in order to have start & awake
        yield return null;

        if(finishedCallBack!=null)
            finishedCallBack();
    }

    public void StartCoroutines(List<IEnumerator> coroutines, CoroutineFinishedCallback finishedCallBack)
    {
        StartCoroutine(InnerCoroutines(coroutines, finishedCallBack));
    }

    private IEnumerator InnerCoroutines(List<IEnumerator> coroutines, CoroutineFinishedCallback finishedCallBack)
    {
        List<Coroutine> cs = new List<Coroutine>();
        foreach (var enumerator in coroutines)
        {
            cs.Add(StartCoroutine(enumerator));
        }

        foreach (var c in cs)
        {
            yield return c;
        }
        
        yield return null;
        
        if(finishedCallBack!=null)
            finishedCallBack();
    }
         
}
