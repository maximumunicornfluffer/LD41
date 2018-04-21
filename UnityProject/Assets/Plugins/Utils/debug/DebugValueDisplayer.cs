using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DebugValueDisplayer : MonoBehaviour
{

    private static DebugValueDisplayer s_instance;
    public static DebugValueDisplayer GetInstance()
    {
        if (s_instance == null)
        {
            var go = new GameObject("DebugValueDisplayer");
            s_instance = go.AddComponent<DebugValueDisplayer>();
            Object.DontDestroyOnLoad(go);
        }
        return s_instance;
    }

    public void AddValueToDisplay(string key, string value)
    {
        if (!m_valuesToDisplay.ContainsKey(key))
            m_valuesToDisplay.Add(key,value);

        m_valuesToDisplay[key] = value;
    }

    private void Start()
    {

    }

    void OnGUI()
    {
        GUILayout.BeginVertical("box");
        foreach (KeyValuePair<string, string> keyValuePair in m_valuesToDisplay)
        {
            GUILayout.Label(keyValuePair.Key + ":" + keyValuePair.Value);
        }
        GUILayout.EndVertical();
    }

    private void Update()
    {

    }

    private Dictionary<string,string> m_valuesToDisplay = new Dictionary<string, string>(); 
}