using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusScript_Player1 : MonoBehaviour
{
    public static Animator animator_faceBackground;

    private void Awake()
    {
        animator_faceBackground = gameObject.GetComponent<Animator>();
    }
}
