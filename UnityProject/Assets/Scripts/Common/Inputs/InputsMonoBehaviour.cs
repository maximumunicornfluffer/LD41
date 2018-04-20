using UnityEngine;
using System.Collections;

public class InputsMonoBehaviour : MonoBehaviour
{

    void Update()
    {
        InputsManager.Instance.Update();
    }

}
