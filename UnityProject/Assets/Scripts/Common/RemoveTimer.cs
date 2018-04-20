using UnityEngine;
using System.Collections;

public class RemoveTimer : MonoBehaviour
{

    public float Timer = 1;

    void Start()
    {
        GameObject.Destroy(gameObject,Timer);
    }


}
