using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTransformBreak : MonoBehaviour
{
    void Awake()
    {
        this.transform.DetachChildren();
    }
}
