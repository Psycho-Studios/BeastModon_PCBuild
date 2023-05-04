using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectRemove : MonoBehaviour
{
    public float float_timeBeforeRemoval;

    IEnumerator initiateRemoval()
    {
        yield return new WaitForSeconds(float_timeBeforeRemoval);
        this.gameObject.SetActive(false);
    }
}
