using UnityEngine;
using System.Collections;

public class ManagerChecker : MonoBehaviour
{

    public GameObject ManagerToCheck;
    public bool DontDestroy;

    void Awake()
    {
        MonoBehaviour behavior = ManagerToCheck.GetComponents<MonoBehaviour>()[0];

        Object obj = GameObject.FindObjectOfType(behavior.GetType());

        if (obj  == null )
        {
           GameObject newObj = GameObject.Instantiate(ManagerToCheck,transform) as GameObject;

            if (DontDestroy == true )
            {
                newObj.transform.parent = null;

                GameObject.DontDestroyOnLoad(newObj);
            }
        }

    }
}
