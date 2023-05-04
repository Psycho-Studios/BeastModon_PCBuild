using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Player1 : MonoBehaviour
{
    private bool bool_initialAnimationsCompleted;
    public static bool bool_rankLowerRequest_player1, bool_rankUpRequest_player1, 
        bool_beastModeRequest_player1, bool_restoreWeaponRequest;
    public GameObject[] gameObjects_weaponSlots;
    private List<Animator> animators_weaponSlots;
    private int[] array_previousWeaponValues;
    
    // Start is called before the first frame update
    private void Awake()
    {
        bool_rankUpRequest_player1 = false;
        animators_weaponSlots = new List<Animator>();
        for (int i = 0; i < 5; i++)
        {
            animators_weaponSlots.Add(gameObjects_weaponSlots[i].GetComponent<Animator>());
        }
    }

    private void Start()
    {
        bool_initialAnimationsCompleted = false;
    }


    void Update()
    {
        if (ProjectileControls_Player1.bool_weaponStrengthArrayPopulated
        && !bool_initialAnimationsCompleted)
        {
            this.array_previousWeaponValues = ProjectileControls_Player1.array_int_weaponStrength;
            for (int i = 0; i < 5; i++)
            {
                animators_weaponSlots[i].SetInteger(
                "WeaponValue",
                i + (5 * ProjectileControls_Player1.array_int_weaponStrength[i]));
            }
            bool_initialAnimationsCompleted = true;
        }

        if(bool_initialAnimationsCompleted)
        { 
            //Animate the weapon slots based on weapon type
            if ((bool_rankUpRequest_player1 || bool_rankLowerRequest_player1)
            && !bool_beastModeRequest_player1) //Runs if the player ranks up, only if beast mode is inactive
            {
                animators_weaponSlots[ProjectileControls_Player1.int_weaponIndex_player1].SetInteger
                    ("WeaponValue",
                    ProjectileControls_Player1.int_weaponValue_player1);
                if(bool_rankUpRequest_player1)
                {
                    bool_rankUpRequest_player1 = false;
                }
                else if (bool_rankLowerRequest_player1)
                {
                    bool_rankLowerRequest_player1 = false;
                }

            }
            else if (bool_beastModeRequest_player1) //Runs if beast mode is initiated
            {
                animators_weaponSlots[ProjectileControls_Player1.int_weaponIndex_player1].SetInteger
                    ("WeaponValue",
                    ProjectileControls_Player1.int_weaponValue_player1);
                bool_beastModeRequest_player1 = false;
            }
            else if (bool_restoreWeaponRequest) //Happens after beast mode is over
            {
                animators_weaponSlots[ProjectileControls_Player1.int_weaponIndex_player1].SetInteger
                    ("WeaponValue",
                    ProjectileControls_Player1.int_weaponValue_player1);
                bool_restoreWeaponRequest = false;
            }
        }
    }
}
