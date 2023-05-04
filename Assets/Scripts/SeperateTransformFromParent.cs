using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeperateTransformFromParent : MonoBehaviour
{
    public bool bool_seperateOnEnable;

    private void Start()
    {
        //Start happens after object load and initialization, OnEnable happen when the object is set active
        if (!bool_seperateOnEnable)
        {
            transform.parent = null;
        }
    }

    private void OnEnable()
    {
        if(bool_seperateOnEnable)
        {
            transform.parent = null;
        }
    }
}
