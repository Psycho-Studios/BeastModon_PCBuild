using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to the BeastMode Indicator in the Player HUD.
/// </summary>
public class BeastModeIndicator_Player1 : MonoBehaviour
{
    public static bool bool_animationAllowed;
    private Animator animator_beastModeIndicator;
    // Start is called before the first frame update
    private void Awake()
    {
        this.animator_beastModeIndicator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ProjectileControls_Player1.int_beastModesAvailable > 0)
        {
            if(!this.animator_beastModeIndicator.GetBool("BeastModesAvailable"))
            {
                this.animator_beastModeIndicator.SetBool("BeastModesAvailable", true);
                Debug.Log("BeastModeIndicator_Player1 - BeastModesAvailable: " + ProjectileControls_Player1.int_beastModesAvailable);
            }
        }
        else 
        {
            if (this.animator_beastModeIndicator.GetBool("BeastModesAvailable"))
            {
                this.animator_beastModeIndicator.SetBool("BeastModesAvailable", false);
            }
        }
    }
}
