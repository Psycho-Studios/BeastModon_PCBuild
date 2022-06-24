using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Player1 : MonoBehaviour
{
    public static bool bool_rankUpRequest_player1, bool_beastModeRequest_player1,
        bool_restoreWeaponRequest;
    public GameObject[] gameObjects_weaponSlots;
    private Animator[] animators_weaponSlots;
    
    // Start is called before the first frame update
    private void Awake()
    {
        bool_rankUpRequest_player1 = false;
        animators_weaponSlots = new Animator[5];
        for(int i = 0; i < 5; i++)
        {
            animators_weaponSlots[i] = gameObjects_weaponSlots[i].GetComponent<Animator>();
        }
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            animators_weaponSlots[i].SetInteger
               ("WeaponValue",
               i + (5 * ProjectileControls.array_int_weaponStrength[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bool_rankUpRequest_player1
        && !bool_beastModeRequest_player1)
        {
            animators_weaponSlots[ProjectileControls.int_weaponIndex_player1].SetInteger
                ("WeaponValue",
                ProjectileControls.int_weaponValue_player1);
            bool_rankUpRequest_player1 = false;
        }
        if(bool_beastModeRequest_player1)
        {
            animators_weaponSlots[ProjectileControls.int_weaponIndex_player1].SetInteger
                ("WeaponValue",
                ProjectileControls.int_weaponValue_player1);
            bool_beastModeRequest_player1 = false;
        }
        if(bool_restoreWeaponRequest)
        {
            animators_weaponSlots[ProjectileControls.int_weaponIndex_player1].SetInteger
                ("WeaponValue",
                ProjectileControls.int_weaponValue_player1);
            bool_restoreWeaponRequest = false;
        }
    }
}
