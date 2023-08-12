using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways()]
public class A : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).name = $"part{i}" ;
        }
    }
}
